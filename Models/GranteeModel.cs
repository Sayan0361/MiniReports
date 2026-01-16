using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniReportsProject.Models
{
    public class GranteeModel
    {
        public int GranteeID { get; set; }
        public string GranteeName { get; set; }
        public string GranteeType { get; set; }
        public string Address { get; set; } 
    }
}