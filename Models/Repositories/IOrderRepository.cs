using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Models.Repositories
{
    public interface IOrderRepository
    {
        Order GetById(int Id);
        void Add(Order o);
        Task<IReadOnlyList<Order>> GetRecentOrdersByUserIdAsync(string userId, int take);
        Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(string userId);
    }
}
