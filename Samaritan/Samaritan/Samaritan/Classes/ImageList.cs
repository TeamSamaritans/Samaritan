using System;
using System.Collections.Generic;
using System.Text;

namespace Samaritan.Classes
{
   public class ImageList : BaseModel
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string image_src { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string craeted_at { get; set; }
        public string updated_at { get; set; }
    }
}
