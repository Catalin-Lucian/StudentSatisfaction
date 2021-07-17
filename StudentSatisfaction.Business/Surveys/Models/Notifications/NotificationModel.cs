﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Models.Notifications
{
    public sealed class NotificationModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; private set; }
        public string Message { get; private set; }
    }
}
