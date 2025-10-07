using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymAngel.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, Failed
        public string PaymentMethod { get; set; } = "VNPay";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User User { get; set; } = null!;
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}

