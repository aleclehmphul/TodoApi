using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private TodoContext _context;

        public TodoItemsController(TodoContext context) 
        { 
            _context = context;
        }

        // GET ALL
        [HttpGet]
        public async Task<ActionResult<List<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }


        // GET {id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var item = await _context.TodoItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }


        // POST
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }


        // PUT
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItem>> PutTodoItem(int id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoItem(int id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null) { return NotFound(); }

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(int id)
        {
            return _context.TodoItems.Any(item => item.Id == id);
        }
    }
}


