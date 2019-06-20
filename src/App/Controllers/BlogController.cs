using Core.Data;
using Core.Helpers;
using Core.Services;
using Markdig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace App.Controllers
{
    public class BlogController : Controller
    {
        IDataService _db;
        IFeedService _ss;
        SignInManager<AppUser> _sm;
        private readonly ICompositeViewEngine _viewEngine;
        static readonly string _listView = "~/Views/Themes/{0}/ListParallax.cshtml";

        public BlogController(IDataService db, IFeedService ss, SignInManager<AppUser> sm, ICompositeViewEngine viewEngine)
        {
            _db = db;
            _ss = ss;
            _sm = sm;
            _viewEngine = viewEngine;
        }

        public async Task<IActionResult> Index(int page = 1, string term = "")
        {
            var blog = await _db.CustomFields.GetBlogSettings();
            var pager = new Pager(page, blog.ItemsPerPage);
            IEnumerable<PostItem> posts;

            if (string.IsNullOrEmpty(term))
            {
                posts = await _db.BlogPosts.GetList(p => p.Published > DateTime.MinValue, pager);
            }
            else
            {
                posts = await _db.BlogPosts.Search(pager, term);
            }

            if (pager.ShowOlder) pager.LinkToOlder = $"blog?page={pager.Older}";
            if (pager.ShowNewer) pager.LinkToNewer = $"blog?page={pager.Newer}";

            var model = new ListModel {
                Blog = blog,
                PostListType = PostListType.Blog,
                Posts = posts,
                Pager = pager
            };

            if (!string.IsNullOrEmpty(term))
            {
                model.Blog.Title = term;
                model.Blog.Description = "";
                model.PostListType = PostListType.Search;
            }

            return View(string.Format(_listView, blog.Theme), model);
        }

        [Route("posts/{slug}")]
        public async Task<IActionResult> Single(string slug)
        {
            try
            {
                var model = await _db.BlogPosts.GetModel(slug);
                model.Post.Content = Markdown.ToHtml(model.Post.Content);

                model.Blog = await _db.CustomFields.GetBlogSettings();

                model.Blog.Cover = string.IsNullOrEmpty(model.Post.Cover) ? 
                    $"{Url.Content("~/")}{model.Blog.Cover}" : 
                    $"{Url.Content("~/")}{model.Post.Cover}";
                model.Blog.Title = model.Post.Title;

                return View($"~/Views/Themes/{model.Blog.Theme}/Post.cshtml", model);
            }
            catch
            {
                return Redirect("~/error/404");
            }
            
        }

        [Route("authors/{name}")]
        public async Task<IActionResult> Authors(string name, int page = 1)
        {
            var blog = await _db.CustomFields.GetBlogSettings();
            var author = await _db.Authors.GetItem(a => a.AppUserName == name);

            var pager = new Pager(page, blog.ItemsPerPage);
            var posts = await _db.BlogPosts.GetList(p => p.Published > DateTime.MinValue && p.AuthorId == author.Id, pager);

            if (pager.ShowOlder) pager.LinkToOlder = $"authors/{name}?page={pager.Older}";
            if (pager.ShowNewer) pager.LinkToNewer = $"authors/{name}?page={pager.Newer}";

            var model = new ListModel {
                PostListType = PostListType.Author,
                Author = author,
                Posts = posts,
                Pager = pager
            };

            model.Blog = blog;
            model.Blog.Cover = $"{Url.Content("~/")}{model.Blog.Cover}";
            model.Blog.Description = "";

            return View(string.Format(_listView, model.Blog.Theme), model);
        }

        [Route("categories/{name}")]
        public async Task<IActionResult> Categories(string name, int page = 1)
        {
            var blog = await _db.CustomFields.GetBlogSettings();
            var pager = new Pager(page, blog.ItemsPerPage);
            var posts = await _db.BlogPosts.GetListByCategory(name, pager);

            if (pager.ShowOlder) pager.LinkToOlder = $"categories/{name}?page={pager.Older}";
            if (pager.ShowNewer) pager.LinkToNewer = $"categories/{name}?page={pager.Newer}";

            var model = new ListModel {
                PostListType = PostListType.Category,
                Posts = posts,
                Pager = pager
            };

            model.Blog = blog;
            model.Blog.Cover = $"{Url.Content("~/")}{model.Blog.Cover}";

            ViewBag.Category = name;
            model.Blog.Description = "";

            return View(string.Format(_listView, model.Blog.Theme), model);
        }

        [Route("feed/{type}")]
        public async Task Rss(string type)
        {
            Response.ContentType = "application/xml";
            string host = Request.Scheme + "://" + Request.Host;

            using (XmlWriter xmlWriter = XmlWriter.Create(Response.Body, new XmlWriterSettings() { Async = true, Indent = true }))
            {
                var posts = await _ss.GetEntries(type, host);

                if (posts != null && posts.Count() > 0)
                {
                    var lastUpdated = posts.FirstOrDefault().Published;
                    var writer = await _ss.GetWriter(type, host, xmlWriter);

                    foreach (var post in posts)
                    {
                        post.Description = Markdown.ToHtml(post.Description);
                        await writer.Write(post);
                    }
                }
            }
        }

        [Route("error/{code:int}")]
        public async Task<IActionResult> Error(int code)
        {
            var model = new PostModel();

            model.Blog = await _db.CustomFields.GetBlogSettings();
            model.Blog.Cover = $"{Url.Content("~/")}{model.Blog.Cover}";

            var viewName = $"~/Views/Themes/{model.Blog.Theme}/Error.cshtml";
            var result = _viewEngine.GetView("", viewName, false);

            if (result.Success)
            {
                return View(viewName, model);
            }
            else
            {
                return View("~/Views/Shared/_Error.cshtml", model);
            }
        }

        [HttpPost, Route("account/logout")]
        public async Task<IActionResult> Logout()
        {
            await _sm.SignOutAsync();
            return Redirect("~/");
        }

        // PCaskey Customizations
        [Route("aboutus")]
        public async Task<IActionResult> AboutUs()
        {
            try
            {
                var model = new PostModel();
                model.Blog = await _db.CustomFields.GetBlogSettings();

                var viewName = $"~/Views/Themes/{model.Blog.Theme}/AboutUs.cshtml";

                return View(viewName, model);
            }
            catch
            {
                return Redirect("~/error/404");
            }
        }

        [Route("gallery")]
        public async Task<IActionResult> Gallery()
        {
            try
            {
                var model = new PostModel();
                model.Blog = await _db.CustomFields.GetBlogSettings();

                var viewName = $"~/Views/Themes/{model.Blog.Theme}/Gallery.cshtml";

                return View(viewName, model);
            }
            catch
            {
                return Redirect("~/error/404");
            }
        }

        [Route("collections")]
        public async Task<IActionResult> Collections()
        {
            try
            {
                var model = new GalleriesModel();
                model.Blog = await _db.CustomFields.GetBlogSettings();


                var directoryPath = Directory.GetCurrentDirectory() + "\\wwwroot\\data\\gallery";
                var directories = Directory.GetDirectories(directoryPath);
                var galleries = new List<GalleryModel>();

                foreach (string dirPath in directories)
                {
                    string directoryName = new DirectoryInfo(dirPath).Name;
                    var gallery = new GalleryModel
                    {
                        Directory = dirPath,
                        Title = directoryName,
                        Blog = model.Blog,
                        Slug = directoryName.Replace(" ", "-")
                    };

                    galleries.Add(gallery);
                }

                model.Galleries = galleries;
                var viewName = $"~/Views/Themes/{model.Blog.Theme}/GalleryCollections.cshtml";

                return View(viewName, model);
            }
            catch
            {
                return Redirect("~/error/404");
            }
        }

        [Route("collections/{slug}")]
        public async Task<IActionResult> Collection(string slug)
        {
            try
            {
                var directoryPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\data\\gallery\\{slug.Replace("-", " ")}";
                var virtualPath = $"/data/gallery/{slug.Replace("-", " ")}/";

                var blog = await _db.CustomFields.GetBlogSettings();

                var model = new GalleryModel()
                {
                    Blog = blog,
                    Directory = directoryPath,
                    Slug = slug,
                    Title = slug.Replace("-", " ")
                };
                model.Blog = blog;

                var images = new List<GalleryImageModel>();

                var files = Directory.GetFiles(directoryPath);
                
                foreach (string file in files)
                {
                    var image = new GalleryImageModel()
                    {
                        Name = Path.GetFileName(file),
                        Path = virtualPath + Path.GetFileName(file)
                    };
                    images.Add(image);
                }
                model.GalleryImages = images;

                var viewName = $"~/Views/Themes/{model.Blog.Theme}/GalleryCollection.cshtml";

                return View(viewName, model);
            }
            catch
            {
                return Redirect("~/error/404");
            }
        }

        [Route("Van")]
        public async Task<IActionResult> Van()
        {
            try
            {
                var model = new PostModel();
                model.Blog = await _db.CustomFields.GetBlogSettings();

                var viewName = $"~/Views/Themes/{model.Blog.Theme}/Van.cshtml";

                return View(viewName, model);
            }
            catch
            {
                return Redirect("~/error/404");
            }
        }

        [Route("Route")]
        public async Task<IActionResult> Route()
        {
            try
            {
                var model = new PostModel();
                model.Blog = await _db.CustomFields.GetBlogSettings();

                var viewName = $"~/Views/Themes/{model.Blog.Theme}/Route.cshtml";

                return View(viewName, model);
            }
            catch
            {
                return Redirect("~/error/404");
            }
        }

    }
}