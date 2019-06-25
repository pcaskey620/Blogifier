using Core.Data;
using Core.Helpers;
using Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Pages.Admin.Gallery
{
    public class IndexModel : AdminPageModel
    {
        public IEnumerable<Core.Data.Gallery> Galleries { get; set; }
        public BlogItem Blog { get; set; }

        IDataService _db;
        INotificationService _ns;

        public IndexModel(IDataService db, INotificationService ns)
        {
            _db = db;
            _ns = ns;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Blog = await _db.CustomFields.GetBlogSettings();

            var directoryPath = Directory.GetCurrentDirectory() + "\\wwwroot\\data\\gallery";
            var directories = Directory.GetDirectories(directoryPath);
            var galleries = new List<Core.Data.Gallery>();

            foreach (string dirPath in directories)
            {
                string directoryName = new DirectoryInfo(dirPath).Name;
                string coverImagePath = $"/data/gallery/{directoryName}/cover.jpg";
                coverImagePath = coverImagePath.Replace(" ", "%20");
                var gallery = new Core.Data.Gallery
                {
                    Directory = dirPath,
                    Title = directoryName,
                    //Blog = Blog,
                    Slug = directoryName.Replace(" ", "-"),
                    CoverImagePath = coverImagePath
                };

                galleries.Add(gallery);
            }

            Galleries = galleries;

            return Page();
        }
    }
}