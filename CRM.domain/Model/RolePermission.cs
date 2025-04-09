using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.domain.Model
{
    public class RolePermission
    {
        [Key]
        public Guid Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public UserRoles? Role { get; set; }
        public Permissions? Permission { get; set; }
    }
}
