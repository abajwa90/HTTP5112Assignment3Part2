using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Miniviableproject.Models
{
    public class Teacher
    {
        public int teacherid { get; set; }
        public string teacherfname { get; set; }
        public string teacherlname { get; set; }
        public string employeenumber { get; set; }
        public decimal salary { get; set; }
        public DateTime hiredate { get; set; }
    }
}