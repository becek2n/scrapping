using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrap.UI.Models.DTO
{
    public class WordpressTagRequestDTO
    {
        public string description { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public List<object> meta { get; set; }
    }

    
}