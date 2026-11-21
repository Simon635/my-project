using System.Security.Claims;
using backend.Dtos;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/repair-requests")]
public class RepairRequestsController(
    IRepairRequestService service,
    ILogger<RepairRequestsController> logger) : ControllerBase
{
    [Authorize(Roles = "Dispatcher")]
    [HttpPost]
    public async Task<ActionResult<RepairRequestResponse>> Create(
        CreateRepairRequestRequest request)
    {
        try
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var repairRequest = await service.CreateAsync(request, currentUserId);
            var response = MapToResponse(repairRequest);

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                response);
        }
        catch (Exception ex)
        {
            logger.LogError(
                "CreateRepairRequest failed; classroomId={ClassroomId}; error={ErrorType}",
                request.ClassroomId,
                ex.GetType().Name);

            return StatusCode(500, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Внутренняя ошибка сервера",
                Detail = "Не удалось создать заявку."
            });
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<RepairRequestResponse>>> GetAll()
    {
        var requests = await service.GetAllAsync();
        return Ok(requests.Select(MapToResponse).ToList());
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RepairRequestResponse>> GetById(int id)
    {
        var repairRequest = await service.GetByIdAsync(id);

        if (repairRequest is null)
            return NotFound();

        return Ok(MapToResponse(repairRequest));
    }

    [Authorize(Roles = "Dispatcher")]
    [HttpPatch("{id:int}/assignee")]
    public async Task<ActionResult<RepairRequestResponse>> Assign(
        int id, AssignRepairRequestRequest request)
    {
        var repairRequest = await service.AssignAsync(id, request.AssigneeUserId);

        if (repairRequest is null)
            return NotFound();

        return Ok(MapToResponse(repairRequest));
    }

    [Authorize(Roles = "Technician")]
    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<RepairRequestResponse>> UpdateStatus(
        int id, UpdateRepairRequestStatusRequest request)
    {
        var repairRequestForAccess = await service.GetByIdAsync(id);
        if (repairRequestForAccess is null)
            return NotFound();

        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (repairRequestForAccess.AssigneeUserId != currentUserId)
            return Forbid();

        var result = await service.UpdateStatusAsync(id, request.Status);

        if (result.Request is null)
            return NotFound();

        if (result.Error is not null)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Невозможно изменить статус",
                Detail = result.Error
            });
        }

        return Ok(MapToResponse(result.Request));
    }

    private static RepairRequestResponse MapToResponse(RepairRequest request)
    {
        return new RepairRequestResponse
        {
            Id = request.Id,
            Number = request.Number,
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            ClassroomId = request.ClassroomId,
            CreatedByUserId = request.CreatedByUserId,
            AssigneeUserId = request.AssigneeUserId
        };
    }
}
