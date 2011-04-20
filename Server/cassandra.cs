using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Thrift.Transport;
using Thrift.Protocol;

namespace Cassandra
{
    public class Exception : System.Exception
    {
        internal Exception(System.Exception innerexception) : base("", innerexception) { }
        internal Exception(string message, System.Exception innerexception) : base(message, innerexception) { }
    }

    public sealed class NotFoundException : Exception { internal NotFoundException(Apache.Cassandra.NotFoundException inner) : base(inner) { } }
    public sealed class UnavailableException : Exception { internal UnavailableException(Apache.Cassandra.UnavailableException inner) : base(inner) { } }
    public sealed class TimedOutException : Exception { internal TimedOutException(Apache.Cassandra.TimedOutException inner) : base(inner) { } }
    public sealed class InvalidRequestException : Exception { public InvalidRequestException(Apache.Cassandra.InvalidRequestException inner) : base(inner.Why, inner) { } }
    public sealed class AuthenticationException : Exception { public AuthenticationException(Apache.Cassandra.AuthenticationException inner) : base(inner.Why, inner) { } }
    public sealed class AuthorizationException : Exception { public AuthorizationException(Apache.Cassandra.AuthorizationException inner) : base(inner.Why, inner) { } }

    public enum ReplicationStrategy {
        SimpleStrategy,
        UnknownStrategy,
    }

    public enum ColumnType {
        Standard,
        Super,
    }

    public enum ConsistencyLevel
    {
        One,
        Quorum,
        LocalQuorum,
        EachQuorum,
        All,
        Any,
    }

    public enum Comparator {
        BytesType,
        AsciiType,
        UTF8Type,
        LexicalUUIDType,
        TimeUUIDType,
        LongType,
    }

    public enum Validation {
        BytesType,
        AsciiType,
        UTF8Type,
        LexicalUUIDType,
        TimeUUIDType,
        LongType,
    }

    public enum IndexOperator
    {
        Eq,
        Gte,
        Gt,
        Lte,
        Lt,
    }

    public enum IndexType {
        Keys
    }

    public sealed class KeySpaceDefinition
    {
        // required fields
        public string Name { get; set; }
        public ReplicationStrategy ReplicationStrategy { get; set; }
        public int ReplicationFactor { get; set; }
        public IList<ColumnFamilyDefinition> ColumnFamilies { get; set; }

        // optional fields
        public IDictionary<string, string> ReplicationStrategyOptions { get; private set; }

        internal KeySpaceDefinition(Apache.Cassandra.KsDef ksdef) {
            Name = ksdef.Name;
            ReplicationFactor = ksdef.Replication_factor;
            ReplicationStrategy = Helpers.ParseReplicationStrategy(ksdef.Strategy_class);

            if (ksdef.Strategy_options != null)
                ReplicationStrategyOptions = new Dictionary<string, string>(ksdef.Strategy_options);
            else
                ReplicationStrategyOptions = new Dictionary<string, string>();

            if (ksdef.Cf_defs != null)
                ColumnFamilies = ksdef.Cf_defs.Select(x => new ColumnFamilyDefinition(x)).ToList();
            else
                ColumnFamilies = new List<ColumnFamilyDefinition>();
        }

        public KeySpaceDefinition Clone() {
            var ret = (KeySpaceDefinition)MemberwiseClone();
            ret.ColumnFamilies = ColumnFamilies.Select(x => x.Clone()).ToList();
            ret.ReplicationStrategyOptions = ReplicationStrategyOptions.ToDictionary(x => x.Key, x => x.Value);
            return ret;
        }

        public KeySpaceDefinition() {
            ReplicationStrategy = ReplicationStrategy.SimpleStrategy;
            ReplicationFactor = 1;
            ColumnFamilies = new List<ColumnFamilyDefinition>();
            ReplicationStrategyOptions = new Dictionary<string, string>();
        }

        internal Apache.Cassandra.KsDef ToThrift() {
            var ret = new Apache.Cassandra.KsDef();
            ret.Name = Helpers.EnsureField("Name", Name);
            ret.Replication_factor = ReplicationFactor;
            ret.Strategy_class = Helpers.SerializeReplicationStrategy(ReplicationStrategy);
            ret.Cf_defs = ColumnFamilies.Select(x => x.ToThrift()).ToList();
            if (ReplicationStrategyOptions.Count > 0) ret.Strategy_options = new Dictionary<string, string>(ReplicationStrategyOptions);
            return ret;
        }
    }

    public sealed class ColumnDefinition
    {
        // required fields
        public Value Name { get; set; }
        public Validation Validation { get; set; }

        // optional fields
        public IndexType? IndexType { get; set; }
        public string IndexName { get; set; }

        public ColumnDefinition() {
            Validation = Cassandra.Validation.BytesType;
        }

        public ColumnDefinition(Apache.Cassandra.ColumnDef def) {
            Name = def.Name;
            Validation = Helpers.ParseValidationClass(def.Validation_class);
            if (def.__isset.index_type)
                IndexType = Helpers.Convert(def.Index_type);
            if (def.__isset.index_name)
                IndexName = def.Index_name;
        }

        internal Apache.Cassandra.ColumnDef ToThrift() {
            var ret = new Apache.Cassandra.ColumnDef();
            ret.Name = Helpers.EnsureField("Name", Name);
            ret.Validation_class = Helpers.SerializeValidation(Validation);
            if (IndexType.HasValue) ret.Index_type = Helpers.Convert(IndexType.Value);
            if (IndexName != null) ret.Index_name = IndexName;
            return ret;
        }
    }

    public sealed class ColumnFamilyDefinition
    {
        // required fields
        public string Keyspace { get; set; }
        public string Name { get; set; }

        // optional fields
        public Comparator? ComparatorType { get; set; }
        public Comparator? SubComparatorType { get; set; }
        public string Comment { get; set; }
        public double? RowCacheSize { get; set; }
        public double? KeyCacheSize { get; set; }
        public double? ReadRepairChance { get; set; }
        public IList<ColumnDefinition> Columns { get; private set; }
        public int? GcGraceSeconds { get; set; }
        public Validation? DefaultValidationClass { get; set; }
        public int? Id { get; set; }
        public int? MinCompactionThreshold { get; set; }
        public int? MaxCompactionThreshold { get; set; }
        public int? RowCacheSavePeriodInSeconds { get; set; }
        public int? KeyCacheSavePeriodInSeconds { get; set; }
        public int? MemtableFlushAfterMins { get; set; }
        public int? MemtableThroughputInMb { get; set; }
        public double? MemtableOperationsInMillions { get; set; }

