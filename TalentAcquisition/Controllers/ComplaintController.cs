using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.UpdatedDomain;

namespace TalentAcquisition.Controllers
{
    public class ComplaintController : Controller
    {
        [Route("Complaint/All")]
        // GET: Complaint
        public ActionResult Index()
        {
            return View();
        }

        // GET: Complaint/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Complaint/Create
        [Route("Complaint/Grievance/Create")]
        public ActionResult Create()
        {
            return View();
        }
        [Route("Complaint/Grievance/Create")]
        // POST: Complaint/Create
        [HttpPost]
        public ActionResult Create(GrievanceReport model)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Complaint/Edit/5
        [Route("Complaint/Disciplinary/Manage")]
        public ActionResult ManageDisciplinaryCases()
        {
            return View();
        }
        [Route("Complaint/Disciplinary/Case")]
        public ActionResult CreateDisciplinaryCase()
        {
            return View();
        }
        [Route("Complaint/Grievance/Type")]
        public ActionResult GrievanceType()
        {
            return View();
        }
        [Route("Complaint/Grievance/Action")]
        public ActionResult GrievanceAction()
        {
            return View();
        }
        [Route("Complaint/Indiscipline/Type")]
        public ActionResult IndisciplineType()
        {
            return View();
        }
    }
}
