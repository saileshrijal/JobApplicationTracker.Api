using JobApplicationTracke.Data.Dto;
using JobApplicationTracke.Data.Interface;
using JobApplicationTracker.Api.Data.Service;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers.Notifications;

[Route("api/notificationTypes")]
public class NotificationTypesController(INotificationsTypesRepository notificationTypesService) : ControllerBase
{
    [HttpGet]
    [Route("/getallnotificationTypes")]
    public async Task<IActionResult> GetAllNotificationTypes()
    {
        var notificationTypes = await notificationTypesService.GetAllNotificationTypesAsync();
        return Ok(notificationTypes);
    }

    [HttpGet]
    [Route("/getnotificationTypesbyid")]
    public async Task<IActionResult> GetNotificationTypesById(int id)
    {
        var notificationTypes = await notificationTypesService.GetNotificationTypesByIdAsync(id);
        if (notificationTypes == null)
        {
            return NotFound();
        }
        return Ok(notificationTypes);
    }

    [HttpPost]
    [Route("/submitnotificationTypes")]
    public async Task<IActionResult> SubmitNotificationTypes([FromBody] NotificationTypesDto notificationTypesDto)
    {
        if (notificationTypesDto == null)
        {
            return BadRequest();
        }

        var response = await notificationTypesService.SubmitNotificationTypesAsync(notificationTypesDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete]
    [Route("/deletenotificationTypes")]
    public async Task<IActionResult> DeleteNotificationTypes(int id)
    {
        var response = await notificationTypesService.DeleteNotificationTypesAsync(id);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}