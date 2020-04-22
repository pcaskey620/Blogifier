using System;
using System.Collections.Generic;
using System.Text;
using Core.Helpers;

namespace Core.Data.Models
{
    public class GalleriesModel
    {
        public BlogItem Blog { get; set; }

        public List<Gallery> Galleries { get; set; }

        public Pager Pager { get; set; }
    }
}
