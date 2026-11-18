using backend.Dtos;
using backend.Models;

namespace backend.Services;

public interface IRepairRequestService
{
    Task<RepairRequest> CreateAsync(CreateRepairRequestRequest request);
    Task<List<RepairRequest>> GetAllAsync();
    Task<RepairRequest?> GetByIdAsync(int id);
    Task<RepairRequest?> AssignAsync(int id, int assigneeUserId);
    Task<(RepairRequest? Request, string? Error)> UpdateStatusAsync(int id, string status);
}