        internal ColumnFamilyDefinition(Apache.Cassandra.CfDef def) {
            Keyspace = def.Keyspace;
            Name = def.Name;
            ComparatorType = Helpers.ParseOptionalComparator(def.Comparator_type);
            SubComparatorType = Helpers.ParseOptionalComparator(def.Subcomparator_type);
            Comment = def.Comment;
            if (def.__isset.row_cache_size) RowCacheSize = def.Row_cache_size;
            if (def.__isset.key_cache_size) KeyCacheSize = def.Key_cache_size;
            if (def.__isset.read_repair_chance) ReadRepairChance = def.Read_repair_chance;
            if (def.__isset.gc_grace_seconds) GcGraceSeconds = def.Gc_grace_seconds;
            if (def.__isset.id) Id = def.Id;
            if (def.__isset.min_compaction_threshold) MinCompactionThreshold = def.Min_compaction_threshold;
            if (def.__isset.max_compaction_threshold) MaxCompactionThreshold = def.Max_compaction_threshold;
            if (def.__isset.row_cache_save_period_in_seconds) RowCacheSavePeriodInSeconds = def.Row_cache_save_period_in_seconds;
            if (def.__isset.key_cache_save_period_in_seconds) KeyCacheSavePeriodInSeconds = def.Key_cache_save_period_in_seconds;
            if (def.__isset.memtable_flush_after_mins) MemtableFlushAfterMins = def.Memtable_flush_after_mins;
            if (def.__isset.memtable_throughput_in_mb) MemtableThroughputInMb = def.Memtable_throughput_in_mb;
            if (def.__isset.memtable_operations_in_millions) MemtableOperationsInMillions = def.Memtable_operations_in_millions;
            if (def.__isset.default_validation_class) DefaultValidationClass = Helpers.ParseOptionalValidationClass(def.Default_validation_class);
            if (def.Column_metadata != null)
                Columns = def.Column_metadata.Select(x => new ColumnDefinition(x)).ToList();
            else
                Columns = new List<ColumnDefinition>();
        }

        public ColumnFamilyDefinition() {
            Columns = new List<ColumnDefinition>();
        }

        internal Apache.Cassandra.CfDef ToThrift() {
            Apache.Cassandra.CfDef ret = new Apache.Cassandra.CfDef();
            ret.Name = Helpers.EnsureField("Name", Name);
            ret.Keyspace = Helpers.EnsureField("Keyspace", Keyspace);
            if (ComparatorType.HasValue) ret.Comparator_type = Helpers.SerializeComparator(ComparatorType.Value);
            if (SubComparatorType.HasValue) ret.Subcomparator_type = SubComparatorType.Value.ToString();
            if (Comment != null) ret.Comment = Comment;
            if (RowCacheSize.HasValue) ret.Row_cache_size = RowCacheSize.Value;
            if (KeyCacheSize.HasValue) ret.Key_cache_size = KeyCacheSize.Value;
            if (ReadRepairChance.HasValue) ret.Read_repair_chance = ReadRepairChance.Value;
            if (Columns.Count > 0) ret.Column_metadata = Columns.Select(x => x.ToThrift()).ToList();
            if (GcGraceSeconds.HasValue) ret.Gc_grace_seconds = GcGraceSeconds.Value;
            if (DefaultValidationClass.HasValue) ret.Default_validation_class = Helpers.SerializeValidation(DefaultValidationClass.Value);
            if (Id.HasValue) ret.Id = Id.Value;
            if (MinCompactionThreshold.HasValue) ret.Min_compaction_threshold = MinCompactionThreshold.Value;
            if (MaxCompactionThreshold.HasValue) ret.Max_compaction_threshold = MaxCompactionThreshold.Value;
            if (RowCacheSavePeriodInSeconds.HasValue) ret.Row_cache_save_period_in_seconds = RowCacheSavePeriodInSeconds.Value;
            if (KeyCacheSavePeriodInSeconds.HasValue) ret.Key_cache_save_period_in_seconds = KeyCacheSavePeriodInSeconds.Value;
            if (MemtableFlushAfterMins.HasValue) ret.Memtable_flush_after_mins = MemtableFlushAfterMins.Value;
            if (MemtableThroughputInMb.HasValue) ret.Memtable_throughput_in_mb = MemtableThroughputInMb.Value;
            if (MemtableOperationsInMillions.HasValue) ret.Memtable_operations_in_millions = MemtableOperationsInMillions.Value;
            return ret;
        }

        internal ColumnFamilyDefinition Clone() {
            ColumnFamilyDefinition ret = (ColumnFamilyDefinition)MemberwiseClone();
            ret.Columns = ret.Columns.ToList();
            return ret;
        }
    }

    public sealed class Value
    {
        byte[] _binary;

        public Value(byte[] v) { _binary = v; }
        public Value(string v) { _binary = Helpers.ToBinary(v); }
        public Value(int v) { _binary = Helpers.ToBinary(v); }
        public Value(long v) { _binary = Helpers.ToBinary(v); }
        public Value(double v) { _binary = Helpers.ToBinary(v); }
        public Value(bool v) { _binary = Helpers.ToBinary(v); }
        public Value(Guid v) { _binary = Helpers.ToBinary(v); }

        public static implicit operator Value(byte[] v) { return new Value(v); }
        public static implicit operator Value(string v) { return new Value(v); }
        public static implicit operator Value(int v) { return new Value(v); }
        public static implicit operator Value(long v) { return new Value(v); }
        public static implicit operator Value(double v) { return new Value(v); }
        public static implicit operator Value(bool v) { return new Value(v); }
        public static implicit operator Value(Guid v) { return new Value(v); }

