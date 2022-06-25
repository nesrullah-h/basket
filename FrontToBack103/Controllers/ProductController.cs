using FrontToBack103.DAL;
using FrontToBack103.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FrontToBack103.Controllers
{
    public class ProductController : Controller
    {
        private AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.ProductCount = _context.Products.Count();
            List<Product> products = _context.Products.Include(p => p.Category).Take(2).ToList();
            return View(products);
           
        }
        public IActionResult LoadMore(int skip)
        {

            List<Product> products = _context.Products.Include(p=>p.Category).Skip(skip).Take(2).ToList();
            return PartialView("_ProductPartial",products);
        }

        public IActionResult SearchProduct(string search)
        {
            List<Product> products = _context.Products.Where(p => p.Name.ToLower().Contains(search.ToLower())).Take(10).ToList();
            return PartialView("_Searech",products);
        }
    }
}
