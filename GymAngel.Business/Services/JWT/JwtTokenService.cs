using GymAngel.Business.Services.JWT;
using GymAngel.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GymAngel.Business.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;

        public JwtTokenService(IConfiguration config, UserManager<User> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<string> GenerateJwtToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            //Claim = thông tin định danh mà ta muốn “nhét” vào token.
            //Mỗi claim là một cặp key → value, ví dụ:
            //| Claim Type | Ý nghĩa | Giá trị |
            //| ---------------- | -------------------------       | ------------------ |
            //| `NameIdentifier` | ID của user                     | `"c2a8f..."`       |
            //| `Name`           | Tên đăng nhập                   | `"kiet.ho"`        |
            //| `Email`          | Email user                      | `"kiet@gmail.com"` |
            //| `Jti`            | ID duy nhất cho mỗi token       | `"d5e83c0b..."`    |

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            //→ Mỗi role sẽ được thêm vào claim, để token biết user có quyền gì.

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//kí để kh thể sửa token nếu kh biết key

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

     
    }
}
