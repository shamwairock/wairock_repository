using DotNetCore_WebAPI_Tester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_WebAPI_Tester.Interfaces
{
    public interface IExpenseGroupService
    {
        IEnumerable<ExpenseGroup> GetAll();
        ExpenseGroup GetById(int id);
        void Update(ExpenseGroup expenseGroup);
        void Delete(int id);
    }
}
