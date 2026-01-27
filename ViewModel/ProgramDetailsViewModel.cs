using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniReportsProject.ViewModel
{
    public class ProgramDetailsViewModel
    {
        public int GranteeID { get; set; }
        public List<string> Schools { get; set; }
        public List<string> Sites { get; set; }
    }
}