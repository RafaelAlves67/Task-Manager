using System.ComponentModel.DataAnnotations;
using TaskManager.Api.Models;

namespace TaskManager.Api.DTOs;

public class CreateTaskDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.normal;

    public ItemStatus Status { get; set; } = ItemStatus.nao_iniciado;
}

public class UpdateTaskDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.normal;

    public ItemStatus Status { get; set; } = ItemStatus.nao_iniciado;
}

public class TaskResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
