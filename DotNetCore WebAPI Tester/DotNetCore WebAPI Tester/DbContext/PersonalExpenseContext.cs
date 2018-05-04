using DotNetCore_WebAPI_Tester.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_WebAPI_Tester.DatabaseContext
{
    public class PersonalExpenseContext : DbContext
    {
        public PersonalExpenseContext():base()
        {
        }

        public PersonalExpenseContext(DbContextOptions<PersonalExpenseContext> options)
             : base(options)
        {

        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseGroup> ExpenseGroups { get; set; }
        public DbSet<ExpenseGroupStatus> ExpenseGroupStatuses { get; set; }
    }
}
