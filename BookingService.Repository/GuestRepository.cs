using BookingService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Repository
{
    public interface IGuestRepository
    {
        Task AddAsync(Guest entity);
        Task<IEnumerable<Guest>> GetByIdAsync(Guid guestId);
        Task<IEnumerable<Guest>> GetByEmailAsync(string email);
    }
    public class GuestRepository : RepositoryBase<Guest>, IGuestRepository
    {
        public GuestRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task AddAsync(Guest entity)
        {
            await Add(entity);
        }

        public async Task<IEnumerable<Guest>> GetByIdAsync(Guid guestId)
        {
            return await Find<Guest>(h => h.Id.Equals(guestId));
        }

        public async Task<IEnumerable<Guest>> GetByEmailAsync(string email)
        {
            return await Find<Guest>(h => h.Email.Equals(email));
        }
    }
}
