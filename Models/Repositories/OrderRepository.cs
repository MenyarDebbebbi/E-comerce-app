﻿using Microsoft.EntityFrameworkCore;

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
    }
}
