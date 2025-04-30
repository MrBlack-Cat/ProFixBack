using Common.GlobalResponse;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Common.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetMessagesByUserId(int userId)
    {
        if (userId <= 0)
            return Ok(new { isSuccess = true, data = new List<object>() }); 

        var messages = await _chatService.GetMessagesByUserIdAsync(userId);

        return Ok(new { isSuccess = true, data = messages });
    }
}
