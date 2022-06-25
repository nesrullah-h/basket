using FrontToBack103.DAL;
using FrontToBack103.Extentions;
using FrontToBack103.Helpers;
using FrontToBack103.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack103.Areas.AdminF.Controllers
{
    [Area("AdminF")]
    public class SliderController : Controller
    {

        private AppDbContext _context;
        private IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();
            return View(sliders);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (ModelState["Photo"].ValidationState==Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {

                return View();
            }
            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "accept only image");
                return View(); 
            }
            if (slider.Photo.ImageSize(50000))
            {
                ModelState.AddModelError("Photo", "1mb-dan yuxari ola bilmez");

                return View();
            }

            // string path = @"C:\Users\User\Desktop\FileUpload\FrontToBack_fiorello-FileUpload\FrontToBack103\wwwroot\img\slider\";

            string fileName = await slider.Photo.SaveImage(_env, "img");
            Slider newSlider = new Slider();
            newSlider.ImageUrl=await slider.Photo.SaveImage(_env, "img");;
            await _context.Sliders.AddAsync(newSlider);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Delete (int? id)
        {
            if (id == null) return NotFound();
            Slider dbSlider = await _context.Sliders.FindAsync(id);
            if (dbSlider==null) return NotFound();
            Helper.DeleteFile(_env,"img",dbSlider.ImageUrl);

            _context.Sliders.Remove(dbSlider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
