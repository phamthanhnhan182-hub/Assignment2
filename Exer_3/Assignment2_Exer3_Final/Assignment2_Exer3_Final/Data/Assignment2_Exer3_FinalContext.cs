using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Assignment2_Exer3_Final.Models;

namespace Assignment2_Exer3_Final.Data
{
    public class Assignment2_Exer3_FinalContext : DbContext
    {
        public Assignment2_Exer3_FinalContext (DbContextOptions<Assignment2_Exer3_FinalContext> options)
            : base(options)
        {
        }

        public DbSet<Assignment2_Exer3_Final.Models.Item> Item { get; set; } = default!;
        public DbSet<Assignment2_Exer3_Final.Models.Agent> Agent { get; set; } = default!;
        public DbSet<Assignment2_Exer3_Final.Models.Product> Product { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;
    }
}
