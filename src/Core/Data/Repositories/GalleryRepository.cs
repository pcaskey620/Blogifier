using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Data
{
    public interface IGalleryRepository : IRepository<Gallery>
    {
        Task<IEnumerable<Gallery>> GetList();

        Task<IEnumerable<Gallery>> GetListBySeason(string season);    

        Task<Gallery> SaveItem(Gallery item);
    }

    public class GalleryRepository : Repository<Gallery>, IGalleryRepository
    {
        AppDbContext _db;
        public GalleryRepository(AppDbContext db): base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Gallery>> GetList()
        {
            var galleries = _db.Gallery.ToList();

            var list = galleries;

            return await Task.FromResult(list);

        }

        public async Task<IEnumerable<Gallery>> GetListBySeason(string season)
        {
            var galleries = _db.Gallery.ToList().Where(g => g.Season.ToLower() ==  season.ToLower());

            var list = galleries;

            return await Task.FromResult(list);
        }

        public async Task<Gallery> SaveItem(Gallery item)
        {
            Gallery gallery;
            if (item.Id == 0)
            {
                gallery = new Gallery
                {
                    Title = item.Title,
                    Slug = item.Slug,
                    CoverImagePath = item.CoverImagePath,
                    Directory = item.Directory,
                    Season = item.Season
                };

                _db.Gallery.Add(item);
                await _db.SaveChangesAsync();
            }
            else
            {
                gallery = _db.Gallery.Single(g => g.Id == item.Id);

                gallery.Title = item.Title;
                
                //gallery.Slug = item.Slug;
                //gallery.CoverImagePath = item.CoverImagePath;
                //gallery.Directory = item.Directory;

                gallery.Season = item.Season;

                await _db.SaveChangesAsync();

                item.Slug = gallery.Slug;
            }
            return await Task.FromResult(item);
        }
    }
}
