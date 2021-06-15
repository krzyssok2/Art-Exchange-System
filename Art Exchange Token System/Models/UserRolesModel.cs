using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Art_Exchange_Token_System.Models
{
    public class UserRolesModel
    {
        public string UserMail { get; set; }
        public List<string> Roles { get; set; }
    }
}
