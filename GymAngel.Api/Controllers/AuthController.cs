using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymAngel.Data;
using GymAngel.Domain.Entities;
using GymAngel.Business.Services.Auth;
using GymAngel.Business.DTOs.AuthDTOs;

namespace GymAngel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly EmailService _emailService;
        public AuthController(IAuthService authService, EmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Business.DTOs.AuthDTOs.LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (!result.Success)
                return BadRequest(new { message = result.Message });
            //await _emailService.SendEmailAsync("chikietsg@gmail.com", "Test Mail", "<b>Gửi thử thành công!</b>");
            return Ok(new { token = result.Token });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Gọi service, nhận chuỗi kết quả
                string message = await _authService.RegisterAsync(dto);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                // Nếu service ném lỗi, trả về 400
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
