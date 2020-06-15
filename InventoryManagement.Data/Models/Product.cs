﻿using System;
using System.Collections.Generic;

namespace InventoryManagement.Data
{
    public partial class Product
    {
        public Product()
        {
            ProductStock = new HashSet<ProductStock>();
            SellingAdjustment = new HashSet<SellingAdjustment>();
            PurchaseList = new HashSet<PurchaseList>();
        }

        public int ProductId { get; set; }
        public int ProductCatalogId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Warranty { get; set; }
        public double SellingPrice { get; set; }
        public DateTime InsertDate { get; set; }
        public virtual ProductCatalog ProductCatalog { get; set; }
        public virtual ICollection<ProductStock> ProductStock { get; set; }
        public virtual ICollection<SellingAdjustment> SellingAdjustment { get; set; }
        public virtual ICollection<PurchaseList> PurchaseList { get; set; }
    }
}
