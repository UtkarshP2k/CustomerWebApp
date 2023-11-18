using Customer_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                Id = 1,
                Name = "Utkarsh",
                Address = "Mumbai",
                Phone = 9920023455
            },
            new Customer
            {
                Id = 2,
                Name = "Mansi",
                Address = "Pune",
                Phone = 9922345555
            },
            new Customer
            {
                Id = 3,
                Name = "Suresh",
                Address = "Mumbai",
                Phone = 9912023455
            },
            new Customer
            {
                Id = 4,
                Name = "Gauresh",
                Address = "Noida",
                Phone = 9930023455
            }

            );
        }
    }
}
