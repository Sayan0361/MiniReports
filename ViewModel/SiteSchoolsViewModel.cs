using MiniReportsProject.Models;
using System.Collections.Generic;

namespace MiniReportsProject.ViewModel
{
    public class SiteSchoolsViewModel
    {
        public int SiteID { get; set; }
        public int GranteeID { get; set; }
        public string GranteeName { get; set; }
        public IEnumerable<SchoolModel> Schools { get; set; }
        public ModalViewModel Modal { get; set; }
    }
}