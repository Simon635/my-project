namespace backend.Models;

public class RepairRequest
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "New";
    public int ClassroomId { get; set; }
    public int CreatedByUserId { get; set; }
    public int? AssigneeUserId { get; set; }
}
