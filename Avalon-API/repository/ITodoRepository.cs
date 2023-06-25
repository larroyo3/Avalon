using System;
using System.Collections.Generic;
using Avalon_API.Models;

namespace Avalon_API.DAL
{
    public interface ITodoRepository : IDisposable
    {
        Task<IEnumerable<TodoItemDTO>> GetTodoItemsAsync();
        Task<TodoItem> GetTodoItemByIDAsync(long id);
        void InsertTodoItem(TodoItem item);
        void DeleteTodoItem(TodoItem item);
        void UpdateTodoItem(TodoItem item);
        Task SaveAsync();
        bool TodoItemExists(long id);
    }
}