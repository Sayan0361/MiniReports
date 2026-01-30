using Dapper;
using Microsoft.Ajax.Utilities;
using MiniReportsProject.DAL;
using MiniReportsProject.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System;

namespace MiniReportsProject.Controllers
{
    public class StateController : Controller
    {
        GranteeDAL _granteeDAL = new GranteeDAL();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllGrantees()
        {
            try
            {
                List<GranteeModel> grantees = _granteeDAL.GetAllGrantees();
                
                if(grantees == null || !grantees.Any())
                {
                    return Json(new 
                    {
                        success = false,
                        message = "No grantees found."
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new 
                {
                    success = true,
                    data = grantees
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while retrieving grantees: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create()
        {
            var createViewModel = new GranteeCreateViewModel
            {
                EntityTypes = _granteeDAL.GetAllGranteeTypes("Grantee")
            };
           
            return View(createViewModel);
        }

        // AJAX endpoint for creating grantee
        [HttpPost]
        public JsonResult CreateGrantee(GranteeCreateViewModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.GranteeName))
                {
                    return Json(new 
                    {
                        success = false,
                        message = "Grantee name is required."
                    });
                }

                if (string.IsNullOrWhiteSpace(model.SelectedTypeName))
                {
                    return Json(new 
                    {
                        success = false,
                        message = "Grantee type is required."
                    });
                }

                // Lookup EntityTypeID by Level and TypeName
                int typeId = _granteeDAL.GetTypeIDByTypeName("Grantee", model.SelectedTypeName);
                if (typeId == 0)
                {
                    return Json(new 
                    {
                        success = false,
                        message = "Selected grantee type not found."
                    });
                }

                // Create GranteeModel
                var grantee = new GranteeModel
                {
                    GranteeName = model.GranteeName.Trim(),
                    GranteeTypeID = typeId,
                    Address = model.Address?.Trim()
                };

                _granteeDAL.AddGrantee(grantee);

                return Json(new 
                {
                    success = true,
                    message = "Grantee created successfully!"
                });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error creating grantee: " + ex.Message
                });
            }
        }

        // Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GranteeCreateViewModel createViewModel)
        {
            if (!ModelState.IsValid)
            {
                createViewModel.EntityTypes = _granteeDAL.GetAllGranteeTypes("Grantee");
                return View(createViewModel);
            }

            int typeId = _granteeDAL.GetTypeIDByTypeName("Grantee", createViewModel.SelectedTypeName);
            if (typeId == 0)
            {
                ModelState.AddModelError(nameof(createViewModel.SelectedTypeName), "Selected type not found.");
                createViewModel.EntityTypes = _granteeDAL.GetAllGranteeTypes("Grantee");
                return View(createViewModel);
            }

            var grantee = new GranteeModel
            {
                GranteeName = createViewModel.GranteeName,
                GranteeTypeID = typeId,
                Address = createViewModel.Address
            };

            _granteeDAL.AddGrantee(grantee);
            return RedirectToAction("Index");
        }

        public ActionResult GetGranteeTypesByLevel(string level)
        {
            List<EntityTypeModel> granteeTypes = _granteeDAL.GetAllGranteeTypes(level);
            return View(granteeTypes);
        }
    }
}
