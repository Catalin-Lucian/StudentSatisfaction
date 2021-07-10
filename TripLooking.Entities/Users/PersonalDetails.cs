using System;

namespace StudentSatisfaction.Entities.Users
{
    public sealed class PersonalDetails:Entity
    {
        public PersonalDetails(string firstName,string lastName,int age,DateTime birthDate,string email,string phone,Guid facultyDetailsId):base()
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            BirthDate = birthDate;
            Email = email;
            Phone = phone;
            FacultyDetailsId = facultyDetailsId;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int Age { get; private set; }
        public DateTime BirthDate { get; private set; }

        public string Email { get; private set; }
        public string Phone { get; private set; }
        public Guid FacultyDetailsId { get; private set; }
    }
}