using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext DbContext;
        public SQLWalkRepository(NZWalksDBContext dbContext)
        {
            DbContext = dbContext;
        }



        public async Task<Walk> createAsync(Walk walk)
        {
            await DbContext.Walks.AddAsync(walk);
            await DbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<List<Walk>> GetAsync(string? filterOn = null, string? filterQuery = null,
            string? sortOn = null, bool isAsc = true, int pageNumber = 1, int pageSize = 1000)
        {
            var Walks = DbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            if (!(String.IsNullOrEmpty(filterOn)) && !(String.IsNullOrEmpty(filterQuery)))
            {
                if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    Walks = Walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            if (!(String.IsNullOrEmpty(sortOn)))
            {
                if (sortOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    Walks = isAsc ? Walks.OrderBy(x => x.Name) : Walks.OrderByDescending(x => x.Name);
                }
                else if (sortOn.Equals("length", StringComparison.OrdinalIgnoreCase))
                {
                    Walks = isAsc ? Walks.OrderBy(x => x.LengthInKm) : Walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            var skipResults = (pageNumber - 1) * pageSize;


            return await Walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }
        public async Task<Walk?> GetIdAsync(Guid id)
        {
            return await DbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, UpdateWalkRequestDTO request)
        {
            var existingWalk = await DbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = request.Name;
            existingWalk.LengthInKm = request.LengthInKm;
            existingWalk.WalkImageUrl = request.WalkImageUrl;
            existingWalk.DifficultyId = request.DifficultyId;
            existingWalk.RegionId = request.RegionId;

            await DbContext.SaveChangesAsync();
            return existingWalk;
        }
        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await DbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }

            DbContext.Walks.Remove(existingWalk);
            await DbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
