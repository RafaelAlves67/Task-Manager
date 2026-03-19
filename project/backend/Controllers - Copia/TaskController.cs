using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.DTOs;
using TaskManager.Api.Models;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/task")]
[Produces("application/json")]
public class TaskController : ControllerBase
{
    private readonly AppDbContext _db;

    public TaskController(AppDbContext db) => _db = db;

    // GET /api/task
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _db.Tasks
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => MapToDto(t))
            .ToListAsync();

        return Ok(tasks);
    }

    // GET /api/task/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null) return NotFound(new { message = $"Tarefa {id} não encontrada." });
        return Ok(MapToDto(task));
    }

    // POST /api/task
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var task = new TaskItem
        {
            Title       = dto.Title,
            Description = dto.Description,
            Priority    = dto.Priority,
            Status      = dto.Status,
            CreatedAt   = DateTimeOffset.UtcNow,
            UpdatedAt   = DateTimeOffset.UtcNow
        };
        
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, MapToDto(task));
    }

    // PUT /api/task/{id}
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var task = await _db.Tasks.FindAsync(id);
        if (task is null) return NotFound(new { message = $"Tarefa {id} não encontrada." });

        task.Title       = dto.Title;
        task.Description = dto.Description;
        task.Priority    = dto.Priority;
        task.Status      = dto.Status;
        task.UpdatedAt   = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(MapToDto(task));
    }

    // DELETE /api/task/{id}
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null) return NotFound(new { message = $"Tarefa {id} não encontrada." });

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ── Mapper ──────────────────────────────────────────────
    private static TaskResponseDto MapToDto(TaskItem t) => new()
    {
        Id          = t.Id,
        Title       = t.Title,
        Description = t.Description,
        Priority    = t.Priority.ToString(),
        Status      = t.Status.ToString(),
        CreatedAt   = t.CreatedAt,
        UpdatedAt   = t.UpdatedAt
    };
}
