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
        public async Task<ActionResult<IEnumerable<Gallery>>> Get()
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
                var existing = _data.Galleries.Single(a => a.Title == model.Title);
                if (existing != null)
                {
                    return BadRequest("Collection already exists");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data");
                }

                model.Slug = model.Title.Replace(" ", "-");

                var directoryPath = $"/data/gallery/{model.Slug}";
                model.Directory = directoryPath;

                var directoryFullPath = Directory.GetCurrentDirectory() + $"\\wwwroot\\data\\gallery\\{model.Slug}";

                if (!Directory.Exists(directoryFullPath))
                {
                    Directory.CreateDirectory(directoryFullPath);
                }
                
                // add gallery to app database 
                await _data.Galleries.SaveItem(model);
                var created = _data.Galleries.Single(a => a.Title == model.Title);
                return Created($"/api/gallery/{model.Title}", created);
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
        public async Task<ActionResult<IEnumerable<GalleryImage>>> Get(int id)
        {
            var gallery = _data.Galleries.Single(g => g.Id == id);

            var directoryPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\data\\gallery\\{gallery.Slug}";
            var virtualPath = $"/data/gallery/{gallery.Slug}/";

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

            return Ok(images);
        }
    }
}
