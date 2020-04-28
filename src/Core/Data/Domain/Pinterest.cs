using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Data
{
    public class Pinterest
    {
        public Pinterest() { }
        
        public int Id { get; set; }

        public int BlogPostId { get; set; }
 
        [Url]
        public string PinUrl { get; set; }
        
        public virtual List<PinterestImage> Images { get; set; }

    }
    public class PinterestImage
    {
        public PinterestImage() { }
        
        public int Id { get; set; }
        
        public int PinterestId { get; set; }

        public string ImageUrl { get; set; }
    }
}
