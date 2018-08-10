using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentAcquisition.BusinessLogic.UpdatedDomain;

namespace Talent.HRM.Services.Approval
{
    public interface IApprovalService
    {
        List<ApprovalEntry> ApprovalEntries { get; set; }
        Task<bool> IsApprovalEntryExisting(string type, string processno);
        Task<bool> IsEmployeeAuthorizedForAction(string type, int userId);
        Task<List<ApprovalEntry>> GenerateApprovalEntries<T>(T value, string key, string processno, int userId);
        Task<ApprovalEntry> GetNextApproval(List<ApprovalEntry> entries, string processno);
        Task<ApprovalEntry> UpdateApprovalEntry(ApprovalEntry entry);
        int TotalApprovals { get; }
        int RequiredApprovals { get; }
    }
}