        public static implicit operator byte[](Value v) { return v.ToBinary(); }
        public static implicit operator string(Value v) { return v.ToString(); }
        public static implicit operator int(Value v) { return v.ToInt(); }
        public static implicit operator long(Value v) { return v.ToLong(); }
        public static implicit operator double(Value v) { return v.ToDouble(); }
        public static implicit operator bool(Value v) { return v.ToBool(); }
        public static implicit operator Guid(Value v) { return v.ToGuid(); }

        public byte[] ToBinary() { return _binary; }
        public new string ToString() { return Helpers.ToString(_binary); }
        public int ToInt() { return Helpers.ToInt(_binary); }
        public long ToLong() { return Helpers.ToLong(_binary); }
        public double ToDouble() { return Helpers.ToDouble(_binary); }
        public bool ToBool() { return Helpers.ToBool(_binary); }
        public Guid ToGuid() { return Helpers.ToGuid(_binary); }
    }

    public sealed class Column
    {
        // required fields
        public Value Name     { get; set; }
        public Value Value    { get; set; }
        public long Timestamp { get; set; }

        public Column(Apache.Cassandra.Column col) { 
            Name = col.Name; 
            Value = col.Value; 
            Timestamp = col.Timestamp;
            if (col.__isset.ttl)
                TTL = col.Ttl;
        }

        public Column() { }
        public Column(Value name , Value value, long timestamp) { Name = name; Value = value; Timestamp = timestamp; }

        // optional field
        public int? TTL { get; set; }

        internal Apache.Cassandra.Column ToThrift() {
            var ret = new Apache.Cassandra.Column() {
                Name = Helpers.EnsureField("Name", Name),
                Value = Helpers.EnsureField("*Value", Value),
                Timestamp = Timestamp,
            };
            if (TTL.HasValue)
                ret.Ttl = TTL.Value;
            return ret;
        }
    }

    public sealed class SuperColumn
    {
        public Value Name { get; set; }
        public IList<Column> Columns { get; set; }

        public SuperColumn() { Columns = new List<Column>(); }
        public SuperColumn(string name, IEnumerable<Column> columns) { Name = name; Columns = new List<Column>(columns); }

        internal SuperColumn(Apache.Cassandra.SuperColumn supercol) {
            Name = supercol.Name;
            if (supercol.Columns != null)
                Columns = supercol.Columns.Select(x => new Column(x)).ToList();
            else
                Columns = new List<Column>();
        }

        internal Apache.Cassandra.SuperColumn ToThrift() {
            var ret = new Apache.Cassandra.SuperColumn();
            ret.Name = Helpers.EnsureField("Name", Name);
            ret.Columns = Columns.Select(x => x.ToThrift()).ToList();
            return ret;
        }
    }

    public sealed class ColumnOrSuperColumn
    {
        // optional fields
        public Column Column { get; set; }
        public SuperColumn SuperColumn { get; set; }

        public static implicit operator ColumnOrSuperColumn(Column col) { return new ColumnOrSuperColumn(col); }
        public static implicit operator ColumnOrSuperColumn(SuperColumn col) { return new ColumnOrSuperColumn(col); }

        public ColumnOrSuperColumn() { }
        public ColumnOrSuperColumn(Column col) { Column = col; }
        public ColumnOrSuperColumn(SuperColumn col) { SuperColumn = col; }

        internal Apache.Cassandra.ColumnOrSuperColumn ToThrift() {
            var ret = new Apache.Cassandra.ColumnOrSuperColumn();
            if (Column == null && SuperColumn == null)
                throw new NullReferenceException("either Column or SuperColumn must be provided");
            if (Column != null && SuperColumn != null)
                throw new NullReferenceException("at most one of Column or SuperColumn must be provided");
            if (Column != null)
                ret.Column = Column.ToThrift();
            if (SuperColumn != null)
                ret.Super_column = SuperColumn.ToThrift();
            return ret;
        }

        internal ColumnOrSuperColumn(Apache.Cassandra.ColumnOrSuperColumn cosc) {
            if (cosc.Column != null)
                Column = new Column(cosc.Column);
            else if (cosc.Super_column != null)
                SuperColumn = new SuperColumn(cosc.Super_column);
            else
                throw new InvalidOperationException("invalid thrift ColumnOrSuperColumn--at least one of Column or SuperColumn must be non-null");
        }
    }

    public sealed class ColumnParent
    {
        // required fields
        public string ColumnFamily { get; set; }

        // optional fields
        public Value SuperColumn {get;set;}

        public ColumnParent() { }
        public ColumnParent(string columnfamily) { ColumnFamily = columnfamily; }
        public ColumnParent(string columnfamily, Value supercolumn) : this(columnfamily) { SuperColumn = supercolumn; }

        internal ColumnParent(Apache.Cassandra.ColumnParent par) {
            ColumnFamily = par.Column_family;
            SuperColumn = par.Super_column;
        }

        internal Apache.Cassandra.ColumnParent ToThrift() {
            var ret = new Apache.Cassandra.ColumnParent();
            ret.Column_family = Helpers.EnsureField("ColumnFamily", ColumnFamily);
            if (SuperColumn != null) ret.Super_column = SuperColumn;
            return ret;
        }
    }

    public sealed class ColumnPath
    {
        // required fields
        public string ColumnFamily { get; set; }

        // optional fields
        public Value SuperColumn { get; set; }
        public Value Column { get; set; }

        public ColumnPath() { }
        public ColumnPath(string columnfamily, Value column) {
            ColumnFamily = columnfamily;
            Column = column;
        }
        public ColumnPath(string columnfamily, Value supercolumn, Value column) { 
            ColumnFamily = columnfamily;
            SuperColumn = supercolumn;
            Column = column;
        }

        internal ColumnPath(Apache.Cassandra.ColumnPath path) {
            ColumnFamily = path.Column_family;
            SuperColumn = path.Super_column;
            Column = path.Column;
        }

        internal Apache.Cassandra.ColumnPath ToThrift() {
            var ret = new Apache.Cassandra.ColumnPath();
            ret.Column_family = Helpers.EnsureField("ColumnFamily", ColumnFamily);
            if (SuperColumn != null) ret.Super_column = SuperColumn;
            if (Column != null) ret.Column = Column;
            return ret;
        }
    }

