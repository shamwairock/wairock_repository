using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoBookstore.DataAccess;
using ContosoBookstore.Models;

namespace ContosoBookstore.Controllers
{
    public class ItemController<T> : Controller
    {
        protected BookstoreContext db = new BookstoreContext();

        // GET: Item
        public async Task<ActionResult> Index()
        {
            var items = db.Items.Include(i => i.ItemCategory);
            return View(await items.ToListAsync());
        }

        // GET: Item/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Item/Create
        public virtual ActionResult Create()
        {
            ViewBag.ItemCategoryID = new SelectList(db.ItemCategories, "ItemCategoryID", "Name");
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create([Bind(Include = "ItemID,Name,DisplayName,Price,ItemCategoryID")] T item)
        {
            //if (ModelState.IsValid)
            //{
            //    item.ItemID = Guid.NewGuid();
            //    db.Items.Add(item);
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.ItemCategoryID = new SelectList(db.ItemCategories, "ItemCategoryID", "Name", item.ItemCategoryID);
            return View(item);
        }

        // GET: Item/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemCategoryID = new SelectList(db.ItemCategories, "ItemCategoryID", "Name", item.ItemCategoryID);
            return View(item);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ItemID,Name,DisplayName,Price,ItemCategoryID")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ItemCategoryID = new SelectList(db.ItemCategories, "ItemCategoryID", "Name", item.ItemCategoryID);
            return View(item);
        }

        // GET: Item/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Item item = await db.Items.FindAsync(id);
            db.Items.Remove(item);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
