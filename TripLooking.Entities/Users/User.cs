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
        public User(string type, Guid personalDataId,Guid logInId):base()
        {
            Type = type;
            PersonalDataId = personalDataId;
            logInId = logInId;
            //PersonalData= new PeersonalDetails();
        }

        public string Type { get; private set; }
        public Guid PersonalDataId { get;private set; }

        public Guid LogInId { get; private set; }

        //public PeersonalDetails PersonalData { get; private set; }

        
    }
}
