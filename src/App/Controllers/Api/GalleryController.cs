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
                return Ok(galleries);
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
}
