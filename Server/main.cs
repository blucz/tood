using System;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Tood.Server 
{

    public class Driver {
        static Cassandra.Connection _conn;
        static string __staticdir;

        public static void Main(string[] args) {
            if (args.Length == 0) {
                _Usage();
                return;
            }
            string staticdir = null;
            int port = 5000;
            foreach (var arg in args) {
                if (arg.StartsWith("-static="))
                    staticdir = arg.Substring("-static=".Length).Trim();
                if (arg.StartsWith("-port=")) {
                    if (!int.TryParse(arg.Substring("-port=".Length), out port)) {
                        Console.Error.WriteLine("Invalid Port");
                        _Usage();
                    }
                }
            }

            if (staticdir == null)
                _Usage();

            if (!Directory.Exists(staticdir)) {
                Console.Error.WriteLine("Directory doesn't exist: " + staticdir);
                _Usage();
            }

            __staticdir = staticdir;

            var ioloop = Manos.IO.IOLoop.Instance;
            Manos.Http.HttpServer server = new Manos.Http.HttpServer(HandleRequest, ioloop.CreateSocketStream());
            server.Listen("0.0.0.0", port);
            Console.WriteLine("[driver] Listening on port " + port);
            Console.WriteLine("[driver] Using Static files directory " + new DirectoryInfo(staticdir).FullName);
            Console.WriteLine("[driver] Initializing Cassandra");
            _InitCassandra();
            Console.WriteLine("[driver] Ready.");
            ioloop.Start();
        }

        private static void _Usage() {
            Console.Error.WriteLine("Usage: Server.exe -static=<path to static files> [-port=<port>]");
            Environment.Exit(1);
        }

        private static void _InitCassandra() {
            _conn = new Cassandra.Connection();
            Console.WriteLine("[cassandra] connected to cassandra version " + _conn.DescribeVersion());

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
        }

        private static void _InitializeKeySpace(Cassandra.Connection conn, Cassandra.KeySpaceDefinition current) {
            Cassandra.KeySpaceDefinition existing;
            try {
                existing = conn.DescribeKeySpace(current.Name);
            } catch (Cassandra.NotFoundException) {
                _conn.SystemAddKeyspace(current);
                _conn.SetKeyspace(current.Name);
                Console.WriteLine("[cassandra] created keyspace '" + current.Name + "'");
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
                Console.Error.WriteLine("[cassandra] dropped column family '" + cf + "' from keyspace '" + current.Name + "'");
            }

            foreach (var cf in cf_toadd.Values) {
                _conn.SystemAddColumnFamily(cf);
                Console.Error.WriteLine("[cassandra] added column family '" + cf.Name + "' to keyspace '" + current.Name + "'");
            }

            foreach (var pair in cf_toupdate) {
                pair.Value.Id = pair.Key.Id;
                _conn.SystemUpdateColumnFamily(pair.Value);
                Console.Error.WriteLine("[cassandra] updated column family '" + pair.Value.Name + "' in keyspace '" + current.Name + "'");
            }

            var ksclone = current.Clone();
            ksclone.ColumnFamilies.Clear();
            conn.SystemUpdateKeyspace(ksclone);
            Console.Error.WriteLine("[cassandra] updated keyspace '" + current.Name + "'");
        }

        static void HandleRequest(Manos.Http.IHttpTransaction tx) {
            Console.WriteLine("request path " + tx.Request.Path);
            if (tx.Request.Path == "" || tx.Request.Path == "/") {
                tx.Request.Path = "/index.html";
                HandleRequest(tx);

            } else if (tx.Request.Path.StartsWith("/a/")) {
                HandleApi(tx);

            } else if (tx.Request.Path.StartsWith("/")) {
                HandleStaticFile(tx);

            } else {
                tx.Response.StatusCode = 404;
                tx.Response.End();
                tx.Response.Complete(tx.OnResponseFinished);
            }
        }

        private static void HandleApi(Manos.Http.IHttpTransaction tx) {
            tx.Response.StatusCode = 404;
            tx.Response.End();
            tx.Response.Complete(tx.OnResponseFinished);
        }

        private static void HandleStaticFile(Manos.Http.IHttpTransaction tx) {
            string fn = tx.Request.Path;
            while (fn.StartsWith("/"))
                fn = fn.Substring(1);
            string path = __staticdir;
            foreach (var part in fn.Split('/')) {
                if (part == "..") {
                    tx.Response.StatusCode = 404;
                    tx.Response.End("Not Found");
                    tx.Response.Complete(tx.OnResponseFinished);
                    return;
                }
                path = Path.Combine(path, part);
            }
            ThreadPool.QueueUserWorkItem(_ => {
                try {
                    var bytes = File.ReadAllBytes(path);
                    Manos.Threading.Boundary.Instance.ExecuteOnTargetLoop(() => {
                        tx.Response.StatusCode = 200;
                        tx.Response.Headers.SetHeader("Content-Type", _GetContentTypeFromExtension(Path.GetExtension(path)));
                        tx.Response.Headers.ContentLength = bytes.Length;
                        tx.Response.Write(bytes);
                        tx.Response.End();
                        tx.Response.Complete(tx.OnResponseFinished);
                    });
                } catch (FileNotFoundException) {
                    Manos.Threading.Boundary.Instance.ExecuteOnTargetLoop(() => {
                        tx.Response.StatusCode = 404;
                        tx.Response.End("Not Found");
                        tx.Response.Complete(tx.OnResponseFinished);
                    });
                } catch (Exception e) {
                    Console.WriteLine("Exception serving static file: " + e);
                    Manos.Threading.Boundary.Instance.ExecuteOnTargetLoop(() => {
                        tx.Response.StatusCode = 500;
                        tx.Response.End("Internal Server Error");
                        tx.Response.Complete(tx.OnResponseFinished);
                    });
                }
            });
        }

        private static string _GetContentTypeFromExtension(string ext) {
            switch (ext.ToLowerInvariant()) {
                case ".htm": return "text/html";
                case ".html": return "text/html";
                case ".png": return "image/png";
                case ".jpg": return "image/jpeg";
                default: return "application/octet-stream";
            }
        }

        static void _HandleRoot(Manos.Http.IHttpTransaction tx) {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            tx.Response.StatusCode = 200;
            StringBuilder sb = new StringBuilder();

            _conn.Insert("brian", new Cassandra.ColumnParent("users"), new Cassandra.Column("name", "Brian Luczkiewicz", DateTime.UtcNow.Ticks));
            _conn.Insert("brian", new Cassandra.ColumnParent("users"), new Cassandra.Column("email", "brian@blucz.com", DateTime.UtcNow.Ticks));

            int count;
            if (!_conn.TryGet("brian", new Cassandra.ColumnPath("users", "visitcount"), out count))
                count = 0;

            count += 1;
            _conn.Insert("brian", new Cassandra.ColumnParent("users"), new Cassandra.Column("visitcount", count, DateTime.UtcNow.Ticks));

            var name = _conn.Get("brian", new Cassandra.ColumnPath("users", "name"));
            var email = _conn.Get("brian", new Cassandra.ColumnPath("users", "email"));

            sb.AppendLine("Name: " + name.Column.Value.ToString());
            sb.AppendLine("Email: " + email.Column.Value.ToString());
            sb.AppendLine("Visit Count: " + count);

            tx.Response.End(sb.ToString());
            tx.Response.Complete(tx.OnResponseFinished);
            sw.Stop();
            Console.WriteLine("services request in " + sw.ElapsedMilliseconds + "ms");
        }
    }
}
