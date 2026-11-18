namespace backend.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public ICollection<RepairRequest> CreatedRequests { get; set; } = [];
    public ICollection<RepairRequest> AssignedRequests { get; set; } = [];
}
