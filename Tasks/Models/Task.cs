using System.ComponentModel.DataAnnotations;
using Tasks.Validation;

namespace Tasks.Models;

public class Task
{
    public int Id { get; set; }

    [Required] [MaxLength(100)] public string Title { get; set; } = string.Empty;

    [Required] [MaxLength(500)] public string Description { get; set; } = string.Empty;

    [Required]
    [FutureDate(ErrorMessage = "Only future dates accepted")]
    [DataType(DataType.Date)]
    [Display(Name = "Due Date")]
    public DateTime DueDate { get; set; }

    [Required] public TaskStatus Status { get; set; }
}