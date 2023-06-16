using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrap.UI.Models.DTO
{
    public class ExceptionResponseDTO
    {
        public string code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public List<int> additional_data { get; set; }
    }
    public class Data
    {
        public int status { get; set; }
        public int term_id { get; set; }
    }

}