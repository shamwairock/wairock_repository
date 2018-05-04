using System;

namespace DotNetCore_WebAPI_Tester.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public int ExpenseGroupId { get; set; }
    }
}