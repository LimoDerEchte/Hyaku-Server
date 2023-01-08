using System;
using System.Net;
using System.Text;

namespace HyakuServer.Networking
{
    public class QueryServer
    {
        private static HttpListener _server;

        public static void Run()
        {
            _server = new HttpListener();
            _server.Prefixes.Add($"http://+:{Program.Config.QueryPort}/");
            _server.Start();
            Receive();
            Console.WriteLine($"Query Server started on port {Program.Config.QueryPort}");
        }

        private static void Receive()
        {
            _server.BeginGetContext(ListenerCallback, _server);
        }
        
        private static void ListenerCallback(IAsyncResult result)
        {
            if (_server.IsListening)
            {
                var context = _server.EndGetContext(result);
                _server.BeginGetContext(ListenerCallback, _server);
                var response = context.Response;
                response.StatusCode = (int) HttpStatusCode.OK;
                response.ContentType = "text/plain";
                byte[] bytes = Encoding.UTF8.GetBytes(GenerateListString());
                response.OutputStream.Write(bytes, 0, bytes.Length);
                response.OutputStream.Close();
            }
        }

        private static string GenerateListString()
        {
            //return "9|Test 1|3|Test 2|2|Test 3|9|Test 4|4|Test 5|7|Test6|9|Test 7|7|Test9|9|Test8|8";
            string o = HyakuServer.Lobbies.Count.ToString();
            foreach(Lobby l in HyakuServer.Lobbies.Values)
            {
                o += $"|{l.Name}|{l.Clients.Count}";
            }
            return o;
        }
    }
}