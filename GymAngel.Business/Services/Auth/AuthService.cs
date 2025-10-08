using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymAngel.Business.DTOs.AuthDTOs;
using GymAngel.Business.Services.JWT;
using GymAngel.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace GymAngel.Business.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public AuthService(UserManager<User> userManager,
                            IJwtTokenService jwtService,
                            IConfiguration configuration,
                            EmailService emailService
         )
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _configuration = configuration;
            _emailService = emailService;
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
        public async Task<string> RegisterAsync(RegisterDTO dto)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(dto.Email);
            var existingUserByName = await _userManager.FindByNameAsync(dto.UserName);

            if (existingUserByEmail != null || existingUserByName != null)
            {
                throw new Exception("Email hoặc Username đã tồn tại!");
            }

            var user = new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                FullName = dto.FullName,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            await _userManager.AddToRoleAsync(user, "User");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = System.Net.WebUtility.UrlEncode(token);

            var confirmationLink = $"{_configuration["ClientUrl"]}/confirm-email?email={user.Email}&token={encodedToken}";
            var emailBody = $@"
        <div style='max-width:500px;margin:40px auto;padding:32px 24px;background:#222;border-radius:12px;color:#eee;font-family:sans-serif;'>
            <h2 style='text-align:center;margin-bottom:24px;'>Welcome to <b>Gym Angel System!</b></h2>
            <p>Dear <b>{dto.UserName}</b>,</p>
            <p>Thank you for registering an account with <b>Gym Angel System</b>.</p>
            <p>Please click the button below to verify your email address:</p>
            <div style='text-align:center;margin:32px 0;'>
                <a href='{confirmationLink}' style='background:#4FC3F7;color:#222;padding:12px 32px;border-radius:8px;text-decoration:none;font-weight:bold;font-size:18px;display:inline-block;'>Confirm email</a>
            </div>
            <p style='margin-top:32px;'>After verification, you can log in and use all features of the Health Care System.</p>
            <hr style='margin:32px 0;border:none;border-top:1px solid #444;'/>
            <p style='font-size:13px;color:#aaa;text-align:center;'>This email was sent automatically. Please do not reply.</p>
        </div>";

            try
            {
                await _emailService.SendEmailAsync(user.Email,
                    "Confirm account registration - Gym Angel System",
                    emailBody);
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(user);
                throw new Exception("Email không tồn tại hoặc không gửi được. Vui lòng nhập email hợp lệ.");
            }

            return "Đăng ký thành công! Vui lòng kiểm tra email để xác nhận tài khoản.";
        }



    }
}
