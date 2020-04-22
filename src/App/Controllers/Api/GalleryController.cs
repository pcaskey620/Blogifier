using Core.Data;
using Core.Data.Models;
using Core.Helpers;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace App.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : Controller
    {
        IDataService _data;

        public GalleryController(IDataService data)
        {
            _data = data;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gallery>>> Get(int page = 1)
        {
            try
            {
                var galleries = await _data.Galleries.GetList();

                return Ok(galleries.ToList().OrderBy(g => g.Title));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Gallery>> Post(Gallery model)
        {
            try
            {
                var existing = _data.Galleries.Single(a => a.Id == model.Id);

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data");
                }

                model.Slug = model.Title.Replace(" ", "-");

                Gallery gallery = null;
                if (existing == null)            
                {                    
                    var directoryPath = $"/data/gallery/{model.Slug}";
                    model.Directory = directoryPath;

                    var directoryFullPath = Directory.GetCurrentDirectory() + $"\\wwwroot\\data\\gallery\\{model.Slug}";

                    if (!Directory.Exists(directoryFullPath))
                    {
                        Directory.CreateDirectory(directoryFullPath);
                    }

                    await _data.Galleries.SaveItem(model);
                    gallery = _data.Galleries.Single(a => a.Title == model.Title);

                    return Created($"/api/gallery/{model.Title}", gallery);
                }    
                else
                {
                    await _data.Galleries.SaveItem(model);
                    gallery = _data.Galleries.Single(a => a.Id == model.Id);

                    return Accepted($"/api/gallery/{model.Title}", gallery);
                }                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class GalleryEditController : Controller
    {
        IDataService _data;

        public GalleryEditController(IDataService data)
        {
            _data = data;
        }

        [HttpGet]
        public async Task<ActionResult<GalleryViewModel>> Get(int id, int page = 1)
        {
            var gallery = _data.Galleries.Single(g => g.Id == id);

            var directoryPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\{gallery.Directory}";
            var virtualPath = $"{gallery.Directory}/";

            var images = new List<GalleryImage>();

            var files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                var image = new GalleryImage()
                {
                    Name = Path.GetFileName(file),
                    Path = virtualPath + Path.GetFileName(file)
                };
                images.Add(image);
            }

            var pager = new Pager(page, 12);

            var skip = pager.CurrentPage * pager.ItemsPerPage - pager.ItemsPerPage;
            pager.Configure(images.Count());

            if (pager.ShowOlder) pager.LinkToOlder = $"collections?page={pager.Older}";
            if (pager.ShowNewer) pager.LinkToNewer = $"collections?page={pager.Newer}";
            pager.LinkBase = $"collections";

            var modelImages = images.Skip(skip).Take(pager.ItemsPerPage).ToList();

            //var blog = await _db.CustomFields.GetBlogSettings();

            var model = new GalleryViewModel()
            {
                Gallery = gallery,
                Images = modelImages,
                Pager = pager

            };

            return Ok(model);
        }

        [HttpGet("ID/{id}", Name = "GetL")]
        public async Task<ActionResult<Gallery>> GetGalleryDetails(int id)
        {
            var gallery = _data.Galleries.Single(g => g.Id == id);

            return Ok(gallery);
        }
    }
}
