using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MiniReportsProject.ViewModel
{
    public class SchoolDetailsViewModel
    {
        public int SiteID { get; set; }
        public string SchoolName { get; set; }
        public string Address { get; set; }
        public string SiteName { get; set; }
        public string GradeName { get; set; }

        [DisplayName("Grades Served")]
        public List<string> GradeNameList { get; set; }
    }
}