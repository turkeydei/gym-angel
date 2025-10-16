using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymAngel.Business.DTOs.AuthDTOs
{
    public class ConfirmEmailDTO
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Token { get; set; }
    }
}
