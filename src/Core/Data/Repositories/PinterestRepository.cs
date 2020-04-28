using Core.Data;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.Data
{
    public interface IPinterestRepository : IRepository<Pinterest>
    {
        Task<Pinterest> GetItem(Expression<Func<Pinterest, bool>> predicate);
        Task<IEnumerable<Pinterest>> GetList(Expression<Func<Pinterest, bool>> predicate, Pager pager);
        Task Save(Pinterest pinterest);
    }

    public class PinterestRepository : Repository<Pinterest>, IPinterestRepository
    {
        AppDbContext _db;
        public PinterestRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Pinterest> GetItem(Expression<Func<Pinterest, bool>> predicate)
        {
            try
            {
                var pinterest = _db.Pinterests.Single(predicate);

                if (pinterest != null)
                {
                    pinterest.Images = _db.PinterestImages.Where(p => p.PinterestId == pinterest.Id).ToList();
                }

                return await Task.FromResult(pinterest);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Pinterest>> GetList(Expression<Func<Pinterest, bool>> predicate, Pager pager)
        {
            var take = pager.ItemsPerPage == 0 ? 10 : pager.ItemsPerPage;
            var skip = pager.CurrentPage * take - take;

            var pins = _db.Pinterests.Where(predicate)
                .ToList();

            pager.Configure(pins.Count);

            var list = pins.Skip(skip).Take(take).ToList();

            return await Task.FromResult(list);
        }

        public async Task Save(Pinterest pinterest)
        {
            var dbPinterest = _db.Pinterests.Single(p => p.Id == pinterest.Id);

            if (dbPinterest == null)
            {
                await _db.Pinterests.AddAsync(pinterest);
            }
            else 
            {
                _db.Pinterests.Update(pinterest);
            }
            
            await _db.SaveChangesAsync();
        }

    }
}
