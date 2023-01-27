using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UBA_Network_Security_System.Models
{
    public class JobType
    {
        public int ID { get; set; }
        public string Type { get; set; }

    }

    public class State
    {
        public int ID { get; set; }
        public string Name { get; set; }

    }

    public class Country
    {
        public int ID { get; set; }
        public string Name { get; set; }

    }

    public class IdentificationType
    {
        public int ID { get; set; }
        public string Name { get; set; }

    }

    public class _Gender
    {
        public string Name { get; set; }

    }

    public class UtilityHelpers
    {
        public IEnumerable<JobType> GetJobTypeList()
        {
            return new List<JobType>
            {
               new JobType()
               {
                   ID = 1,
                   Type = "Permanent"
               },
               new JobType()
               {
                   ID = 2,
                   Type = "Remote"
               },
               new JobType()
               {
                   ID = 3,
                   Type = "Part-Time"
               },
               new JobType()
               {
                   ID = 4,
                   Type = "Contractor"
               },
            };
        }

        public IEnumerable<State> GetStates()
        {
            return new List<State>
            {
               new State() {ID = 1,Name = "Abia"},
               new State() { ID = 2, Name = "Anambra"},
               new State() {ID = 3, Name = "Akwa-Ibom"},
               new State() { ID = 4, Name = "Bauchi"},
               new State() { ID = 4, Name = "Bay"},
               new State() { ID = 4, Name = "Divorced"},
               new State() { ID = 4, Name = "Divorced"},
               new State() { ID = 4, Name = "Divorced"},
               new State() { ID = 4, Name = "Divorced"},
               new State() { ID = 4, Name = "Divorced"},
               new State() { ID = 4, Name = "Divorced"},
            };
        }

        public IEnumerable<Country> GetCountryList()
        {
            return new List<Country>
            {
               new Country()
               {
                   ID = 1,
                   Name = "Nigeria"
               },
               new Country()
               {
                   ID = 2,
                   Name = "Ghana"
               },
               new Country()
               {
                   ID = 3,
                   Name = "Egypt"
               },
               new Country()
               {
                   ID = 4,
                   Name = "Cameron"
               },
            };
        }

        public IEnumerable<IdentificationType> GetIdentificationTypeList()
        {
            return new List<IdentificationType>
            {
               new IdentificationType()
               {
                   ID = 1,
                   Name = "National Identity"
               },
               new IdentificationType()
               {
                   ID = 2,
                   Name = "Voters' Card"
               },
               new IdentificationType()
               {
                   ID = 3,
                   Name = "Drivers' License"
               },
               new IdentificationType()
               {
                   ID = 4,
                   Name = "Passport"
               },
            };
        }

        public IEnumerable<_Gender> GetGenders()
        {
            return new List<_Gender>()
            {
                new _Gender()
                {
                    Name = "Male"
                },
                new _Gender()
                {
                    Name = "Female"
                }
            };
        }
    }

}