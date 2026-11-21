using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class RepairRequestService(AppDbContext db) : IRepairRequestService
{
    private static readonly string[] AllowedStatuses =
        ["New", "InProgress", "Closed", "Cancelled"];

    public async Task<RepairRequest> CreateAsync(CreateRepairRequestRequest request, int createdByUserId)
    {
        var lastNumber = await db.RepairRequests
            .MaxAsync(x => (int?)x.Number) ?? 1000;

        var repairRequest = new RepairRequest
        {
            Number = lastNumber + 1,
            Title = request.Title,
            Description = request.Description,
            ClassroomId = request.ClassroomId,
            CreatedByUserId = createdByUserId,
            Status = "New",
            CreatedAt = DateTimeOffset.UtcNow
        };

        db.RepairRequests.Add(repairRequest);
        await db.SaveChangesAsync();

        return repairRequest;
    }

    public async Task<List<RepairRequest>> GetAllAsync()
    {
        return await db.RepairRequests
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<RepairRequest?> GetByIdAsync(int id)
    {
        return await db.RepairRequests
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<RepairRequest?> AssignAsync(int id, int assigneeUserId)
    {
        var request = await GetByIdAsync(id);

        if (request is null)
            return null;

        var userExists = await db.Users
            .AnyAsync(x => x.Id == assigneeUserId);

        if (!userExists)
            throw new InvalidOperationException("Исполнитель не найден.");

        request.AssigneeUserId = assigneeUserId;
        await db.SaveChangesAsync();

        return request;
    }

    public async Task<(RepairRequest? Request, string? Error)> UpdateStatusAsync(
        int id, string status)
    {
        var request = await GetByIdAsync(id);

        if (request is null)
            return (null, null);

        if (!AllowedStatuses.Contains(status))
            return (request, "Недопустимый статус заявки");

        if (status == "InProgress" && request.AssigneeUserId is null)
            return (request, "Нельзя начать выполнение без назначенного исполнителя");

        request.Status = status;
        await db.SaveChangesAsync();

        return (request, null);
    }
}
