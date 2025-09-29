using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace GymAngel.Domain.Entities
{
    public class Role : IdentityRole<int>
    {
        // Có sẵn Id, Name, NormalizedName, ConcurrencyStamp
        // Nếu cần custom thêm thì viết ở đây
    }
}
