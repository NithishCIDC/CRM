﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.domain.Model
{
    public class UserRoles
    {
        [Key]
        public Guid Id { get; set; }
        public string? Role { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
