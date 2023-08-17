using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DTO
{
    public class ForgotPasswordResponse : Status
    {
        [Required]
        public string Username { get; set; }
        public string PasswordResetToken { get; set; }
    }
}
