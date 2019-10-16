using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ContosoBookstore.Models;

namespace ContosoBookstore.DataAccess
{
    public class BookstoreInitializer : DropCreateDatabaseIfModelChanges<BookstoreContext>
    {
        protected override void Seed(BookstoreContext context)
        {
            var itemCategories = new List<ItemCategory>()
            {
                new ItemCategory() {ItemCategoryID = Guid.NewGuid(), Name = "Pen"},
                new ItemCategory() {ItemCategoryID = Guid.NewGuid(), Name = "Book"},
            };

            itemCategories.ForEach(itemCategory => context.ItemCategories.AddOrUpdate(itemCategory));
        }
    }
}