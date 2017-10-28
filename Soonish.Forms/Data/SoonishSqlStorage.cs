using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Soonish.Forms.Data
{
    public class SoonishSqlStorage:ISoonishStorage
    {
        private readonly FormsDbContext _context;

        public SoonishSqlStorage(FormsDbContext context)
        {
            _context = context;
        }

        public Task<ReadOnlyCollection<Response>> GetResponsesForMonth(int year, int month)
        {
            throw new NotImplementedException();
        }

        public async Task<ReadOnlyCollection<Response>> GetResponsesForYear(int year)
        {
            throw new NotImplementedException();
            //return new ReadOnlyCollection<Response>( await _context.Responses.Where(r => true).ToListAsync());
        }

        public async Task Insert(Response entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
