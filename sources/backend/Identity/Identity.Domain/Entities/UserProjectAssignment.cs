﻿using AcademyCloud.Identity.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.Identity.Domain.Entities
{
    public class UserProjectAssignment
    {
        public Guid Id { get; set; }

        public virtual User User { get; set; }

        public virtual Project Project { get; set; }

        public UserRole Role { get; set; }

        public UserProjectAssignment(Guid id, User user, Project project, UserRole role)
        {
            Id = id;
            User = user;
            Project = project;
            Role = role;
        }

        protected UserProjectAssignment() { }
    }
}
