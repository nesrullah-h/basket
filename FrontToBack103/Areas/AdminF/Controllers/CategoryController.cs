using FrontToBack103.DAL;
using FrontToBack103.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack103.Areas.AdminF.Controllers
{
    [Area("AdminF")]
    public class CategoryController : Controller
    {
        private AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id==null) return NotFound();
            Category dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory==null) return NotFound();
            return View(dbCategory);
        }

     

        public async Task<IActionResult> Delete(int? id)
        {
            if (id==null) return NotFound();
            Category dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory==null) return NotFound();
            _context.Categories.Remove(dbCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool isExistName= _context.Categories.Any(c => c.Name.ToLower()==category.Name.ToLower());
            if (isExistName)
            {
                ModelState.AddModelError("Name", "eyni adli category movcuddur");
                return View();
            }
            Category newCategory = new Category();
            newCategory.Name=category.Name;
            newCategory.Desc=category.Desc;
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Category dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory == null) return NotFound();
            return View(dbCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Category dbCategory = await _context.Categories.FindAsync(id);

            Category existNameCategory = _context.Categories.FirstOrDefault(c=>c.Name.ToLower()==category.Name.ToLower());

            if (existNameCategory!=null)
            {
                if (dbCategory!=existNameCategory)
                {
                    ModelState.AddModelError("Name", "name already exist");
                    return View();
                }
            }

            if (dbCategory == null) return NotFound();
            dbCategory.Name=category.Name;
            dbCategory.Desc=category.Desc;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
