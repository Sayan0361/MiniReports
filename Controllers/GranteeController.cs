using MiniReportsProject.DAL;
using MiniReportsProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniReportsProject.Controllers
{
    public class GranteeController : Controller
    {
        GranteeDAL _granteeDAL = new GranteeDAL();
        ProgramDAL _programDAL = new ProgramDAL();
        // GET: Grantee
        public ActionResult Index(int ID)
        {
            var SiteList = _granteeDAL.GetAllSitesByGranteeID(ID);
            string GranteeName = _granteeDAL.GetGranteeNameByID(ID);

            var ProgramData = _programDAL.GetProgramData(ID);

            int GranteeID = ID;

            var siteViewModel = new SiteViewModel();
            siteViewModel.siteList = SiteList;
            siteViewModel.GranteeName = GranteeName;
            siteViewModel.GranteeID = GranteeID;

            siteViewModel.ProgramList = ProgramData;

            // Build dynamic modal model instead of using ViewBag
            siteViewModel.Modal = new ModalViewModel
            {
                Title = "Login",
                Message = "Login Success.",
                Status = true, // set to true to show the modal
                UserName = GranteeName
            };

            return View(siteViewModel);
        }
    }
}