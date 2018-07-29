using System;
using System.Collections.Generic;
using System.Text;

namespace Samaritan.Classes
{
    public class Post : BaseModel
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string image_src { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string craeted_at { get; set; }
        public string updated_at { get; set; }
        public string file { get; set; }

        public string Post_date => $"Uploaded at {string.Format("{0:dd-MMM-yyyy hh:mm tt}", Convert.ToDateTime(craeted_at))}";
    }
}
