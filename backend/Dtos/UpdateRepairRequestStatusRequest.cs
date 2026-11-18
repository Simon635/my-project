using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public class UpdateRepairRequestStatusRequest
{
    [Required]
    [RegularExpression("^(New|InProgress|Closed|Cancelled)$",
        ErrorMessage = "Допустимые статусы: New, InProgress, Closed, Cancelled")]
    public string Status { get; set; } = string.Empty;
}
