using Core.Data;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System;

namespace Common.Widgets
{
    [ViewComponent(Name = "Pinterest")]
    public class Pinterest : ViewComponent
    {
        IDataService _db;
        public Pinterest(IDataService db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(string theme, string widget, int postModelId)
        {
            // https://www.pinterest.com/pin/create/button/?url=&media=&description=

            var pinterest = await _db.Pinterests.GetItem(p => p.BlogPostId == postModelId);
                   
            //string urlParam = HttpUtility.UrlEncode($"https://www.parksandwils.com/posts/{slug}");
            //string mediaParam = HttpUtility.UrlEncode("https://aajoaihlwo.cloudimg.io/cdno/n/n/https://www.parksandwils.com/" + "/data/admin/2020/4/dave-and-matt-vans-pinterest.png");
            //string descriptionParam = ""; // slug.Replace("-", " ");

            //var builder = new UriBuilder("https://www.pinterest.com/pin/create/button");
            //var query = HttpUtility.ParseQueryString(builder.Query);
            //query["url"] =urlParam;
            //query["media"] = mediaParam;
            //query["description"] = descriptionParam;

            //builder.Query = query.ToString();
            //string url = builder.ToString();
                        
            return View("~/Views/Widgets/Pinterest/Index.cshtml", pinterest);
        }
    }
}
