using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> createAsync(Walk walk);
        Task<List<Walk>> GetAsync(string? filterOn = null, string? filterQuery = null, string? sortOn = null, bool isAsc = true, int pageNumber = 1, int pageSize = 1000);
        Task<Walk?> GetIdAsync(Guid id);
        Task<Walk?> UpdateAsync(Guid id, UpdateWalkRequestDTO request);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
