using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymAngel.Domain.Entities;

namespace GymAngel.Business.Services.JWT
{
    public interface IJwtTokenService
    {
      
            Task<string> GenerateJwtToken(User user);
        
    }
}
