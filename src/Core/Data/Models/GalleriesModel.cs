using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Data.Models
{
    public class GalleriesModel
    {
        public BlogItem Blog { get; set; }

        public List<Gallery> Galleries { get; set; }
    }
}
