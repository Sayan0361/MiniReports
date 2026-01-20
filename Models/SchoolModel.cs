using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MiniReportsProject.Models
{
    public class SchoolModel
    {
        public int SchoolID { get; set; }
        [DisplayName("School Name")]
        public string SchoolName { get; set; }
        public int SiteID { get; set; }
        public string Address { get; set; }

    }
}