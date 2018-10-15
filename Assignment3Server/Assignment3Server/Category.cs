using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment3Server {
    public class Category {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

}
