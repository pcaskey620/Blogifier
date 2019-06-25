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
    public class EditModel : AdminPageModel
    {
        public Core.Data.Gallery Gallery { get; set; }
        public BlogItem Blog { get; set; }

        IDataService _db;
        INotificationService _ns;


        public EditModel(IDataService db, INotificationService ns)
        {
            _db = db;
            _ns = ns;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Blog = await _db.CustomFields.GetBlogSettings();

            Gallery = _db.Galleries.Single(g => g.Id == id);

            return Page();
        }
    }
}