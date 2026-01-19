using MiniReportsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniReportsProject.ViewModel
{
    public class SiteCreateViewModel
    {
        public string SiteName { get; set; }

        public string SelectedTypeName { get; set; }

        public IEnumerable<EntityTypeModel> EntityTypes { get; set; }

        public string Address { get; set; }

        public int GrantID { get; set; }
    }
}