using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Users
{
    public sealed class Notification: Entity
    {
        public Notification(Guid userId, string message): base()
        {

        }

        public Guid UserId { get; private set; }
        public string Message { get; private set; }
    }
}
