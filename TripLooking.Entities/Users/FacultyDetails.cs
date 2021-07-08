using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Users
{
    public sealed class FacultyDetails:Entity
    {
        public FacultyDetails(string name,DateTime startDate,DateTime endDate):base()
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }

        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
    }
}
