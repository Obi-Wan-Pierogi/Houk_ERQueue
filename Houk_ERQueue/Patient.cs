using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Houk_ERQueue
{
    internal class Patient
    {
        private string _LastName { get; } 
        private string _FirstName { get; }
        private DateOnly _Birthdate { get; }
        private int Priority { get; }

        // constructor. if age is < 21 or > 65, priority is 1.
        public Patient(string lastName, string firstName, DateOnly birthdate, int priority)
        {
            this._LastName = lastName;
            this._FirstName = firstName;
            this._Birthdate = birthdate;
            if (DateTime.Today.Year - _Birthdate.Year < 21 || DateTime.Today.Year - _Birthdate.Year > 65)
            {
                this.Priority = 1;
            }
            else
            {
                this.Priority = priority;
            }
        }
        //Overrides the ToString method to return the patient's name, birthdate, and priority
        public override string ToString()
        {
            return _LastName + ", " + _FirstName + ", " + _Birthdate + ", " + Priority;

        }
        //Getters for the patient's name, birthdate, and priority
        public string GetLastName()
        {
            return _LastName;
        }
        public string GetFirstName()
        {
            return _FirstName;
        }
        public DateOnly GetBirthdate()
        {
            return _Birthdate;
        }
        public int GetPriority()
        {
            return Priority;
        }
    }
}
