using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CampusService.Controllers;

[ApiController]
[Route("api/v1/sports/bookings")]
public class SportsBookingsController : ControllerBase
{
    private static readonly ActivitySource ActivitySource =
        new("CampusService.SportsComplex");

    private readonly ILogger<SportsBookingsController> _logger;

    public SportsBookingsController(ILogger<SportsBookingsController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult CreateBooking([FromBody] CreateSportsBookingDto request)
    {
        using var activity = ActivitySource.StartActivity("CreateSportsBooking");

        activity?.SetTag("jira.issue", "CAMPUS-110");
        activity?.SetTag("campus.entity_id", request.StudentId);
        activity?.SetTag("campus.facility_id", request.FacilityId);
        activity?.SetTag("campus.time_slot", request.TimeSlot);

        var bookingId = Guid.NewGuid();

        activity?.SetTag("campus.booking_id", bookingId);
        activity?.SetTag("campus.booking_result", "Created");

        _logger.LogInformation("Booking created. BookingId={BookingId}, TraceId={TraceId}",
            bookingId, Activity.Current?.TraceId);

        return Ok(new BookingResultDto(bookingId));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult CancelBooking(Guid id)
    {
        using var activity = ActivitySource.StartActivity("CancelSportsBooking");

        activity?.SetTag("jira.issue", "CAMPUS-110");
        activity?.SetTag("campus.entity_id", id);
        activity?.SetTag("campus.booking_result", "Cancelled");

        _logger.LogInformation("Booking cancelled. BookingId={BookingId}, TraceId={TraceId}",
            id, Activity.Current?.TraceId);

        return NoContent();
    }
}

public record CreateSportsBookingDto(
    Guid StudentId,
    string FacilityId,
    string TimeSlot);

public record BookingResultDto(Guid BookingId);
