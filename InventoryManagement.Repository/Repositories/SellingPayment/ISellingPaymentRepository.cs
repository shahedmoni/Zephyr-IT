﻿using InventoryManagement.Data;
using JqueryDataTables.LoopsIT;
using System;
using System.Threading.Tasks;

namespace InventoryManagement.Repository
{
    public interface ISellingPaymentRepository : IRepository<SellingPayment>
    {
        Task<int> GetNewSnAsync();
        Task<DbResponse> DuePaySingleAsync(SellingDuePaySingleModel model, IUnitOfWork db);
        Task<DbResponse<int>> DuePayMultipleAsync(SellingDuePayMultipleModel model, IUnitOfWork db);
        double DailyCashCollectionAmount(DateTime? date);
        DataResult<SellingPaymentRecordModel> Records(DataRequest request);
        double CollectionAmountDateWise(DateTime? sDateTime, DateTime? eDateTime);

    }
}