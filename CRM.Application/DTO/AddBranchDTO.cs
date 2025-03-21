using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTO
{
    public class AddBranchDTO
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? BranchName { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
