using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Tood.Server 
{

    public class Driver {
        static Cassandra.Connection _conn;

        public static void Main(string[] args) {
            var ioloop = Manos.IO.IOLoop.Instance;
            Manos.Http.HttpServer server = new Manos.Http.HttpServer(HandleRequest, ioloop.CreateSocketStream());
            server.Listen("0.0.0.0", 5000);
            _InitCassandra();
            ioloop.Start();
        }

        private static void _InitCassandra() {
            _conn = new Cassandra.Connection();
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("CASSANDRA INIT: connected to cassandra " + _conn.DescribeVersion());

            Cassandra.KeySpaceDefinition ksdef = new Cassandra.KeySpaceDefinition() {
                Name = "tood",
                ColumnFamilies = {
                    new Cassandra.ColumnFamilyDefinition() {
                        Keyspace = "tood",
                        Name = "users",
                    }
                }, 
                ReplicationFactor = 1,
                ReplicationStrategy = Cassandra.ReplicationStrategy.SimpleStrategy,
            };

            _InitializeKeySpace(_conn, ksdef);
            Console.WriteLine("------------------------------------------------------------------------");
        }

        private static void _InitializeKeySpace(Cassandra.Connection conn, Cassandra.KeySpaceDefinition current) {
            Cassandra.KeySpaceDefinition existing;
            try {
                existing = conn.DescribeKeySpace(current.Name);
            } catch (Cassandra.NotFoundException) {
                _conn.SystemAddKeyspace(current);
                _conn.SetKeyspace(current.Name);
                Console.WriteLine("CASSANDRA INIT: created keyspace '" + current.Name + "'");
                return;
            }

            _conn.SetKeyspace(current.Name);

            Dictionary<string, Cassandra.ColumnFamilyDefinition> cf_toremove = new Dictionary<string, Cassandra.ColumnFamilyDefinition>();
            Dictionary<string, Cassandra.ColumnFamilyDefinition> cf_toadd = new Dictionary<string, Cassandra.ColumnFamilyDefinition>();
            List<KeyValuePair<Cassandra.ColumnFamilyDefinition, Cassandra.ColumnFamilyDefinition>> cf_toupdate = new List<KeyValuePair<Cassandra.ColumnFamilyDefinition, Cassandra.ColumnFamilyDefinition>>();

            foreach (var cf in existing.ColumnFamilies)
                cf_toremove.Add(cf.Name, cf);

            foreach (var cf in current.ColumnFamilies) {
                Cassandra.ColumnFamilyDefinition oldcf;
                if (cf_toremove.TryGetValue(cf.Name, out oldcf)) {
                    cf_toremove.Remove(cf.Name);
                    cf_toupdate.Add(new KeyValuePair<Cassandra.ColumnFamilyDefinition, Cassandra.ColumnFamilyDefinition>(oldcf, cf));
                } else {
                    cf_toadd.Add(cf.Name, cf);
                }
            }

            foreach (var cf in cf_toremove.Keys) {
                _conn.SystemDropColumnFamily(cf);
                Console.Error.WriteLine("CASSANDRA INIT: dropped column family '" + cf + "' from keyspace '" + current.Name + "'");
            }

            foreach (var cf in cf_toadd.Values) {
                _conn.SystemAddColumnFamily(cf);
                Console.Error.WriteLine("CASSANDRA INIT: added column family '" + cf.Name + "' to keyspace '" + current.Name + "'");
            }

            foreach (var pair in cf_toupdate) {
                pair.Value.Id = pair.Key.Id;
                _conn.SystemUpdateColumnFamily(pair.Value);
                Console.Error.WriteLine("CASSANDRA INIT: updated column family '" + pair.Value.Name + "' in keyspace '" + current.Name + "'");
            }

            var ksclone = current.Clone();
            ksclone.ColumnFamilies.Clear();
            conn.SystemUpdateKeyspace(ksclone);
            Console.Error.WriteLine("CASSANDRA INIT: updated keyspace '" + current.Name + "'");
        }

        static void HandleRequest(Manos.Http.IHttpTransaction tx) {
            if (tx.Request.Path == "" || tx.Request.Path == "/")
                _HandleRoot(tx);
            else {
                tx.Response.StatusCode = 404;
                tx.Response.End();
                tx.Response.Complete(tx.OnResponseFinished);
            }
        }

        static void _HandleRoot(Manos.Http.IHttpTransaction tx) {
            tx.Response.StatusCode = 200;
            StringBuilder sb = new StringBuilder();

            _conn.Insert("brian", new Cassandra.ColumnParent("users"), new Cassandra.Column("name", "Brian Luczkiewicz", DateTime.UtcNow.Ticks));
            _conn.Insert("brian", new Cassandra.ColumnParent("users"), new Cassandra.Column("email", "brian@blucz.com", DateTime.UtcNow.Ticks));

            int count;
            try {
                count = _conn.Get("brian", new Cassandra.ColumnPath("users", "visitcount")).Column.Value;
            } catch (Cassandra.NotFoundException) {
                count = 0;
            }

            count += 1;
            _conn.Insert("brian", new Cassandra.ColumnParent("users"), new Cassandra.Column("visitcount", count, DateTime.UtcNow.Ticks));

            var name = _conn.Get("brian", new Cassandra.ColumnPath("users", "name"));
            var email = _conn.Get("brian", new Cassandra.ColumnPath("users", "email"));

            sb.AppendLine("Name: " + name.Column.Value.ToString());
            sb.AppendLine("Email: " + email.Column.Value.ToString());
            sb.AppendLine("Visit Count: " + count);

            tx.Response.End(sb.ToString());
            tx.Response.Complete(tx.OnResponseFinished);
        }
    }
}
