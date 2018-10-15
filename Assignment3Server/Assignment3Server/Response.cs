using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment3Server {
    [Serializable]
    public class Response {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Body { get; set; }
    }
}
