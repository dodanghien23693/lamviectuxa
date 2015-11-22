using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSoftSeo.Models
{
    public class Bidder
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime BidDay { get; set; }
        public virtual Job Job { get; set; }
        public bool IsChoosed { get; set; }
        public string Description { get; set; }
        public Bidder()
        {

        }
    }
}
