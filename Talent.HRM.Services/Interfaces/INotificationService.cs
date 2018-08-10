using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.HRM.Services.Notification;

namespace Talent.HRM.Services.Interfaces
{
    public interface INotificationService
    {
       Task UpdateNotifications();
       Task<GeneralNotificationDTO> GeneralNotifications();
       Task GetNotificationForEmployeeWithId(int userId);
       Task<EmployeeApprovalDTO> GetApprovalsForEmployeeWithId(int userId);
    }
}
