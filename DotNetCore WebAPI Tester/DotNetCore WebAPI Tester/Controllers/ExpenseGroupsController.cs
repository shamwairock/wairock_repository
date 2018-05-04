using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore_WebAPI_Tester.Models;
using DotNetCore_WebAPI_Tester.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using DotNetCore_WebAPI_Tester.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetCore_WebAPI_Tester.Controllers
{
    [Route("WebApi/[controller]")]
    public class ExpenseGroupsController : Controller
    {
        private IExpenseGroupService _expenseGroupService;
        public ExpenseGroupsController(IExpenseGroupService expenseGroupService)
        {
            _expenseGroupService = expenseGroupService;
        }

        [HttpGet]
        public IEnumerable<ExpenseGroup> GetAll()
        {
            return _expenseGroupService.GetAll();
        }

        // GET api/ExpenseGroups/1
        [HttpGet("{id}", Name = "ExpenseGroup")]
        public IActionResult GetById(int id)
        {
            var item = _expenseGroupService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        //[HttpGet("{id}", Name = "ExpenseGroupStatusId")]
        //public IEnumerable<ExpenseGroup> GetExpenseGroupByCategory(int expenseGroupStatusId)
        //{
        //    return _personalExpenseContext.ExpenseGroups.Where(x => x.ExpenseGroupStatusId == expenseGroupStatusId).ToList();
        //}

        ////The [FromBody] attribute tells MVC to get the value of the to-do item from the body of the HTTP request.
        //[HttpPost]
        //public IActionResult Create([FromBody] ExpenseGroup expenseGroup)
        //{
        //    if (expenseGroup == null)
        //    {
        //        return BadRequest();
        //    }

        //    if (_personalExpenseContext.ExpenseGroups.Any(x => x.Id == expenseGroup.Id))
        //    {
        //        throw new ApplicationException("Invalid ExpenseGroup Id");
        //    }

        //    _personalExpenseContext.ExpenseGroups.Add(expenseGroup);
        //    _personalExpenseContext.SaveChanges();

        //    //The CreatedAtRoute method returns a 201 response, 
        //    //which is the standard response for an HTTP POST method that creates a new resource on the server
        //    return CreatedAtRoute("ExpenseGroup", new { id = expenseGroup.Id }, expenseGroup);
        //}

        //[HttpPut("{id}")]
        //public IActionResult Update(int id, [FromBody] ExpenseGroup expenseGroup)
        //{
        //    if (expenseGroup == null || expenseGroup.Id != id)
        //    {
        //        return BadRequest();
        //    }

        //    var existingExpenseGroup = _personalExpenseContext.ExpenseGroups.FirstOrDefault(t => t.Id == id);
        //    if (existingExpenseGroup == null)
        //    {
        //        return NotFound();
        //    }

        //    existingExpenseGroup.Title = expenseGroup.Title;
        //    existingExpenseGroup.UserId = expenseGroup.UserId;
        //    existingExpenseGroup.ExpenseGroupStatusId = expenseGroup.ExpenseGroupStatusId;
        //    existingExpenseGroup.Description = expenseGroup.Description;

        //    _personalExpenseContext.ExpenseGroups.Update(existingExpenseGroup);
        //    _personalExpenseContext.SaveChanges();

        //    return new NoContentResult();
        //}

        //[HttpDelete("id")]
        //public IActionResult Delete(int id)
        //{
        //    var todo = _personalExpenseContext.ExpenseGroups.FirstOrDefault(t => t.Id == id);
        //    if (todo == null)
        //    {
        //        return NotFound();
        //    }

        //    _personalExpenseContext.ExpenseGroups.Remove(todo);
        //    _personalExpenseContext.SaveChanges();

        //    return new NoContentResult();
        //}

    }
}
