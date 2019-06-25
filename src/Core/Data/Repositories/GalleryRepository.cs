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

        public async Task<Gallery> SaveItem(Gallery item)
        {
            if (item.Id == 0)
            {
                // add new
                _db.Gallery.Add(item);
                await _db.SaveChangesAsync();
            }
            else
            {
                // update exising
            }
            return await Task.FromResult(item);
        }
    }
}
