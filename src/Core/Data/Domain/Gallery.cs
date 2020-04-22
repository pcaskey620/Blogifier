using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Core.Data
{
    public class Gallery
    {
        public Gallery() { }

        public int Id { get; set; }

        //public int AuthorId { get; set; }

        public string Directory { get; set; }

        [Required]
        [StringLength(160)]
        public string Title { get; set; }

        [StringLength(160)]
        public string Slug { get; set; }

        public List<GalleryImage> GalleryImages { get; set; }

        [StringLength(250)]
        public string CoverImagePath { get; set; }

        [StringLength(160)]
        public string Season { get; set; }
    }

    public class GalleryImage
    {
        public int Id { get; set; }

        public string Path { get; set; }
        
        public string Name { get; set; }
    }
}
