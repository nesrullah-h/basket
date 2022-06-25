﻿using FrontToBack103.DAL;
using FrontToBack103.Models;
using FrontToBack103.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack103.Controllers
{
    public class BasketController : Controller
    {
        private AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.FindAsync(id);

            if (dbProduct == null) return NotFound();
            List<BasketProduct> products;

            string existBasket = Request.Cookies["basket"];

            if (existBasket == null)
            {
                products = new List<BasketProduct>();

            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketProduct>>(Request.Cookies["basket"]);

            }
            BasketProduct existBasketproduct = products.FirstOrDefault(p => p.Id == dbProduct.Id);
            if (existBasketproduct == null)
            {
                BasketProduct basketProduct = new BasketProduct();

                basketProduct.Id = dbProduct.Id;
                basketProduct.Name = dbProduct.Name;
                basketProduct.Count = 1;

                products.Add(basketProduct);
            }
            else
            {
                if (dbProduct.Count <= existBasketproduct.Count)
                {
                    TempData["Fail"] = "not enough count";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    existBasketproduct.Count++;
                }
            }



            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromMinutes(30) });

            return RedirectToAction("Index", "Home");

        }
        public IActionResult Basket()
        {
            List<BasketProduct> products = JsonConvert.DeserializeObject<List<BasketProduct>>(Request.Cookies["basket"]);
            List<BasketProduct> updatesproducts = new List<BasketProduct>();
            foreach (var item in products)
            {
                Product dbproduct = _context.Products.FirstOrDefault(p => p.Id == item.Id);
                BasketProduct basket = new BasketProduct()
                {
                    Id = dbproduct.Id,
                    Price = dbproduct.Price,
                    Name = dbproduct.Name,
                    ImageUrl = dbproduct.ImageUrl,
                    Count = item.Count
                };

                updatesproducts.Add(basket);
            }
            return View(updatesproducts);
        }

        public IActionResult RemoveItem(int? id)
        {
            if (id == null) return NotFound();
            string basket = Request.Cookies["basket"];
            List<BasketProduct> products = JsonConvert.DeserializeObject<List<BasketProduct>>(basket);
            BasketProduct existProduct = products.FirstOrDefault(p => p.Id == id);
            if (existProduct==null) return NotFound();
            products.Remove(existProduct);
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions
            {
                MaxAge=TimeSpan.FromMinutes(20)
            });
            return RedirectToAction(nameof(basket));

        }
        public IActionResult Plus(int? id)
        {
            if (id == null) return NotFound();
            string basket = Request.Cookies["basket"];
            List<BasketProduct> products = JsonConvert.DeserializeObject<List<BasketProduct>>(basket);
            BasketProduct existProduct = products.FirstOrDefault(p => p.Id == id);
            if (existProduct == null) return NotFound();

            Product dbProduct = _context.Products.FirstOrDefault(p => p.Id == id);
            if (dbProduct.Count > existProduct.Count)
            {

                existProduct.Count++;
            }
            else
            {
                TempData["Fail"] = "not enough count";
                return RedirectToAction("Basket", "Basket");
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(20)
            });
            return RedirectToAction(nameof(basket));

        }
        public IActionResult Minus(int? id)
        {
            if (id == null) return NotFound();
            string basket = Request.Cookies["basket"];
            List<BasketProduct> products = JsonConvert.DeserializeObject<List<BasketProduct>>(basket);
            BasketProduct existProduct = products.FirstOrDefault(p => p.Id == id);
            if (existProduct == null) return NotFound();
            if (existProduct.Count > 1)
            {
                existProduct.Count--;
            }
            else
            {
                RemoveItem(existProduct.Id);
                return RedirectToAction(nameof(basket));
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(20)
            });
            return RedirectToAction(nameof(basket));

        }
    }
}