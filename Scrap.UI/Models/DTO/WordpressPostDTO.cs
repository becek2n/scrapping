using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrap.UI.Models.DTO
{
    public class WordpressPostRequestDTO
    {
        public string date { get; set; }
        public string date_gmt { get; set; }
        public string slug { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string author { get; set; }
        public string excerpt { get; set; }
        public int? featured_media { get; set; }
        public string comment_status { get; set; }
        public string ping_status { get; set; }
        public string format { get; set; }
        public string meta { get; set; }
        public string sticky { get; set; }
        public string template { get; set; }
        public List<int> categories { get; set; }
        public List<int> tags { get; set; }
    }

    
}