using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGIC.Models.TransactionModels
{
    public class PostArtTransactionResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public GetTradeInfoModel ArtData { get; set; }
    }
}
