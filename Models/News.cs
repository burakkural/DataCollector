using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class News
    {
        //public int Id { get; set; }
        public string Title { get; set; }
        public String Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public NEWS_SOURCE Source { get; set; }
        //public  DateTime CreatedOn { get; set; }
        //public  DateTime? UpdatedOn { get; set; }
        //public  bool IsDeleted { get; set; }
    }
}
