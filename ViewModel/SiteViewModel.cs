using MiniReportsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniReportsProject.ViewModel
{
    public class SiteViewModel
    {
        public List<SiteModel> siteList { get; set; }
        public string GranteeName { get; set; }
        public int GranteeID { get; set; }
    }
}