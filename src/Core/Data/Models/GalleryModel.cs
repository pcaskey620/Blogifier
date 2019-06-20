using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Core.Data
{
    public class GalleriesModel
    {
        public BlogItem Blog { get; set; }
        
        public List<GalleryModel> Galleries { get; set; }
    }

    public class GalleryModel
    {
        public BlogItem Blog { get; set; }

        [Required]
        public string Directory { get; set; }

        [Required]
        public string Title { get; set; }

        public string Slug { get; set; }

        public List<GalleryImageModel> GalleryImages { get; set; }
    }

    public class GalleryImageModel
    {
        public string Path { get; set; }
        
        public string Name { get; set; }
    }
}