    public sealed class SliceRange
    {
        // required fields
        public Value Start { get; set; }
        public Value Finish { get; set; }
        public bool Reversed { get; set; }
        public int Count { get; set; }

        public SliceRange() { Reversed = false; Count = 100; }

        internal SliceRange(Apache.Cassandra.SliceRange range) {
            Start = range.Start;
            Finish = range.Finish;
            Reversed = range.Reversed;
            Count = range.Count;
        }

        internal Apache.Cassandra.SliceRange ToThrift() {
            return new Apache.Cassandra.SliceRange() {
                Start = Helpers.EnsureField("Start", Start),
                Finish = Helpers.EnsureField("Finish", Finish),
                Reversed = Reversed,
                Count = Count,
            };
        }
    }

    public sealed class SlicePredicate
    {
        // optional fields
        public IList<Value> ColumnNames { get; set; }
        public SliceRange SliceRange { get; set; }

        public SlicePredicate() { ColumnNames = new List<Value>(); }

        internal SlicePredicate(Apache.Cassandra.SlicePredicate predicate) {
            if (predicate.Slice_range != null)
                SliceRange = new SliceRange(predicate.Slice_range);
            ColumnNames = new List<Value>();
            if (predicate.Column_names != null)
                foreach (var name in predicate.Column_names)
                    ColumnNames.Add(name);
        }

        internal Apache.Cassandra.SlicePredicate ToThrift() {
            var ret =  new Apache.Cassandra.SlicePredicate();
            if (ColumnNames.Count > 0) ret.Column_names = ColumnNames.Select(x => x.ToBinary()).ToList();
            if (SliceRange != null) ret.Slice_range = SliceRange.ToThrift();
            return ret;
        }
    }

    public sealed class IndexExpression
    {
        // required fields
        public Value ColumnName { get; set; }
        public IndexOperator Operator { get; set; }
        public Value Value { get; set; }

        internal IndexExpression(Apache.Cassandra.IndexExpression expr) {
            ColumnName = expr.Column_name;
            Operator = Helpers.Convert(expr.Op);
            Value = expr.Value;
        }

        public IndexExpression() { Operator = IndexOperator.Eq; }

        internal Apache.Cassandra.IndexExpression ToThrift() {
            return new Apache.Cassandra.IndexExpression() {
                Column_name = Helpers.EnsureField("ColumnName", ColumnName),
                Value = Helpers.EnsureField("Value", Value),
                Op = Helpers.Convert(Operator),
            };
        }
    }

    public sealed class IndexClause
    {
        public IList<IndexExpression> Expressions { get; set; }
        public Value StartKey { get; set; }
        public int Count { get; set; }

        public IndexClause() { Count = 100; Expressions = new List<IndexExpression>(); }

        internal IndexClause(Apache.Cassandra.IndexClause clause) {
            StartKey = clause.Start_key;
            if (clause.Expressions != null)
                Expressions = clause.Expressions.Select(x => new IndexExpression(x)).ToList();
            else
                Expressions = new List<IndexExpression>();
            Count = clause.Count;
        }

        public Apache.Cassandra.IndexClause ToThrift() {
            return new Apache.Cassandra.IndexClause() {
                Expressions = Expressions.Select(x=>x.ToThrift()).ToList(),
                Start_key = Helpers.EnsureField("StartKey", StartKey),
                Count = Count,
            };
        }
    }

    public sealed class KeyRange
    {
        // required fields
        public int Count { get; set; }

        // optional fields
        public Value StartKey { get; set; }
        public Value EndKey { get; set; }
        public string StartToken { get; set; }
        public string EndToken { get; set; }

        public KeyRange() { Count = 100; }

        internal KeyRange(Apache.Cassandra.KeyRange range) {
            Count = range.Count;
            if (range.Start_key != null) StartKey = range.Start_key;
            if (range.End_key != null) EndKey = range.End_key;
            if (range.Start_token != null) StartToken = range.Start_token;
            if (range.End_token != null) EndToken = range.End_token;
        }

        internal Apache.Cassandra.KeyRange ToThrift() {
            var ret = new Apache.Cassandra.KeyRange();
            ret.Count = Count;
            if (StartKey != null) ret.Start_key = StartKey;
            if (EndKey != null) ret.End_key = EndKey;
            if (StartToken != null) ret.Start_token = StartToken;
            if (EndToken != null) ret.End_token = EndToken;
            return ret;
        }
    }

    public sealed class KeySlice
    {
        // required fields
        public Value Key { get; set; }
        public IList<ColumnOrSuperColumn> Columns { get; set; }

        public KeySlice() {
            Columns = new List<ColumnOrSuperColumn>();
        }

        internal KeySlice(Apache.Cassandra.KeySlice slice) {
            Key = slice.Key;
            Columns = slice.Columns.Select(x => new ColumnOrSuperColumn(x)).ToList();
        }

        internal Apache.Cassandra.KeySlice ToThrift() {
            var ret = new Apache.Cassandra.KeySlice();
            ret.Key = Helpers.EnsureField("Key", Key);
            ret.Columns = Columns.Select(x => x.ToThrift()).ToList();
            return ret;
        }
    }

    public sealed class KeyCount
    {
        // required fields
        public Value Key { get; set; }
        public int Count { get; set; }

        public KeyCount() {}

        internal KeyCount(Apache.Cassandra.KeyCount keycount) {
            Key = keycount.Key;
            Count = keycount.Count;
        }

        internal Apache.Cassandra.KeyCount ToThrift() {
            var ret = new Apache.Cassandra.KeyCount();
            ret.Key = Helpers.EnsureField("Key", Key);
            ret.Count = Count;
            return ret;
        }
    }

    public sealed class Deletion
    {
        // required fields
        public long Timestamp { get; set; }

        // optional fields
        public Value SuperColumn { get; set; }
        public SlicePredicate Predicate { get; set; }

        public Deletion() { }

        internal Deletion(Apache.Cassandra.Deletion del) {
            Timestamp = del.Timestamp;
            if (del.Super_column != null) SuperColumn = del.Super_column;
            if (del.Predicate != null) Predicate = new SlicePredicate(del.Predicate);
        }

