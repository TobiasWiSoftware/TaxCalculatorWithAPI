using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculatorLibary.Models
{
    public class User : IdentityUser
    {
       public User(string userName) : base(userName)
        {
        }

    }
}
