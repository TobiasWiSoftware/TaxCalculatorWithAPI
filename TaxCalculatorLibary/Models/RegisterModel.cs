using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculatorLibary.Models
{
    public class RegisterModel
    {
        [Required]
        //[EmailAddress]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public RegisterModel()
        {

        }
   
        public RegisterModel(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
