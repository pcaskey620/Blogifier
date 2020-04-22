using Core.Data;
using Core.Data.Models;
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

        [Route("blog")]
        public async Task<IActionResult> Posts(int page = 1, string term = "")
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
                model.Blog.Description = model.Post.Description;

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
        public async Task<IActionResult> Collections(int page = 1, string season = "")
        {
            try
            {
                var model = new GalleriesModel();
                model.Blog = await _db.CustomFields.GetBlogSettings();

                // Refactor to use _db.Galleries
                var galleries = string.IsNullOrWhiteSpace(season) ? await _db.Galleries.GetList() : await _db.Galleries.GetListBySeason(season);

                galleries.ToList().ForEach(g => g.CoverImagePath = $"{g.Directory}/cover.jpg");
              
                var pager = new Pager(page, 12);

                var skip = pager.CurrentPage * pager.ItemsPerPage - pager.ItemsPerPage;
                pager.Configure(galleries.Count());

                if (pager.ShowOlder) pager.LinkToOlder = $"collections?page={pager.Older}";
                if (pager.ShowNewer) pager.LinkToNewer = $"collections?page={pager.Newer}";
                pager.LinkBase = $"collections";

                model.Galleries = galleries.OrderBy(g => g.Title).Skip(skip).Take(pager.ItemsPerPage).ToList();
                model.Pager = pager;

                var viewName = $"~/Views/Themes/{model.Blog.Theme}/GalleryCollections.cshtml";

                return View(viewName, model);
            }
            catch
            {
                return Redirect("~/error/404");
            }
        }

        [Route("collections/{slug}")]
        public async Task<IActionResult> Collection(string slug, int page = 1)
        {
            try
            {
                var directoryPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\data\\gallery\\{slug}";
                var virtualPath = $"/data/gallery/{slug}/";

                var blog = await _db.CustomFields.GetBlogSettings();

                var files = Directory.GetFiles(directoryPath);
                var totalFileCount = files.Count();

                var pager = new Pager(page, 12);
                
                var skip = pager.CurrentPage * pager.ItemsPerPage - pager.ItemsPerPage;
                pager.Configure(totalFileCount);

                if (pager.ShowOlder) pager.LinkToOlder = $"collections/{slug}?page={pager.Older}";
                if (pager.ShowNewer) pager.LinkToNewer = $"collections/{slug}?page={pager.Newer}";
                pager.LinkBase = $"collections/{slug}";

                var images = new List<GalleryImage>();
                
                foreach (string file in files)
                {
                    var image = new GalleryImage()
                    {
                        Name = Path.GetFileName(file),
                        Path = virtualPath + Path.GetFileName(file)
                    };
                    images.Add(image);
                }

                var pageImages = images.Skip(skip).Take(pager.ItemsPerPage).ToList();

                var gallery = new Gallery()
                {
                    Directory = directoryPath,
                    Slug = slug,
                    Title = slug.Replace("-", " "),
                    GalleryImages = pageImages
                };

                var model = new GalleryViewModel()
                {
                    Blog = blog,
                    Gallery = gallery,
                    Pager = pager
                };                               

                var viewName = $"~/Views/Themes/Materialize/GalleryCollection.cshtml";

                return View(viewName, model);
            }
            catch
            {
                return Redirect("~/error/404");
            }
        }

        [Route("van")]
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

        [Route("route")]
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

        [Route("videos")]
        public async Task<IActionResult> Videos()
        {
            try
            {
                var model = new PostModel();
                model.Blog = await _db.CustomFields.GetBlogSettings();

                var viewName = $"~/Views/Themes/{model.Blog.Theme}/Videos.cshtml";

                return View(viewName, model);
            }
            catch
            {
                return Redirect("~/error/404");
            }
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var pager = new Pager(1, 10);
                IEnumerable<PostItem> featuredPosts;

                featuredPosts = await _db.BlogPosts.GetList(x => x.IsFeatured == true, pager);
                var recentFeaturedPost = featuredPosts.OrderByDescending(i => i.Published).Take(3);

                var directoryPath = Directory.GetCurrentDirectory() + "\\wwwroot\\data\\gallery";

                // defaulting to first 6 directories
                var directories = Directory.GetDirectories(directoryPath).Take(6);
                var galleries = new List<Gallery>();

                foreach (string dirPath in directories)
                {                  
                    string directoryName = new DirectoryInfo(dirPath).Name;
                    string coverImagePath = $"/data/gallery/{directoryName}/cover.jpg";
                    var gallery = new Gallery
                    {
                        Directory = dirPath,
                        Title = directoryName.Replace("-", " "),
                        //Blog = model.Blog,
                        Slug = directoryName.Replace(" ", "-"),
                        CoverImagePath = coverImagePath
                    };

                    galleries.Add(gallery);
                }

                var model = new ListModel
                {
                    PostListType = PostListType.Blog,
                    Posts = recentFeaturedPost,
                    Pager = pager,
                    Galleries = galleries
                };
                model.Blog = await _db.CustomFields.GetBlogSettings();

                var viewName = $"~/Views/Themes/{model.Blog.Theme}/Home.cshtml";

                return View(viewName, model);
            }
            catch
            {
                return Redirect("~/error/404");
            }
        }

        [Route("youtube")]
        public void Youtube()
        {
            Response.Redirect("https://www.youtube.com/channel/UCvvYRmVC0_VZdsyJj0Ldapw/featured");
        }

        [Route("instagram")]
        public void Instagram()
        {
            Response.Redirect("https://www.instagram.com/parks.and.wils/");
        }

    }
}