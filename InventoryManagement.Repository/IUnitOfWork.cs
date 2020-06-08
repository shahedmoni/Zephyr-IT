﻿using System;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IPageLinkRepository PageLinks { get; }
        IPageLinkCategoryRepository PageLinkCategories { get; }
        IPageLinkAssignRepository PageLinkAssigns { get; }
        IProductCatalogRepository ProductCatalogs { get; }
        IProductCatalogTypeRepository ProductCatalogTypes { get; }
        IProductStockRepository ProductStocks { get; }
        IPurchaseRepository Purchases { get; }
        IPurchasePaymentRepository PurchasePayments { get; }
        IRegistrationRepository Registrations { get; }
        IExpenseCategoryRepository ExpenseCategories { get; }
        IExpenseRepository Expenses { get; }
        IInstitutionRepository Institutions { get; }
        IVendorRepository Vendors { get; }
        ISellingRepository Selling { get; }
        ISellingPaymentRepository SellingPayments { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