        internal Apache.Cassandra.Deletion ToThrift() {
            var ret = new Apache.Cassandra.Deletion();
            ret.Timestamp = Timestamp;
            if (SuperColumn != null) ret.Super_column = SuperColumn;
            if (ret.Predicate != null) ret.Predicate = Predicate.ToThrift(); 
            return ret;
        }
    }

    public sealed class Mutation
    {
        // optional fields
        public ColumnOrSuperColumn ColumnOrSuperColumn { get; set; }
        public Deletion Deletion { get; set; }

        public Mutation() { }

        internal Mutation(Apache.Cassandra.Mutation mut) {
            if (mut.Column_or_supercolumn != null) ColumnOrSuperColumn = new ColumnOrSuperColumn(mut.Column_or_supercolumn);
            if (mut.Deletion != null) Deletion = new Deletion(mut.Deletion);
        }

        internal Apache.Cassandra.Mutation ToThrift() {
            var ret = new Apache.Cassandra.Mutation();
            if (ColumnOrSuperColumn != null) ret.Column_or_supercolumn = ColumnOrSuperColumn.ToThrift();
            if (Deletion != null) ret.Deletion = Deletion.ToThrift();
            return ret;
        }
    }

    public sealed class TokenRange
    {
        // required fields
        public string StartToken { get; set; }
        public string EndToken { get; set; }
        public IList<string> Endpoints { get; set; }

        public TokenRange() { Endpoints = new List<string>(); }

        internal TokenRange(Apache.Cassandra.TokenRange range) {
            StartToken = range.Start_token;
            EndToken = range.End_token;
            if (range.Endpoints != null)
                Endpoints = new List<string>(range.Endpoints);
            else
                Endpoints = new List<string>();
        }

        internal Apache.Cassandra.TokenRange ToThrift() {
            return new Apache.Cassandra.TokenRange() {
                Start_token = StartToken,
                End_token = EndToken,
                Endpoints = Endpoints.ToList(),
            };
        }
    }

    public sealed class AuthenticationRequest
    {
        // required fields
        public IDictionary<string, string> Credentials { get; set; }

        public AuthenticationRequest() { Credentials = new Dictionary<string, string>(); }

        internal AuthenticationRequest(Apache.Cassandra.AuthenticationRequest req) {
            if (req.Credentials != null)
                Credentials = new Dictionary<string, string>(req.Credentials);
            else
                Credentials = new Dictionary<string, string>();
        }

        internal Apache.Cassandra.AuthenticationRequest ToThrift() {
            return new Apache.Cassandra.AuthenticationRequest() {
                Credentials = new Dictionary<string, string>(Credentials)
            };
        }
    }

    public class Connection : IDisposable
    {
        const int DefaultPort = 9160;
        const int DefaultTimeout = 30000;
        const string DefaultHost = "localhost";

        TSocket _socket;
        TFramedTransport _transport;
        TProtocol _protocol;
        Apache.Cassandra.Cassandra.Client _client;

        public ConsistencyLevel DefaultConsistencyLevel { get; set; }

        public Connection()
            : this(DefaultHost, DefaultPort, DefaultTimeout) { }

        public Connection(string host)
            : this(host, DefaultPort, DefaultTimeout) { }

        public Connection(string host, int port)
            : this(host, port, DefaultTimeout) { }

        public Connection(string host, int port, int timeout) {
            _socket = new TSocket(host, port, timeout);
            _transport = new TFramedTransport(_socket);
            _transport.Open();
            _protocol = new TBinaryProtocol(_transport);
            _client = new Apache.Cassandra.Cassandra.Client(_protocol);
            DefaultConsistencyLevel = ConsistencyLevel.Quorum;
        }

        public void Login(AuthenticationRequest req) {
            _Wrap(() => _client.login(req.ToThrift()));
        }

        public void SetKeyspace(string keyspace) {
            _Wrap(() => _client.set_keyspace(keyspace));
        }

        public ColumnOrSuperColumn Get(Value key, ColumnPath path, ConsistencyLevel consistencylevel) {
            return new ColumnOrSuperColumn(_WrapValue(() => _client.get(key, path.ToThrift(), Helpers.Convert(consistencylevel))));
        }
        public ColumnOrSuperColumn Get(Value key, ColumnPath path) { 
            return Get(key, path, DefaultConsistencyLevel); 
        }

        public bool TryGet(Value key, ColumnPath path, ConsistencyLevel consistencylevel, out SuperColumn ret) {
            try {
                ret = Get(key, path, consistencylevel).SuperColumn;
                return true;
            } catch (Cassandra.NotFoundException) {
                ret = null;
                return false;
            }
        }

        public bool TryGet(Value key, ColumnPath path, out SuperColumn ret) {
            return TryGet(key, path, DefaultConsistencyLevel, out ret);
        }

        public bool TryGet(Value key, ColumnPath path, ConsistencyLevel consistencylevel, out int ret) {
            try {
                ret = (int)Get(key, path, consistencylevel).Column.Value;
                return true;
            } catch (Cassandra.NotFoundException) {
                ret = default(int);
                return false;
            }
        }

        public bool TryGet(Value key, ColumnPath path, out int ret) {
            return TryGet(key, path, DefaultConsistencyLevel, out ret);
        }

        public bool TryGet(Value key, ColumnPath path, ConsistencyLevel consistencylevel, out string ret) {
            try {
                ret = (string)Get(key, path, consistencylevel).Column.Value;
                return true;
            } catch (Cassandra.NotFoundException) {
                ret = default(string);
                return false;
            }
        }

        public bool TryGet(Value key, ColumnPath path, out string ret) {
            return TryGet(key, path, DefaultConsistencyLevel, out ret);
        }

        public bool TryGet(Value key, ColumnPath path, ConsistencyLevel consistencylevel, out double ret) {
            try {
                ret = (double)Get(key, path, consistencylevel).Column.Value;
                return true;
            } catch (Cassandra.NotFoundException) {
                ret = default(double);
                return false;
            }
        }

        public bool TryGet(Value key, ColumnPath path, out double ret) {
            return TryGet(key, path, DefaultConsistencyLevel, out ret);
        }

