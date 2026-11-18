using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public class AssignRepairRequestRequest
{
    [Range(1, int.MaxValue)]
    public int AssigneeUserId { get; set; }
}
