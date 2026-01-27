using MiniReportsProject.DAL;
using MiniReportsProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniReportsProject.Controllers
{
    public class ProgramController : Controller
    {
        ProgramDAL _programDAL = new ProgramDAL();
        // GET: Program/{program ID}
        public ActionResult Index(int id, int GranteeID)
        {
            var SitesLinkedToCurrentProgram = _programDAL.GetAllSitesDetailsByProgramID(id, GranteeID);
            var SchoolsLinkedToCurrentProgram = _programDAL.GetAllSchoolDetailsByProgramID(id, GranteeID);

            var model = new ProgramDetailsViewModel
            {
                Sites = SitesLinkedToCurrentProgram,
                Schools = SchoolsLinkedToCurrentProgram
            };

            return View(model);
        }

        public ActionResult Link(int id)
        {
            var ProgramTypes = _programDAL.GetAllProgramTypes();
            var UnlinkedSites = _programDAL.GetAllUnlinkedSitesWithProgram(id);

            var model = new linkProgramSitesViewModel
            {
                GranteeID = id,
                ProgramTypes = ProgramTypes,
                UnlinkedSites = UnlinkedSites
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult LinkProgramToSite(int SiteID, int ProgramTypeID)
        {
            int result = _programDAL.LinkProgramToSite(SiteID, ProgramTypeID);
            return RedirectToAction("Index", "Site", new {id = SiteID});
        }
    }
}