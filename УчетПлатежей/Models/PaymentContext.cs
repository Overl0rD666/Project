using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace УчетПлатежей.Models
{
    public class PaymentContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Position> Positions { get; set; }

        public PaymentContext(DbContextOptions<PaymentContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
