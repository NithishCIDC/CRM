using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.domain.Model
{
    public class Organization:BaseModel
    {
        public string? Org_Name { get; set; }
        public string? Org_type { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Website { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Branch> Branches { get; set; } = [];
        public ICollection<UserRoles> Roles { get; set; } = [];
    }
}
 