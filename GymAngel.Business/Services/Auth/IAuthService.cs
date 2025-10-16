using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymAngel.Business.DTOs.AuthDTOs;

namespace GymAngel.Business.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResultDTO> LoginAsync(LoginDTO dto);
        Task<string> RegisterAsync(RegisterDTO dto);
        Task<string> ForgotPasswordAsync(ForgotPasswordDTO dto);
        Task<string> ResetPasswordAsync(ResetPasswordDTO dto);
        Task<string> ConfirmEmailAsync(ConfirmEmailDTO dto);
        Task<string> ResendConfirmEmailAsync(ForgotPasswordDTO dto);
    }
}
