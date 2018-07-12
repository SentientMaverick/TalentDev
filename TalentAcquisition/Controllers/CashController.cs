using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Helper;

namespace TalentAcquisition.Controllers
{
    public class CashController : Controller
    {
        #region Initializers

        TalentContext db = new TalentContext();
        private readonly IWorkflowHelper _workFlowHelper = new WorkFlowHelper();

        #endregion
        #region Views
        [Route("Expenses/Transaction/Create")]
        // GET: Cash
        public ActionResult CashRequest()
        {
            ViewBag.Branches = db.Branches.ToList();
            ViewBag.Items = new List<CashLineItem> ();
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName });
            ViewBag.CashRequisitionTypeCode = new SelectList(db.CashRequisitionTypes, "Code", "Description");
            var model = new CashRequisition();
            model.DateCreated = DateTime.UtcNow;
            model.DateModified = null;
            model.No = "CR" + (db.CashRequisitions.Count() + 1);
            return View(model);
        }
        [HttpPost]
        [Route("Expenses/Transaction/Create")]
        // GET: Cash
        public async Task<ActionResult> CashRequest(CashRequisition model,List<CashLineItem> Line)
        {
            
            ViewBag.Branches = db.Branches.ToList();
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName });
            ViewBag.CashRequisitionTypeCode = new SelectList(db.CashRequisitionTypes, "Code", "Description");
            try
            {
                if (ModelState.IsValid)
                {
                    var Userid = User.Identity.GetUserId();
                    var currentUser = db.Employees.Where(x => x.UserId == Userid).First();
                    var userid = currentUser.ID;
                    bool flow =await _workFlowHelper.IsEmployeeAuthorizedForAction("cash",userid);
                    if (!flow)
                    {
                        ViewBag.Error = "Not Authorized To Make Cash Requisition";
                        return View(model);
                    }
                    if (Line == null)
                    {
                        ViewBag.Error = "Line Items Empty! Add Appropriate Line Items in Requisition";
                        return View(model);
                        //db.CashRequisitions.Add(model);
                        //db.SaveChanges();
                        //return RedirectToAction("Transaction/Create", "Expenses");
                    }
                    else
                    {
                        bool itemscheck = CheckLineItems(Line, model);
                        if (itemscheck)
                        {
                            db.CashRequisitions.Add(model);
                            bool itemsstatus = UpdateLineItems(Line, model);
                            var  entries = await _workFlowHelper.GenerateApprovalEntries(model, "cash", model.No, userid);

                            db.SaveChanges();
                        }
                        return RedirectToAction("Transaction/Create", "Expenses");
                    }

                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private bool UpdateLineItems(List<CashLineItem> items, CashRequisition model)
        {
            foreach (var item in items)
            {
                item.CashRequisitionNo = model.No;
                item.Amount = (decimal)item.Amount;
                db.CashLineItems.Add(item);
            }
            //db.CashLineItems.AddRange(items);
            db.SaveChanges();
            return true;
        }

        private bool CheckLineItems(List<CashLineItem> items, CashRequisition model)
        {
            
            return true;
        }
        [Route("Expenses/Transactions/Approve/{id}")]
        // GET: Cash
        public ActionResult ApproveCashRequest(string id)
        {
            ViewBag.Branches = db.Branches.ToList();
            ViewBag.Items = db.CashLineItems.Where(x=>x.CashRequisitionNo==id).Select(x=>new {x.Id,x.Name,x.Description,x.Amount,x.CashRequisitionNo }).ToList();
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName });
            ViewBag.CashRequisitionTypeCode = new SelectList(db.CashRequisitionTypes, "Code", "Description");
            var model = db.CashRequisitions.Where(x => x.No == id).First();
            model.TotalAmount = db.CashLineItems.Where(x => x.CashRequisitionNo == id).Select(x => x.Amount).Sum().ToString();
            return View(model);
        }
        [HttpPost]
        [Route("Expenses/Transactions/Approve/{id}")]
        // GET: Cash
        public async Task<ActionResult> ApproveCashRequest(string id,CashRequisition model)
        {
            ViewBag.CashRequisitionTypeCode = new SelectList(db.CashRequisitionTypes, "Code", "Description");
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName });
            ViewBag.Branches = db.Branches.ToList();

            if (id == null)
            {
                return HttpNotFound();
            }
            var cash = db.CashRequisitions.Where(x => x.No == id);
            if (cash == null)
            {
                return HttpNotFound();
            }
            bool y = await _workFlowHelper.IsApprovalEntryExisting("cash", cash.First().No);
            if (!y)
            {
                ViewBag.Error = "Document has completed Approval Steps";
                return View(cash.First());
            }
            var Userid = User.Identity.GetUserId();
            var currentUser = db.Employees.Where(x => x.UserId == Userid).First();
            var userid = currentUser.ID;
            List<ApprovalEntry> entries=new List<ApprovalEntry>();
            if (y)
            {
                var processno = cash.First().No;
                entries = db.ApprovalEntries.Where(r => r.ProcessNo == processno).ToList() ;
            }
            //else
            //{
            //    entries = await _workFlowHelper.GenerateApprovalEntries<CashRequisition>(cash.First(),"cash",cash.First().No,userid);
            //}
            var currententry = await _workFlowHelper.GetNextApproval(entries, cash.First().No);
            if (currententry == null)
            {
                ViewBag.Error = "Document has completed Approval Steps";
                return View(cash.First());
            }
            if (currententry.Approver ==currentUser.EmployeeNumber)
            {
                currententry = await _workFlowHelper.UpdateApprovalEntry(currententry);
                cash.First().Status = "Approved by " + db.Employees.Where(x => x.EmployeeNumber == currententry.Approver).First().OfficePosition.Title;
                db.SaveChanges();
            }
            else
            {
                ViewBag.Error = "Not Authorized to Approve Document";
                return View(cash.First());
            }
            return RedirectToAction("Transactions/Approve/"+id,"Expenses");
        }
        [Route("Expenses/Transactions/Open")]
        // GET: Performance/Create
        public ActionResult OpenTransactions()
        {
            return View(db.CashRequisitions);
        }
        [Route("Expenses/Transactions/Approved")]
        // GET: Performance/Create
        public ActionResult ApprovedTransactions()
        {
            return View("OpenTransactions", db.CashRequisitions);
        }
        [Route("Expenses/Transaction/Types")]
        // GET: Performance/Create
        public ActionResult TransactionTypes()
        {
            return View();
        } 
        #endregion

        #region JsonActions
        [HttpGet]
        [Route("api/Transaction/Types")]
        public JsonResult GetTrasactionTypes()
        {
            List<CashRequisitionType> types = db.CashRequisitionTypes.ToList();
            return Json(new { status = "True", types = types }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/Transaction/Types")]
        public JsonResult GetTrasactionTypes(List<CashRequisitionType> types)
        {
            foreach (var type in types)
            {
                if (db.CashRequisitionTypes.Where(x => x.Code == type.Code).Any())
                {
                    db.CashRequisitionTypes.Add(type);
                    db.Entry(type).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.CashRequisitionTypes.Add(type);
                }
            }
            db.SaveChanges();
            return Json(new { status = "True", types = types }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
