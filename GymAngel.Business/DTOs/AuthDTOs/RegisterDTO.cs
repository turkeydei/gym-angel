﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymAngel.Business.DTOs.AuthDTOs
{
    public class RegisterDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }


    }
}
