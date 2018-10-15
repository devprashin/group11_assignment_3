using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace Assignment3 {

    public class Program {
        public static List<Category> categories = new List<Category>();
        private TcpListener server;
        private TcpClient client;

        public void Start() {
            categories.Add(new Category() { Id = 1, Name = "Beverages" });
            categories.Add(new Category() { Id = 2, Name = "Condiments" });
            categories.Add(new Category() { Id = 3, Name = "Confections" });

            server = new TcpListener(IPAddress.Loopback, 5000);
            server.Start();
            Console.WriteLine("Started.....");

            while (true) {
                Console.WriteLine("no client yet");
                client = server.AcceptTcpClient();
                Console.WriteLine("client");
                Thread t = new Thread(Process);
                t.Start(client);
            }
        }


        void Process(Object obj) {
            var client = obj as TcpClient;
            var strm = client.GetStream();

            if (!(obj is TcpClient)) return;

            var buffer = new byte[client.ReceiveBufferSize];

            var readCnt = strm.Read(buffer, 0, buffer.Length);

            var payload = Encoding.UTF8.GetString(buffer, 0, readCnt);

            var req = JsonConvert.DeserializeObject<Request>(payload);



            if (req.Method == null) {
                string resp = "missing method";
                buffer = new byte[resp.Length];
                strm.Write(buffer, 0, buffer.Length);
            }




        }

    }

    public class Response {
        public string Status { get; set; }
        public string Body { get; set; }
    }

    public class Category {
        [JsonProperty("cid")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    class Request {
        /*Method = "xxxx",
        Path = "testing",
        Date = UnixTimestamp(),
        Body = "{}"*/
        public string Method { get; set; }
        public string Path { get; set; }
        public double UnixTimestamp { get; set; }
        public string Body { get; set; }
    }

    // helper classes
    public static class Util {
        public static string ToJson(this object data) {
            return JsonConvert.SerializeObject(data,
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        public static T FromJson<T>(this string element) {
            return JsonConvert.DeserializeObject<T>(element);
        }

        public static void SendRequest(this TcpClient client, string request) {
            var msg = Encoding.UTF8.GetBytes(request);
            client.GetStream().Write(msg, 0, msg.Length);
        }

        public static Response ReadResponse(this TcpClient client) {
            var strm = client.GetStream();
            //strm.ReadTimeout = 250;
            byte[] resp = new byte[2048];
            using (var memStream = new MemoryStream()) {
                int bytesread = 0;
                do {
                    bytesread = strm.Read(resp, 0, resp.Length);
                    memStream.Write(resp, 0, bytesread);

                } while (bytesread == 2048);

                var responseData = Encoding.UTF8.GetString(memStream.ToArray());
                return JsonConvert.DeserializeObject<Response>(responseData);
            }
        }
    }
}