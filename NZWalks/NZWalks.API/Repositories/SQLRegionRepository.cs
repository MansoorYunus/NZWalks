using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionInterface
    {
        private readonly NZWalksDBContext _dbContext;
        public SQLRegionRepository(NZWalksDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<Region> Create(Region region)
        {
            await _dbContext.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync();
        }
        public async Task<Region?> GetRegionById(Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> Update(Region request, Guid id)
        {
            var region = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (region != null)
            {
                region.Code = request.Code;
                region.Name = request.Name;
                region.RegionImageUrl = request.RegionImageUrl;

                await _dbContext.SaveChangesAsync();

                return region;
            }
            else
            {
                return null;
            }
        }
        public async Task<Region?> Delete(Guid id)
        {
            var region =  await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region != null)
            {
                _dbContext.Regions.Remove(region);
                await _dbContext.SaveChangesAsync();
                return region;
            }
            else { return null; }
        }

    }
}
