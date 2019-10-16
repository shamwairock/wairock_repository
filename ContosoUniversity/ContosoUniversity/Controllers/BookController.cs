using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ContosoBookstore.Models;

namespace ContosoBookstore.Controllers
{
    public class BookController : ItemController<Book>
    {
        public override async Task<ActionResult> Create([Bind(Include = "ItemID,Name,DisplayName,Price,ItemCategoryID,Author")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.ItemID = Guid.NewGuid();
                db.Items.Add(book);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ItemCategoryID = new SelectList(db.ItemCategories, "ItemCategoryID", "Name", book.ItemCategoryID);
            return View(book);
        }

        //public override Task<ActionResult> Create([Bind(Include = "ItemID,Name,DisplayName,Price,ItemCategoryID")] Book book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        book.ItemID = Guid.NewGuid();
        //        db.Items.Add(book);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ItemCategoryID = new SelectList(db.ItemCategories, "ItemCategoryID", "Name", item.ItemCategoryID);
        //    return View(item);
        //}
    }
}