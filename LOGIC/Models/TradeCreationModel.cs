using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOGIC.Models
{
    public class TradeCreationModel
    {
        public string SecondTraderUserName { get; set; }
        public List<CreationArtDataModel> WantedArt { get; set; }
        public List<CreationArtDataModel> OfferedArt { get; set; }
    }

    public class CreationArtDataModel
    {
        public string ArtFile { get; set; }
    }
}
