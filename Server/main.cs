using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Tood.Server 
{

    public class Driver {
        public static void Main(string[] args) {
            var ioloop = Manos.IO.IOLoop.Instance;
            Manos.Http.HttpServer server = new Manos.Http.HttpServer(HandleRequest, ioloop.CreateSocketStream());
            server.Listen("0.0.0.0", 5000);
            ioloop.Start();
        }

        static void HandleRequest(Manos.Http.IHttpTransaction tx) {
            using (Cassandra.Connection conn = new Cassandra.Connection()) {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("VERSION: " + conn.DescribeVersion());
                sb.AppendLine("PARTITIONER: " + conn.DescribePartitioner());

                tx.Response.StatusCode = 200;
                tx.Response.End(sb.ToString());
                tx.Response.Complete(tx.OnResponseFinished);
            }
        }
    }
}
