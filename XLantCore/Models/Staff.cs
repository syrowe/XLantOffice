﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class Staff: Person
    {
        public Staff()
        {

        }

        public Staff(string id)
        {
            this.PrimaryID = id;
        }

        public Staff(string id, string name)
        {
            this.PrimaryID = id;
            this.Name = name;
        }

        public Staff(string id, string name, string department)
        {
            this.PrimaryID = id;
            this.Name = name;
            this.Department = department;
        }

        public Staff(int id)
        {
            this.PrimaryID = id.ToString();
        }

        public Staff(int id, string name)
        {
            this.PrimaryID = id.ToString();
            this.Name = name;
        }

        public Staff(int id, string name, string department)
        {
            this.PrimaryID = id.ToString();
            this.Name = name;
            this.Department = department;
        }

        public StaffGrade Grade { get; set; }
        public string Username { get; set; }
        public String Department { get; set; }
        public String Office { get; set; }
        public bool Active { get; set; }

    }
}
