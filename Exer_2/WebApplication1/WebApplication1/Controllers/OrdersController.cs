using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class OrdersController : Controller
    {
        private SE_LabAssignment2Entities2 db = new SE_LabAssignment2Entities2();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Agent);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName");
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order, int[] ItemID, int[] Quantity)
        {
            if (ModelState.IsValid)
            {
                // 1. Lưu thông tin Order chung
                order.OrderDate = DateTime.Now;
                db.Orders.Add(order);
                db.SaveChanges(); // Lưu để lấy ra OrderID tự động tăng

                // 2. Chạy vòng lặp lưu từng món vào OrderDetail
                for (int i = 0; i < ItemID.Length; i++)
                {
                    var detail = new OrderDetail();
                    detail.OrderID = order.OrderID; // Lấy ID vừa tạo ở trên
                    detail.ItemID = ItemID[i];
                    detail.Quantity = Quantity[i];
                    
                    detail.UnitAmount = decimal.Parse(db.Items.Find(ItemID[i]).Size);

                    db.OrderDetails.Add(detail);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", order.AgentID);
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", order.AgentID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,OrderDate,AgentID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", order.AgentID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
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

        public ActionResult Statistics()
        {
            // Lấy dữ liệu và ép về kiểu string ngay tại Controller
            var data = db.OrderDetails
                .GroupBy(d => d.Item.ItemName)
                .Select(g => new {
                    Name = g.Key,
                    Count = g.Sum(s => s.Quantity)
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .AsEnumerable() // Chuyển về bộ nhớ để xử lý string
                .Select(x => x.Name + "|" + x.Count)
                .ToList();

            ViewBag.BestItems = data;
            return View();
        }
    }
}
