using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymAngel.Business.DTOs.AuthDTOs;
using GymAngel.Business.Services.JWT;
using GymAngel.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymAngel.Business.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _jwtService;

        public AuthService(UserManager<User> userManager, IJwtTokenService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }
        public async Task<LoginResultDTO> LoginAsync(LoginDTO dto)
        {
            User user = dto.UsernameOrEmail.Contains("@")? await _userManager.FindByEmailAsync(dto.UsernameOrEmail)
                : await _userManager.FindByNameAsync(dto.UsernameOrEmail);
            if(user == null)
                return new LoginResultDTO { Success = false, Message = "Invalid username or email" };
            var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if(validPassword && !user.EmailConfirmed)
                return new LoginResultDTO { Success = false, Message = "Please confirm your email before login." };
            var token = await _jwtService.GenerateJwtToken(user);
            return new LoginResultDTO
            {
                Success = true,
                Token = token
            };
        
        }
   
      

    }
}
