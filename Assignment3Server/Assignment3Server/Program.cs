using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Assignment3Server {
    class Program {
        public static List<Category> categories;
        private static TcpListener server;
        private static TcpClient client;

        static void Main(string[] args) {
            //var server = new TcpListener(IPAddress.Parse("127.0.1"), 15000);
            server = new TcpListener(IPAddress.Loopback, 15000);
            server.Start();
            Start();
            Console.WriteLine($"{categories[0].Name}");
            Console.WriteLine("Server started");

            //string method= "";
            //switch (method) {
            //    case "delete": 
                        
            //    break;

            //    default:
            //    break;
            //}




            Response response = new Response();
            while (true) {
                client = server.AcceptTcpClient();
                var stream = client.GetStream();
                var buffer = new byte[client.ReceiveBufferSize];
                var readCount = stream.Read(buffer, 0, buffer.Length);
                var payload = Encoding.UTF8.GetString(buffer, 0, readCount);
                var req = JsonConvert.DeserializeObject<Request>(payload);
                Console.WriteLine($"Message: {payload}");

                 
                if (req.Method.ToLower() == "create") {
                    var category = req.Body.FromJson<Category>();
                    category.Id = categories.Max(x => x.Id) + 1;
                    //category.Id = 10;
                    categories.Add(category);
                    Console.WriteLine("New category added: "+ category.Id);
                    response.Id = category.Id;
                    response.Status = "202 created";
                    response.Body = req.Body;

                    //response.Status = "2 created";
                    //response.Body = req.Body;
                    //var res = JsonConvert.SerializeObject(response);
                    //var resPayload = Util.ObjectToByteArray(response);
                    
                    client.SendRequest(response.ToJson());
                    Console.WriteLine($"Message: {response.ToString()}");

                    //stream.Write(resPayload, 0, resPayload.Length);
                }else if( req.Method.ToLower()== "echo") {
                    Console.WriteLine("Writing Echo");
                    response.Body = req.Body;
                    response.Status = "OK";
                    client.SendRequest(response.ToJson());
                }else if(req.Method.ToLower()== "delete") {
                    //if(req.Path.ToLower() == "")
                    string[] delimiter = req.Path.Split("/");
                    int id;
                    int.TryParse(delimiter[delimiter.Length - 1], out id);
                    Console.WriteLine(id);


                    foreach (var item in categories) {
                        if (item.Id == id) {
                            categories.Remove(item);
                            break;
                        }
                    }


                    //if (req.Body.ToLower() != null) {
                    //    foreach (var item in categories) {
                    //        if(item.Name == req.Body) {
                    //            categories.Remove(item);
                    //        }
                    //    }
                    //}

                    response.Status = "5 not found";

                    client.SendRequest(response.ToJson());
                }

                stream.Close();
                client.Close();

            }
        }
        static void Start() {
            categories = new List<Category> {
              new Category() { Id = 1, Name = "Beverages" },
              new Category() { Id = 2, Name = "Condiments" },
              new Category() { Id = 3, Name = "Confections" }, 
              new Category() { Id= 1234, Name= "Random"}
            };
        }

        static void Process(Object obj) {
            //    var client = obj as TcpClient;
            //    var stream = client.GetStream();

            //    if (!(obj is TcpClient)) return;

            //    var buffer = new byte[client.ReceiveBufferSize];

            //    var readCnt = stream.Read(buffer, 0, buffer.Length);

            //    var payload = Encoding.UTF8.GetString(buffer, 0, readCnt);

            //    var request = JsonConvert.DeserializeObject<Request>(payload);
            //    //var request = JsonConvert.DeserializeObject<Request>(requestMessage);

            //    //if (req.Method == null) {
            //    //    string resp = "missing method";
            //    //    buffer = new byte[resp.Length];
            //    //    strm.Write(buffer, 0, buffer.Length);
            //    //}
            //    //var request = client.Read();
            //    //if (request.Body.Contains("{}")) {

            //    //}
            //    Console.WriteLine($"requestmessage: {request}");

            //    //var response = new Response { Status = "4 Bad Request" };
            //    //if (request.Method.ToLower() == "echo") {
            //    //    response.Status = "1 OK";
            //    //    response.Body = request.Body;

            //    //}
            //    //else if (request.Method.ToLower() == "create") {
            //    //    var category = request.Body.FromJson<Category>();
            //    //    category.Id = categories.Max(x => x.Id) + 1;
            //    //    //category.Id = 10;
            //    //    categories.Add(category);

            //    //    response.Status = "2 created";
            //    //    response.Body = categories.ToJson();
            //    //}
            //    var response = new Response();
            //    if (request.Method.ToLower() == "create") {
            //        var category = request.Body.FromJson<Category>();
            //        category.Id = categories.Max(x=> x.Id)+1;
            //        category.Name = "Testing";
            //        //category.Id = 500;
            //        categories.Add(category);
            //        response.Status = "OK";
            //        response.Body = category.Name;
            //        //var responseClient = JsonConvert.SerializeObject(response);
            //        var responseBytes = Encoding.UTF8.GetBytes(response.ToJson());
            //        Console.WriteLine($"response: {response.Body}{response.Status}");
            //        stream.Write(responseBytes, 0, payload.Length);
            //    }else if(request.Method.ToLower() == "echo") {
            //        Console.WriteLine("Request received");
            //    }

            //    stream.Close();
            //    client.Close();

            //}
        }



        /*

        class Program
        {
            static void Main(string[] args)
            {
                var client = new TcpClient();
                client.Connect(IPAddress.Parse("127.0.0.1"), 15000);

                var msg = Encoding.UTF8.GetBytes("I am testting net client and server");

                var stream = client.GetStream();

                stream.Write(msg, 0, msg.Length);

                var buffer = new byte[client.ReceiveBufferSize];

                var readCount= stream.Read(buffer, 0, buffer.Length);

                var serverMessage= Encoding.UTF8.GetString(buffer, 0, readCount);

                Console.WriteLine($"Response: {serverMessage}");

                stream.Close();
                client.Close();
            }
        }

        */
    }
}
