﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Domains.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual Domain Domain { get; set; }

        public virtual ICollection<UserProjectAssignment> Users { get; set; }

        public Project(Guid id, string name, Domain domain, ICollection<UserProjectAssignment> users = null)
        {
            Id = id;
            Name = name;
            Domain = domain;
            Users = users ?? new List<UserProjectAssignment>();
        }

        public Project() { }
    }
}