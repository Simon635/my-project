using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public class CreateRepairRequestRequest
{
    [Required]
    [StringLength(80, MinimumLength = 5)]
    public string Title { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int ClassroomId { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;
}
