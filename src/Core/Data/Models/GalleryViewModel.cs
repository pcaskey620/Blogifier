using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Data.Models
{
    public class GalleryViewModel
    {
        public BlogItem Blog { get; set; }

        public Gallery Gallery { get; set; }

        public List<GalleryImage> Images { get; set; }

    }
}
