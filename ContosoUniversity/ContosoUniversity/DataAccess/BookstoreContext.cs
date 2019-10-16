using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using ContosoBookstore.Models;

namespace ContosoBookstore.DataAccess
{
    public class BookstoreContext : DbContext
    {
        public BookstoreContext() : base("BookstoreContext")
        {
         
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Tender> Tenders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("Book");
            modelBuilder.Entity<Pen>().ToTable("Pen");

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}