
using backend.Dtos;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/repair-requests")]
public class RepairRequestsController : ControllerBase
{
    private readonly IRepairRequestService _service;
    private readonly ILogger<RepairRequestsController> _logger;

    public RepairRequestsController(
        IRepairRequestService service,
        ILogger<RepairRequestsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    public ActionResult<RepairRequestResponse> Create(
        CreateRepairRequestRequest request)
    {
        try
        {
            var repairRequest = _service.Create(request);
            var response = MapToResponse(repairRequest);

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                response);
        }
        catch (Exception ex)
        {
            _logger.LogError(
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

    [HttpGet]
    public ActionResult<List<RepairRequestResponse>> GetAll()
    {
        var requests = _service.GetAll()
            .Select(MapToResponse)
            .ToList();

        return Ok(requests);
    }

    [HttpGet("{id:int}")]
    public ActionResult<RepairRequestResponse> GetById(int id)
    {
        var repairRequest = _service.GetById(id);

        if (repairRequest is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(repairRequest));
    }

    [HttpPatch("{id:int}/assignee")]
    public ActionResult<RepairRequestResponse> Assign(
        int id,
        AssignRepairRequestRequest request)
    {
        var repairRequest = _service.Assign(id, request.AssigneeUserId);

        if (repairRequest is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(repairRequest));
    }

    [HttpPatch("{id:int}/status")]
    public ActionResult<RepairRequestResponse> UpdateStatus(
        int id,
        UpdateRepairRequestStatusRequest request)
    {
        var result = _service.UpdateStatus(id, request.Status);

        if (result.Request is null)
        {
            return NotFound();
        }

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

    private static RepairRequestResponse MapToResponse(
        RepairRequest request)
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

