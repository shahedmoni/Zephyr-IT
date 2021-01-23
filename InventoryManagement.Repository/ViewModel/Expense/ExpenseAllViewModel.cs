﻿using System;

namespace InventoryManagement.Repository
{
    public class ExpenseAllViewModel
    {
        public int Id { get; set; }
        public int VoucherNo { get; set; }
        public bool IsApproved { get; set; }
        public bool IsTransportation { get; set; }
        public string CreateBy { get; set; }
        public string ExpenseCategory { get; set; }
        public double ExpenseAmount { get; set; }
        public string ExpenseFor { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}