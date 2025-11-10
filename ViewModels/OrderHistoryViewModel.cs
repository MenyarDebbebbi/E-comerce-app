using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication2.ViewModels
{
    public class OrderHistoryViewModel
    {
        public string UserDisplayName { get; init; } = string.Empty;
        public IReadOnlyList<OrderHistoryEntryViewModel> Orders { get; init; } = Array.Empty<OrderHistoryEntryViewModel>();

        public int TotalOrders => Orders.Count;

        public decimal TotalAmount =>
            Orders.Aggregate(0m, (acc, order) => acc + order.TotalAmount);

        public DateTime? LastOrderDate => Orders.FirstOrDefault()?.OrderDate;
    }

    public class OrderHistoryEntryViewModel
    {
        public int OrderId { get; init; }
        public DateTime OrderDate { get; init; }
        public decimal TotalAmount { get; init; }
        public string Address { get; init; } = string.Empty;
        public IReadOnlyList<OrderHistoryItemViewModel> Items { get; init; } = Array.Empty<OrderHistoryItemViewModel>();
    }

    public class OrderHistoryItemViewModel
    {
        public string ProductName { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal SubTotal => UnitPrice * Quantity;
    }
}

