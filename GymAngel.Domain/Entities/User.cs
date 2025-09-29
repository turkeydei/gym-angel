using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace GymAngel.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? MembershipExpiry { get; set; }
        public DateTime MembershipStart { get; set; }
    }
}
