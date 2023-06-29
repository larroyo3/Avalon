using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Avalon_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Avalon_API.DAL
{
    public class TodoItemRepository : ITodoRepository, IDisposable
    {
        private TodoContext context;

        public TodoItemRepository(TodoContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TodoItemDTO>> GetTodoItemsAsync()
        {
            return await context.TodoItems.Select(x => ItemToDTO(x)).ToListAsync();
        }

        [ErrorHandlingAspect]
        public async Task<TodoItem> GetTodoItemByIDAsync(long id)
        {
            var item = await context.TodoItems.FindAsync(id);
            if (item == null)
            {
                throw new Exception("Item is null");
            }

            return item;
        }

        public void InsertTodoItem(TodoItem item)
        {
            context.TodoItems.Add(item);
        }

        public void DeleteTodoItem(TodoItem item)
        {
            context.TodoItems.Remove(item);
        }

        public void UpdateTodoItem(TodoItem item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        new TodoItemDTO
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };

        public bool TodoItemExists(long id)
        {
            return context.TodoItems.Any(e => e.Id == id);
        }
    }
}