using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOGIC.Models
{
    public class ArtListModel
    {
        public List<ArtDataModel> ArtList { get; set; }
    }
    public class ArtDataModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ArtCategoryModel Catgegory { get; set; }
        public string FileName { get; set; }
    }
    public class ArtCategoryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
