using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrap.UI.Models.Wordpress
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class Tag
    {
        public int id { get; set; }
        public int count { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string taxonomy { get; set; }
        public List<object> meta { get; set; }
        public Links _links { get; set; }
    }
    public class WpPostType
    {
        public string href { get; set; }
    }




}