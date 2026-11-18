using backend.Dtos;
using backend.Models;

namespace backend.Services;

public interface IRepairRequestService
{
    RepairRequest Create(CreateRepairRequestRequest request);
    List<RepairRequest> GetAll();
    RepairRequest? GetById(int id);
    RepairRequest? Assign(int id, int assigneeUserId);
    (RepairRequest? Request, string? Error) UpdateStatus(int id, string status);
}