        public bool TryGet(Value key, ColumnPath path, ConsistencyLevel consistencylevel, out bool ret) {
            try {
                ret = (bool)Get(key, path, consistencylevel).Column.Value;
                return true;
            } catch (Cassandra.NotFoundException) {
                ret = default(bool);
                return false;
            }
        }

        public bool TryGet(Value key, ColumnPath path, out bool ret) {
            return TryGet(key, path, DefaultConsistencyLevel, out ret);
        }

        public bool TryGet(Value key, ColumnPath path, ConsistencyLevel consistencylevel, out long ret) {
            try {
                ret = (long)Get(key, path, consistencylevel).Column.Value;
                return true;
            } catch (Cassandra.NotFoundException) {
                ret = default(long);
                return false;
            }
        }

        public bool TryGet(Value key, ColumnPath path, out long ret) {
            return TryGet(key, path, DefaultConsistencyLevel, out ret);
        }

        public bool TryGet(Value key, ColumnPath path, ConsistencyLevel consistencylevel, out Column ret) {
            try {
                ret = Get(key, path, consistencylevel).Column;
                return true;
            } catch (Cassandra.NotFoundException) {
                ret = null;
                return false;
            }
        }

        public bool TryGet(Value key, ColumnPath path, out Column ret) {
            return TryGet(key, path, DefaultConsistencyLevel, out ret);
        }

        public bool TryGet(Value key, ColumnPath path, ConsistencyLevel consistencylevel, out ColumnOrSuperColumn ret) {
            try {
                ret = Get(key, path, consistencylevel);
                return true;
            } catch (Cassandra.NotFoundException) {
                ret = null;
                return false;
            }
        }

        public bool TryGet(Value key, ColumnPath path, out ColumnOrSuperColumn ret) {
            return TryGet(key, path, DefaultConsistencyLevel, out ret);
        }

        public IList<ColumnOrSuperColumn> GetSlice(Value key, ColumnParent parent, SlicePredicate pred, ConsistencyLevel consistencylevel) {
            return _WrapValue(() => _client.get_slice(key, parent.ToThrift(), pred.ToThrift(), Helpers.Convert(consistencylevel))).Select(x => new ColumnOrSuperColumn(x)).ToList();
        }
        public IList<ColumnOrSuperColumn> GetSlice(Value key, ColumnParent parent, SlicePredicate pred) {
            return GetSlice(key, parent, pred, DefaultConsistencyLevel);
        }

        public int GetCount(Value key, ColumnParent parent, SlicePredicate pred, ConsistencyLevel consistencylevel) {
            return _WrapValue(() => _client.get_count(key, parent.ToThrift(), pred.ToThrift(), Helpers.Convert(consistencylevel)));
        }
        public int GetCount(Value key, ColumnParent parent, SlicePredicate pred) {
            return GetCount(key, parent, pred, DefaultConsistencyLevel);
        }

        public IDictionary<Value,IList<ColumnOrSuperColumn>> MultiGetSlice(IList<Value> key, ColumnParent parent, SlicePredicate pred, ConsistencyLevel consistencylevel) {
            return Helpers.Convert(_WrapValue(() => _client.multiget_slice(Helpers.Convert(key), parent.ToThrift(), pred.ToThrift(), Helpers.Convert(consistencylevel))));
        }
        public IDictionary<Value,IList<ColumnOrSuperColumn>> MultiGetSlice(IList<Value> key, ColumnParent parent, SlicePredicate pred) {
            return MultiGetSlice(key, parent, pred, DefaultConsistencyLevel);
        }

        public IDictionary<Value, int> MultiGetCount(IList<Value> key, ColumnParent parent, SlicePredicate pred, ConsistencyLevel consistencylevel) {
            return Helpers.Convert(_WrapValue(() => _client.multiget_count(Helpers.Convert(key), parent.ToThrift(), pred.ToThrift(), Helpers.Convert(consistencylevel))));
        }
        public IDictionary<Value, int> MultiGetCount(IList<Value> key, ColumnParent parent, SlicePredicate pred) {
            return MultiGetCount(key, parent, pred, DefaultConsistencyLevel);
        }

        public IList<KeySlice> GetRangeSlices(ColumnParent parent, SlicePredicate pred, KeyRange range, ConsistencyLevel consistencylevel) {
            return _WrapValue(() => _client.get_range_slices(parent.ToThrift(), pred.ToThrift(), range.ToThrift(), Helpers.Convert(consistencylevel))).Select(x => new KeySlice(x)).ToList();
        }
        public IList<KeySlice> GetRangeSlices(ColumnParent parent, SlicePredicate pred, KeyRange range) {
            return GetRangeSlices(parent, pred, range, DefaultConsistencyLevel);
        }

        public IList<KeySlice> GetIndexedSlices(ColumnParent parent, IndexClause clause, SlicePredicate pred, ConsistencyLevel consistencylevel) {
            return _WrapValue(() => _client.get_indexed_slices(parent.ToThrift(), clause.ToThrift(), pred.ToThrift(), Helpers.Convert(consistencylevel))).Select(x => new KeySlice(x)).ToList();
        }
        public IList<KeySlice> GetIndexedSlices(ColumnParent parent, IndexClause clause, SlicePredicate pred) {
            return GetIndexedSlices(parent, clause, pred, DefaultConsistencyLevel);
        }

        public void Insert(Value key, ColumnParent parent, Column column, ConsistencyLevel consistencylevel) {
            _Wrap(() => _client.insert(key, parent.ToThrift(), column.ToThrift(), Helpers.Convert(consistencylevel)));
        }
        public void Insert(Value key, ColumnParent parent, Column column) {
            Insert(key, parent, column, DefaultConsistencyLevel);
        }

        public void Remove(Value key, ColumnPath path, long timestamp, ConsistencyLevel consistencylevel) {
            _Wrap(() => _client.remove(key, path.ToThrift(), timestamp, Helpers.Convert(consistencylevel)));
        }
        public void Remove(Value key, ColumnPath path, long timestamp) {
            Remove(key, path, timestamp, DefaultConsistencyLevel);
        }

