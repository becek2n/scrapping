using Newtonsoft.Json;
using Scrap.UI.Models.Wordpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrap.UI.Models.DTO
{
    public class WordpressMediaRequestDTO
    {
        public DateTime date { get; set; }
        public DateTime date_gmt { get; set; }
        public string slug { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public int author { get; set; }
        public string comment_status { get; set; }
        public string type { get; set; }
        public string ping_status { get; set; }
        public Meta meta { get; set; }
        public string template { get; set; }
        public string alt_text { get; set; }
        public string caption { get; set; }
        public string description { get; set; }
        public int post { get; set; }
       
    }

}