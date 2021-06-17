using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC.Models.TransactionModels
{
    public class CreateUserTransactionModel
    {
        public bool Success { get; set; }
        public List<IdentityError> Errors { get; set; }
    }
}
