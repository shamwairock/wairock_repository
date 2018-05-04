using DotNetCore_WebAPI_Tester.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore_WebAPI_Tester.Models;
using DotNetCore_WebAPI_Tester.DatabaseContext;

namespace DotNetCore_WebAPI_Tester.DataAccessServices
{
    public class ExpenseGroupService : IExpenseGroupService
    {
        private PersonalExpenseContext _personalExpenseContext;
        public ExpenseGroupService(PersonalExpenseContext personalExpenseContext)
        {
            _personalExpenseContext = personalExpenseContext;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExpenseGroup> GetAll()
        {
            return _personalExpenseContext.ExpenseGroups.ToList();
        }

        public ExpenseGroup GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ExpenseGroup expenseGroup)
        {
            throw new NotImplementedException();
        }
    }
}
