using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Models.Notifications
{
    public sealed class CreateNotificationModel
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
    }
}