        public void BatchMutate(IDictionary<Value, IDictionary<string, IList<Mutation>>> mutations, ConsistencyLevel consistencylevel) {
            _Wrap(() => _client.batch_mutate(Helpers.Convert(mutations), Helpers.Convert(consistencylevel)));
        }
        public void BatchMutate(IDictionary<Value, IDictionary<string, IList<Mutation>>> mutations) {
            BatchMutate(mutations, DefaultConsistencyLevel);
        }

        public void Truncate(string cfname) {
            _Wrap(() => _client.truncate(cfname));
        }

        public IDictionary<string, IList<string>> DescribeSchemaVersions() {
            return Helpers.Convert(_WrapValue(() => _client.describe_schema_versions()));
        }

        public IList<KeySpaceDefinition> DescribeKeySpaces() {
            return Helpers.Convert(_WrapValue(() => _client.describe_keyspaces()));
        }

        public KeySpaceDefinition DescribeKeySpace(string keyspace) {
            return new KeySpaceDefinition(_WrapValue(() => _client.describe_keyspace(keyspace)));
        }

        public string DescribeClusterName() {
            return _WrapValue(() => _client.describe_cluster_name());
        }

        public string DescribeVersion() {
            return _WrapValue(() => _client.describe_version());
        }

        public string DescribePartitioner() {
            return _WrapValue(() => _client.describe_partitioner());
        }

        public string DescribeSnitch() {
            return _WrapValue(() => _client.describe_snitch());
        }

        public IList<TokenRange> DescribeRing(string keyspace) {
            return Helpers.Convert(_WrapValue(() => _client.describe_ring(keyspace)));
        }

        public string SystemAddColumnFamily(ColumnFamilyDefinition def) {
            return _WrapValue(() => _client.system_add_column_family(def.ToThrift()));
        }

        public string SystemDropColumnFamily(string cf) {
            return _WrapValue(() => _client.system_drop_column_family(cf));
        }

        public string SystemAddKeyspace(KeySpaceDefinition def) {
            return _WrapValue(() => _client.system_add_keyspace(def.ToThrift()));
        }

        public string SystemDropKeyspace(string cf) {
            return _WrapValue(() => _client.system_drop_keyspace(cf));
        }

        public string SystemUpdateKeyspace(KeySpaceDefinition def) {
            return _WrapValue(() => _client.system_update_keyspace(def.ToThrift()));
        }

        public string SystemUpdateColumnFamily(ColumnFamilyDefinition def) {
            return _WrapValue(() => _client.system_update_column_family(def.ToThrift()));
        }

        delegate T Returns<T>();
        private T _WrapValue<T>(Returns<T> a) {
            try {
                return a();
            } catch (Apache.Cassandra.NotFoundException e) {
                throw new NotFoundException(e);
            } catch (Apache.Cassandra.InvalidRequestException e) {
                throw new InvalidRequestException(e);
            } catch (Apache.Cassandra.AuthenticationException e) {
                throw new AuthenticationException(e);
            } catch (Apache.Cassandra.AuthorizationException e) {
                throw new AuthorizationException(e);
            } catch (Apache.Cassandra.TimedOutException e) {
                throw new TimedOutException(e);
            } catch (Apache.Cassandra.UnavailableException e) {
                throw new UnavailableException(e);
            }
        }

        private void _Wrap(Action a) {
            try {
                a();
            } catch (Apache.Cassandra.NotFoundException e) {
                throw new NotFoundException(e);
            } catch (Apache.Cassandra.InvalidRequestException e) {
                throw new InvalidRequestException(e);
            } catch (Apache.Cassandra.AuthenticationException e) {
                throw new AuthenticationException(e);
            } catch (Apache.Cassandra.AuthorizationException e) {
                throw new AuthorizationException(e);
            } catch (Apache.Cassandra.TimedOutException e) {
                throw new TimedOutException(e);
            } catch (Apache.Cassandra.UnavailableException e) {
                throw new UnavailableException(e);
            }
        }

        public void Dispose() {
            _transport.Close();
            _socket.Close();
        }
    }

    internal static class Helpers
    {
        public static T EnsureField<T>(string fieldname, T value) where T : class {
            if (value == null) throw new NullReferenceException(fieldname + " must be provided");
            return value;
        }
        public static ReplicationStrategy ParseReplicationStrategy(string strategy) {
            switch (strategy) {
                case "org.apache.cassandra.locator.SimpleStrategy":   return ReplicationStrategy.SimpleStrategy;
                default:                                                 return ReplicationStrategy.UnknownStrategy;
            }
        }
        public static string SerializeReplicationStrategy(ReplicationStrategy strategy) {
            switch (strategy) {
                case ReplicationStrategy.SimpleStrategy: return "org.apache.cassandra.locator.SimpleStrategy";
                default: throw new InvalidOperationException();
            }
        }
        internal static string SerializeComparator(Comparator comparator) { 
            return comparator.ToString(); 
        }
        internal static Comparator? ParseOptionalComparator(string type) {
            if (type == null) return null;
            int iof = type.LastIndexOf('.');
            if (iof != -1) type = type.Substring(iof + 1);
            return (Comparator)Enum.Parse(typeof(Comparator), type, true);
        }
        internal static string SerializeValidation(Validation validation) { 
            return validation.ToString(); 
        }
        internal static Validation? ParseOptionalValidationClass(string validation) {
            if (validation == null) return null;
            return ParseValidationClass(validation);
        }
        internal static Validation ParseValidationClass(string validation) {
            int iof = validation.LastIndexOf('.');
            if (iof != -1) validation = validation.Substring(iof + 1);
            return (Validation)Enum.Parse(typeof(Validation), validation, true);
        }
        internal static IndexType Convert(Apache.Cassandra.IndexType indexType) {
            switch (indexType) {
                case Apache.Cassandra.IndexType.KEYS: return IndexType.Keys;
                default: throw new InvalidOperationException();
            }
        }
        internal static Apache.Cassandra.IndexType Convert(IndexType indexType) {
            switch (indexType) {
                case IndexType.Keys: return Apache.Cassandra.IndexType.KEYS;
                default: throw new InvalidOperationException();
            }
        }

