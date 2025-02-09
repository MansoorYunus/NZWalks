using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionInterface
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetRegionById(Guid id);
        Task<Region> Create(Region region);
        Task<Region> Update(Region region, Guid id);
        Task<Region> Delete(Guid id);
    }
}
