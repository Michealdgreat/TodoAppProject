using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoLibrary.Models;

namespace TodoLibrary.DataAccess;

public class TodoData : ITodoData
{
    private readonly ISqlDataAccess _sql;

    public TodoData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public Task<List<TodoModel>> GetAllAssigned(int assignedTo)
    {
        return _sql.LoadData<TodoModel, dynamic>("dbo.spTodos_GetAllAssigned", new { AssignedTo = assignedTo }, "Default"); // exec dbo.spTodos_GetAllAssigned @AssignedTo = assignedTo

    }

    public async Task<TodoModel> GetOneAssigned(int assignedTo, int todoId)
    {
        var results = await _sql.LoadData<TodoModel, dynamic>("dbo.spTodos_GetOneAssigned", new { AssignedTo = assignedTo, TodoId = todoId }, "Default"); // exec dbo.spTodos_GetAllAssigned @AssignedTo = assignedTo

        return results.FirstOrDefault();
    }

    public async Task<TodoModel> Create(int assignedTo, int task)
    {
        var results = await _sql.LoadData<TodoModel, dynamic>("dbo.spTodos_Create", new { AssignedTo = assignedTo, TodoId = task }, "Default"); // exec dbo.spTodos_GetAllAssigned @AssignedTo = assignedTo

        return results.FirstOrDefault();
    }

    public Task UpdateTask(int assignedTo, int todoId, string task)
    {
        return _sql.SaveData<dynamic>("dbo.spTodos_UpdateTask", new { AssignedTo = assignedTo, TodoId = todoId, Task = task }, "Default"); // exec dbo.spTodos_GetAllAssigned @AssignedTo = assignedTo

    }

    public Task CompleteTodo(int assignedTo, int todoId)
    {
        return _sql.SaveData<dynamic>("dbo.spTodos_CompleteTodo", new { AssignedTo = assignedTo, TodoId = todoId }, "Default"); // exec dbo.spTodos_GetAllAssigned @AssignedTo = assignedTo

    }

    public Task Delete(int assignedTo, int todoId)
    {
        return _sql.SaveData<dynamic>("dbo.spTodos_Delete", new { AssignedTo = assignedTo, TodoId = todoId }, "Default"); // exec dbo.spTodos_GetAllAssigned @AssignedTo = assignedTo

    }
}