        internal static byte[] ToBinary(Guid value)   { return value.ToByteArray();           }
        internal static byte[] ToBinary(bool value)   { return BitConverter.GetBytes(value);  }
        internal static byte[] ToBinary(long value)   { return BitConverter.GetBytes(value);  }
        internal static byte[] ToBinary(int value)    { return BitConverter.GetBytes(value);  }
        internal static byte[] ToBinary(string value) { return Encoding.UTF8.GetBytes(value); }
        internal static byte[] ToBinary(double value) { return BitConverter.GetBytes(value);  }

        internal static string ToString(byte[] value) { return Encoding.UTF8.GetString(value);   }
        internal static double ToDouble(byte[] value) { return BitConverter.ToDouble(value, 0);  }
        internal static int    ToInt(byte[] value)    { return BitConverter.ToInt32(value, 0);   }
        internal static bool   ToBool(byte[] value)   { return BitConverter.ToBoolean(value, 0); }
        internal static Guid   ToGuid(byte[] value)   { return new Guid(value);                  }
        internal static long   ToLong(byte[] value)   { return BitConverter.ToInt64(value, 0);   }

        internal static byte[] ToBinaryOptional(string value) { return value == null ? null : ToBinary(value); }
        internal static string ToStringOptional(byte[] value) { return value == null ? null : ToString(value); }

        internal static Apache.Cassandra.IndexOperator Convert(IndexOperator op) {
            switch (op) {
                case IndexOperator.Eq: return Apache.Cassandra.IndexOperator.EQ;
                case IndexOperator.Gte: return Apache.Cassandra.IndexOperator.GTE;
                case IndexOperator.Gt: return Apache.Cassandra.IndexOperator.GT;
                case IndexOperator.Lte: return Apache.Cassandra.IndexOperator.LTE;
                case IndexOperator.Lt: return Apache.Cassandra.IndexOperator.LT;
                default: throw new InvalidOperationException();
            }
        }

        internal static IndexOperator Convert(Apache.Cassandra.IndexOperator op) {
            switch (op) {
                case Apache.Cassandra.IndexOperator.EQ: return IndexOperator.Eq;
                case Apache.Cassandra.IndexOperator.GTE: return IndexOperator.Gte;
                case Apache.Cassandra.IndexOperator.GT: return IndexOperator.Gt;
                case Apache.Cassandra.IndexOperator.LTE: return IndexOperator.Lte;
                case Apache.Cassandra.IndexOperator.LT: return IndexOperator.Lt;
                default: throw new InvalidOperationException();
            }
        }
    
        internal static Apache.Cassandra.ConsistencyLevel Convert(ConsistencyLevel consistencylevel)
        {
            switch (consistencylevel)
    	    {
		case ConsistencyLevel.One: return Apache.Cassandra.ConsistencyLevel.ONE;
                case ConsistencyLevel.Quorum: return Apache.Cassandra.ConsistencyLevel.QUORUM;
                case ConsistencyLevel.LocalQuorum: return Apache.Cassandra.ConsistencyLevel.LOCAL_QUORUM;
                case ConsistencyLevel.EachQuorum: return Apache.Cassandra.ConsistencyLevel.EACH_QUORUM;
                case ConsistencyLevel.All: return Apache.Cassandra.ConsistencyLevel.ALL;
                case ConsistencyLevel.Any: return Apache.Cassandra.ConsistencyLevel.ANY;
                default: throw new InvalidOperationException();
    	    }
        }
    
        internal static List<byte[]> Convert(IList<Value> key) {
            return key.Select(x=>x.ToBinary()).ToList();
        }
    
        internal static IDictionary<Value,int> Convert(Dictionary<byte[],int> dictionary) {
            Dictionary<Value, int> ret = new Dictionary<Value, int>();
            foreach (var pair in dictionary) ret.Add(pair.Key, pair.Value);
            return ret;
        }

        internal static IDictionary<Value,IList<ColumnOrSuperColumn>> Convert(Dictionary<byte[],List<Apache.Cassandra.ColumnOrSuperColumn>> dictionary) {
            Dictionary<Value, IList<ColumnOrSuperColumn>> ret = new Dictionary<Value, IList<ColumnOrSuperColumn>>();
            foreach (var pair in dictionary) ret.Add(pair.Key, Convert(pair.Value));
            return ret;
        }

        private static IList<ColumnOrSuperColumn> Convert(List<Apache.Cassandra.ColumnOrSuperColumn> list) {
            return list.Select(x => new ColumnOrSuperColumn(x)).ToList();
        }

        internal static Dictionary<byte[], Dictionary<string, List<Apache.Cassandra.Mutation>>> Convert(IDictionary<Value, IDictionary<string, IList<Mutation>>> mutations) {
            Dictionary<byte[], Dictionary<string, List<Apache.Cassandra.Mutation>>> ret = new Dictionary<byte[], Dictionary<string, List<Apache.Cassandra.Mutation>>>();
            foreach (var pair in mutations) ret.Add(pair.Key, Convert(pair.Value));
            return ret;
        }

        private static Dictionary<string, List<Apache.Cassandra.Mutation>> Convert(IDictionary<string, IList<Mutation>> cfmutations) {
            Dictionary<string, List<Apache.Cassandra.Mutation>> ret = new Dictionary<string, List<Apache.Cassandra.Mutation>>();
            foreach (var mut in cfmutations) ret.Add(mut.Key, Convert(mut.Value));
            return ret;
        }

        private static List<Apache.Cassandra.Mutation> Convert(IList<Mutation> mutationlist) {
            return mutationlist.Select(x => x.ToThrift()).ToList();
        }

        internal static IDictionary<string, IList<string>> Convert(Dictionary<string, List<string>> dictionary) {
            Dictionary<string, IList<string>> ret = new Dictionary<string, IList<string>>();
            foreach (var item in dictionary) ret.Add(item.Key, item.Value);
            return ret;
        }

        internal static IList<KeySpaceDefinition> Convert(List<Apache.Cassandra.KsDef> list) {
            return list.Select(x => new KeySpaceDefinition(x)).ToList();
        }

        internal static IList<TokenRange> Convert(List<Apache.Cassandra.TokenRange> list) {
            return list.Select(x => new TokenRange(x)).ToList();
        }
    }
}
