﻿using InventoryManagement.Repository;
using JqueryDataTables.LoopsIT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _db;
        public ProductController(IUnitOfWork db)
        {
            _db = db;
        }

        //Add products info
        [Authorize(Roles = "admin, add-product")]
        public IActionResult AddProduct()
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
            return View();
        }

        //POST: Product
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductShowViewModel model)
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");

            if (!ModelState.IsValid) return View(model);

            var isExist = await _db.Products.IsExistAsync(model.ProductName, model.ProductCatalogId).ConfigureAwait(false);

            if (!isExist)
            {
                _db.Products.AddCustom(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("AddProduct");
            }

            ModelState.AddModelError("ProductName", "Product Name Already Exist");
            return View(model);
        }

        //get product info from ajax by productId
        public async Task<IActionResult> GetProductInfo(int productId)
        {
            var productList = await _db.Products.FindByIdAsync(productId);
            return Json(productList);
        }

        //delete product
        public int DeleteProduct(int id)
        {
            if (!_db.Products.RemoveCustom(id)) return -1;
            return _db.SaveChanges();
        }

        //get product from ajax by categoryId
        public async Task<IActionResult> GetProductByCategory(int categoryId = 0)
        {
            var productList = await _db.Products.FindByCategoryAsync(categoryId);
            return Json(productList);
        }

        //GET: Barcode
        [Authorize(Roles = "admin, barcode")]
        public IActionResult Barcode()
        {
            return View();
        }

        //GET: Catalog list
        [Authorize(Roles = "admin, category-list")]
        public IActionResult CatalogList()
        {
            var model = _db.ProductCatalogs.ListCustom();
            return View(model);
        }

        //GET: Catalog
        [Authorize(Roles = "admin, category-list")]
        public IActionResult Catalog()
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
            return View();
        }

        //POST: Catalog
        [Authorize(Roles = "admin, category-list")]
        [HttpPost]
        public async Task<IActionResult> Catalog(ProductCatalogViewModel model)
        {
            ViewBag.ParentId = new SelectList(_db.ProductCatalogs.CatalogDll(), "value", "label");
            if (!ModelState.IsValid) return View(model);

            var response = await _db.ProductCatalogs.AddCustomAsync(model).ConfigureAwait(false);

            if (response.IsSuccess)
                return RedirectToAction("CatalogList");

            ModelState.AddModelError("CatalogName", response.Message);
            return View(model);
        }

        //GET: catalog Type
        public async Task<IActionResult> CatalogType()
        {
            var response = await _db.ProductCatalogTypes.ToListAsync();
            return Json(response);
        }

        //POST: catalog type
        [HttpPost]
        public async Task<IActionResult> CatalogType([FromBody] ProductCatalogTypeViewModel model)
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            var response = await _db.ProductCatalogTypes.AddCustomAsync(model).ConfigureAwait(false);

            if (response.IsSuccess)
                return Ok(response.Data);
            else
                return UnprocessableEntity(response.Message);
        }
    }
}