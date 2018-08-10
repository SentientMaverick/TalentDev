using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.HRM.Services.Approval;
using Talent.HRM.Services.Interfaces;
using TalentAcquisition.DataLayer;

namespace Talent.HRM.Services.Notification
{
    public class NotificationService :INotificationService
    {
        private readonly TalentContext _context;
        private readonly IApprovalService _approvalService;
        public NotificationService()
        {
            _context = new TalentContext();
            _approvalService = new ApprovalService(_context);
        }

        public async Task<GeneralNotificationDTO> GeneralNotifications()
        {
           await Task.Delay(10);
            GeneralNotificationDTO notification = new GeneralNotificationDTO();

            notification.JobRequisition = _context.JobRequisitions.Where(x => x.Status == TalentAcquisition.Core.Domain.JobRequisition.JobRequisitionStatus.Created).Count();
            notification.ActiveRequisition = 
                _context.JobRequisitions.Where(x => x.Status >= TalentAcquisition.Core.Domain.JobRequisition.JobRequisitionStatus.Approved
                  && x.Status<=TalentAcquisition.Core.Domain.JobRequisition.JobRequisitionStatus.Approved).Count();
            return notification;
        }

        public async Task<EmployeeApprovalDTO> GetApprovalsForEmployeeWithId(int userId)
        {

            EmployeeApprovalDTO approvals = new EmployeeApprovalDTO();
            await Task.Delay(10);
            return approvals;
        }

        public Task GetNotificationForEmployeeWithId(int userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNotifications()
        {
            throw new NotImplementedException();
        }
    }
}
