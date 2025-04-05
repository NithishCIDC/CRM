using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.domain.Model
{
    public class Session
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Expired_At { get; set; }
    }
}
