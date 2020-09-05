﻿using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public class ProductStockRepository : Repository<ProductStock>, IProductStockRepository
    {
        public ProductStockRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<bool> IsExistAsync(string productStock)
        {
            return Context.ProductStock.AnyAsync(s => s.ProductCode == productStock);
        }

        public Task<List<ProductStockViewModel>> IsExistListAsync(List<ProductStockViewModel> stocks)
        {
            // var codes = stocks.Select(c => c.ProductCode).ToList();
            return Context.ProductStock.Where(s => stocks.Select(c => c.ProductCode).Contains(s.ProductCode)).Select(s => new ProductStockViewModel
            {
                ProductCode = s.ProductCode
            }).ToListAsync();
        }

        public Task<string[]> IsStockOutAsync(string[] codes)
        {
            return Context.ProductStock
                .Where(s => codes.Contains(s.ProductCode) && s.IsSold)
                .Select(s => s.ProductCode)
                .ToArrayAsync();
        }

        public Task<ProductSellViewModel> FindforSellAsync(string code)
        {
            var product = Context.ProductStock
                .Include(s => s.Product)
                .Include(s => s.PurchaseList)
                .Where(s => s.ProductCode == code && !s.IsSold)
                .Select(s => new ProductSellViewModel
                {
                    ProductId = s.ProductId,
                    ProductCatalogId = s.Product.ProductCatalogId,
                    ProductCatalogName = s.Product.ProductCatalog.CatalogName,
                    ProductCode = s.ProductCode,
                    ProductName = s.Product.ProductName,
                    Description = s.PurchaseList.Description,
                    Warranty = s.PurchaseList.Warranty,
                    Note = s.PurchaseList.Note,
                    SellingPrice = s.PurchaseList.SellingPrice,
                    PurchasePrice = s.PurchaseList.PurchasePrice
                });
            return product.FirstOrDefaultAsync();
        }

        public Task<ProductStockDetailsViewModel> FindforDetailsAsync(string code)
        {
            int? sellingListId;
            var product = Context.ProductStock
                .Include(s => s.Product)
                .ThenInclude(p => p.ProductCatalog)
                .Include(s => s.PurchaseList)
                .Include(p => p.SellingList)
                .ThenInclude(sl => sl.Selling)
                .Where(s => s.ProductCode == code)
                .Select(s => new ProductStockDetailsViewModel
                {
                    ProductId = s.ProductId,
                    ProductCode = s.ProductCode,
                    ProductName = s.Product.ProductName,
                    Description = s.PurchaseList.Description,
                    Warranty = s.PurchaseList.Warranty,
                    Note = s.PurchaseList.Note,
                    SellingPrice = s.PurchaseList.SellingPrice,
                    ProductCatalogName = s.Product.ProductCatalog.CatalogName,
                    PurchasePrice = s.PurchaseList.PurchasePrice,
                    SellingId = s.SellingList.SellingId,
                    SellingSn = s.SellingList.Selling.SellingSn,
                    PurchaseId = s.PurchaseList.PurchaseId
                }).FirstOrDefaultAsync();




            return product;
        }

        public Task<List<ProductStock>> SellingStockFromCodesAsync(string[] codes)
        {
            return Context.ProductStock.Include(s => s.Product).Where(s => codes.Contains(s.ProductCode)).ToListAsync();
        }

        public double StockProductPurchaseValue()
        {
            return Context.ProductStock.Include(s => s.PurchaseList)
                       .Where(s => !s.IsSold)?.Sum(s => s.PurchaseList.PurchasePrice) ?? 0;
        }
    }
}