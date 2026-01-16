using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniReportsProject.Models
{
    public class SiteModel
    {
        public int SiteID { get; set; }
        public  string SiteName { get; set; }
        public string SiteType { get; set; }
        public string Address { get; set; }
        public int GranteeID { get; set; }
    }
}