using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Net;

namespace TestTaskNETCore.Models
{
    public class Link
    {
        public int LinkId { get; set; }
        public string LongURL { get; set; }
        public string ShortURL { get; set; }
        public DateTime DateTime { get; set; }
        public int RedirectCount { get; set; }
    }
}
