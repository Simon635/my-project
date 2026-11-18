namespace backend.Models;

public class Classroom
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<RepairRequest> RepairRequests { get; set; } = [];
}
