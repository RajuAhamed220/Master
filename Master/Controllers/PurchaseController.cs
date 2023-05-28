using System;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MasterDetails.Models;

namespace MasterDetails.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly AppDbContext _context;

        public PurchaseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Purchase
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Purchase.Include(p => p.Supplier);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Purchase/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Purchase == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchase/Create
        public IActionResult Create()
        {
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "SupplierName");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName");
            Purchase purchase = new Purchase();
            purchase.PurchaseProducts.Add(new PurchaseProduct { Id = 1 });
            return View(purchase);
        }

        // POST: Purchase/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PurchaseDate,PurchaseCode,PurchaseType,SupplierId,TotalAmount,DiscountPercentage,DiscountAmount,VatPercentage,VatAmount,PaymentAmount")] Purchase purchase)
        {
            if (ModelState.IsValid && purchase.SupplierId == 0)
            {
                ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "SupplierName");
                ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName");
                return View(purchase);

            }
            purchase.PurchaseProducts.RemoveAll(p => p.Quantity == 0);
            _context.Add(purchase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        // GET: Purchase/Edit/5
        public IActionResult Edit(int? id)
        {


            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "SupplierName");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName");

            Purchase purchase = _context.Purchase.Where(x => x.Id == id)
                .Include(i => i.PurchaseProducts)
                .ThenInclude(i => i.Product)
                .FirstOrDefault();

            purchase.PurchaseProducts.ForEach(x => x.Amount = x.Quantity * x.PurchasePrice);

            return View(purchase);
        }

        // POST: Purchase/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PurchaseDate,PurchaseCode,PurchaseType,SupplierId,TotalAmount,DiscountPercentage,DiscountAmount,VatPercentage,VatAmount,PaymentAmount")] Purchase purchase)
        {
            purchase.PurchaseProducts.RemoveAll(p => p.Quantity == 0);

            try
            {
                List<PurchaseProduct> purchaseItems = _context.PurchaseProducts.Where(p => p.Id == id).ToList();
                _context.PurchaseProducts.RemoveRange(purchaseItems);
                await _context.SaveChangesAsync();
                _context.Attach(purchase);
                _context.Entry(purchase).State = EntityState.Modified;
                _context.PurchaseProducts.AddRange(purchase.PurchaseProducts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Purchase/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "SupplierName");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "ProductName");
            Purchase purchase = _context.Purchase.Where(p => p.Id == id)
                .Include(p => p.PurchaseProducts)
                .ThenInclude(p => p.Product)
                .FirstOrDefault();

            purchase.PurchaseProducts.ForEach(p => p.Amount = p.Quantity * p.PurchasePrice);
            return View(purchase);
        }

        // POST: Purchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Purchase purchase)
        {
            try
            {
                List<PurchaseProduct> purchaseItems = _context.PurchaseProducts.Where(i => i.PurchaseId == purchase.Id).ToList();
                _context.PurchaseProducts.RemoveRange(purchaseItems);
                await _context.SaveChangesAsync();
                _context.Remove(purchase);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private bool PurchaseExists(int id)
        {
            return _context.Purchase.Any(e => e.Id == id);
        }

    }
}
