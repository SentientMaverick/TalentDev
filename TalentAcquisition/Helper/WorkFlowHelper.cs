using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Helper
{
    public class WorkFlowHelper : IWorkflowHelper
    {
        TalentContext db = new TalentContext();

        public WorkFlowHelper()
        {
            this.ApprovalEntries = new List<ApprovalEntry>();
        }
        public List<ApprovalEntry> ApprovalEntries { get;set;}

        public int RequiredApprovals
        {
            get
            {
                if(ApprovalEntries!=null && ApprovalEntries.Count() > 0)
                {
                    return ApprovalEntries.Where(x=>x.Status == "Pending")
                                          .OrderBy(x => x.Sequence)
                                          .Count();
                }
                else
                {
                   // throw new NullReferenceException();
                    return 0;
                }
            }
        }

        public int TotalApprovals
        {
            get
            {
                return ApprovalEntries.Count();
            }
        }

        public async Task<bool> IsApprovalEntryExisting(string type, string processno)
        {
            bool x =  db.ApprovalEntries.Where(r => r.ProcessNo == processno).Any();
            await Task.Delay(5);
            return x;
        }
       async Task<List<ApprovalEntry>> IWorkflowHelper.GenerateApprovalEntries<T>(T value, string key, string processno,int userId)
        {
            List<ApprovalEntry> entries = new List<ApprovalEntry>();
            try
            {
                DocumentWorkFlow flow = db.ApprovalFlows.Where(x=>x.ProcessName == key).FirstOrDefault();
                if (key == "cash")
                {
                    //CashRequisition model = db.CashRequisitions.Where(x=>x.No==key).First();
                    var count = db.ApprovalEntries.Count();
                    Dictionary<int, string> approvers = new Dictionary<int, string>();
                    approvers.Add(0, flow.Approver1Id);
                    approvers.Add(1, flow.Approver2Id);
                    approvers.Add(2, flow.Approver3Id);
                    approvers.Add(3, flow.Approver4Id);
                    approvers.Add(4, flow.Approver5Id);
                    for (int i = 0; i < flow.NoOfApprovals; i++)
                    {
                        entries.Add(new ApprovalEntry { No = count + i + 1, Sender = userId.ToString(), ProcessType = key, Sequence = i, ProcessNo = processno, Approver = approvers[i], Status = "Pending" });
                    }
                }
                db.ApprovalEntries.AddRange(entries);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error Generating Approval Entries", ex);
            }
           
            await Task.Delay(5);
            return entries;
        }
        async Task<ApprovalEntry> IWorkflowHelper.GetNextApproval(List<ApprovalEntry> entries, string processno)
        {
           // var entry = new ApprovalEntry();
            ApprovalEntry entry=null;
            var record= entries.Where(x => x.ProcessNo == processno && x.Status=="Pending").OrderBy(x=>x.Sequence);
            if (record.Any())
            {
                entry = record.First();
            }
            await Task.Delay(5);
            return entry;
        }
       async Task<bool> IWorkflowHelper.IsEmployeeAuthorizedForAction(string type, int userId)
        {
            var flow = db.ApprovalFlows.Where(x => x.Sender==userId && x.ProcessName==type);
            bool action = false;
            if (flow.Any())
            {
                action = true;
            } 
            await Task.Delay(5);
            return action;
        }

        async Task<ApprovalEntry> IWorkflowHelper.UpdateApprovalEntry(ApprovalEntry entry)
        {
            entry.Status = "Approved";
            db.ApprovalEntries.Add(entry);
            db.Entry(entry).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            await Task.Delay(5);
            return entry;
        }
    }
}