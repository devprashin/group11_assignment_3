using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment3Server {
    public class Request {
        /*Method = "xxxx",
        Path = "testing",
        Date = UnixTimestamp(),
        Body = "{}"*/
        public string Method { get; set; }
        public string Path { get; set; }
        public double UnixTimestamp { get; set; }
        public string Body { get; set; }
    }

}
