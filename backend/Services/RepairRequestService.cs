using backend.Dtos;
using backend.Models;

namespace backend.Services;

public class RepairRequestService : IRepairRequestService
{
    private readonly List<RepairRequest> _requests = [];
    private int _nextId = 1;
    private int _nextNumber = 1001;

    private static readonly string[] AllowedStatuses =
    [
        "New",
        "InProgress",
        "Closed",
        "Cancelled"
    ];

    public RepairRequest Create(CreateRepairRequestRequest request)
    {
        var repairRequest = new RepairRequest
        {
            Id = _nextId++,
            Number = _nextNumber++,
            Title = request.Title,
            Description = request.Description,
            ClassroomId = request.ClassroomId,
            CreatedByUserId = 5,
            Status = "New",
            AssigneeUserId = null
        };

        _requests.Add(repairRequest);
        return repairRequest;
    }

    public List<RepairRequest> GetAll()
    {
        return _requests;
    }

    public RepairRequest? GetById(int id)
    {
        return _requests.FirstOrDefault(request => request.Id == id);
    }

    public RepairRequest? Assign(int id, int assigneeUserId)
    {
        var request = GetById(id);

        if (request is null)
        {
            return null;
        }

        request.AssigneeUserId = assigneeUserId;
        return request;
    }

    public (RepairRequest? Request, string? Error) UpdateStatus(int id, string status)
    {
        var request = GetById(id);

        if (request is null)
        {
            return (null, null);
        }

        if (!AllowedStatuses.Contains(status))
        {
            return (request, "Недопустимый статус заявки");
        }

        if (status == "InProgress" && request.AssigneeUserId is null)
        {
            return (request, "Нельзя начать выполнение без назначенного исполнителя");
        }

        request.Status = status;
        return (request, null);
    }
}
