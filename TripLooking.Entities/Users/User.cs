using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentSatisfaction.Entities;

namespace StudentSatisfaction.Entities.Users
{
    public sealed class User:Entity
    {
        public User(string type, Guid personalDataId):base()
        {
            Type = type;
            Id_Date_Personale = personalDataId;
        }

        public string Type { get; set; }
        public Guid Id_Date_Personale { get; set; }
    }
}
