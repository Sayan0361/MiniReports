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
        public int SiteTypeID { get; set; }
        public string Address { get; set; }
        public int GrantID { get; set; }
    }
}