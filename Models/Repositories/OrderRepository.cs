using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Models.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext context;
        public OrderRepository(AppDbContext context)
        {
            this.context = context;
        }
        public void Add(Order o)
        {
            context.Orders.Add(o);
            context.SaveChanges();
        }
        public Order GetById(int id)
        {
            return context.Orders
            .Include(o => o.Items)
            .FirstOrDefault(o => o.Id == id);
        }

        public async Task<IReadOnlyList<Order>> GetRecentOrdersByUserIdAsync(string userId, int take)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Array.Empty<Order>();
            }

            return await context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .Take(take)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Array.Empty<Order>();
            }

            return await context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
