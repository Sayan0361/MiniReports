using MiniReportsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniReportsProject.ViewModel
{
    public class linkProgramSitesViewModel
    {
        public int GranteeID { get; set; }
        public List<ProgramModel>ProgramTypes { get; set; }
        public List<SiteModel> UnlinkedSites { get; set; }
    }
}