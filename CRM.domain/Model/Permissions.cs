using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.domain.Model
{
    public class Permissions
    {
        [Key]
        public Guid Id { get; set; }
        public string? Permission { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
