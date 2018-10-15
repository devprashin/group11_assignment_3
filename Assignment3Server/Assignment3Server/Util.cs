using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Assignment3Server {
    public static class Util {


        public static string ToJson(this object data) {
            return JsonConvert.SerializeObject(data,
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        public static T FromJson<T>(this string element) {
            return JsonConvert.DeserializeObject<T>(element);
        }

        public static byte[] ObjectToByteArray(object obj) {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream()) {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
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

        public static Request Read(this TcpClient client) {
            var strm = client.GetStream();
            var buffer = new byte[1024];
            var sb = new StringBuilder();

            do {
                var readCnt = strm.Read(buffer, 0, 1024);

                sb.Append(Encoding.UTF8.GetString(buffer, 0, readCnt));



            } while (strm.DataAvailable);
            return JsonConvert.DeserializeObject<Request>(sb.ToString());

        }
    }
}
