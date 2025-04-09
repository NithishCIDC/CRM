using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.domain.Model
{
    public class Branch:BaseModel
    {
        public string? BranchName { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        public ICollection<User>? Users { get; set; } = [];
    }
}
