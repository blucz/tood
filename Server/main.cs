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
            tx.Response.StatusCode = 200;
            tx.Response.End("Hello, World");
            tx.Response.Complete(tx.OnResponseFinished);
        }
    }
}
