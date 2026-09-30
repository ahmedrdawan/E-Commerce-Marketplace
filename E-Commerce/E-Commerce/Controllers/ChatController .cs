using E_Commerce.Services.Interfaces;
using E_Commerce.ViewModels.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [AllowAnonymous]
    public class ChatController : Controller
    {
        private readonly IChatbotService _chatbotService;

        public ChatController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequestViewModel request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("Message cannot be empty.");
            }

            if (request.Message.Length > 500)
            {
                return BadRequest("Message is too long. Maximum length is 500 characters.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reply = await _chatbotService.GetReplyAsync(request.Message, userId);

            return Json(new ChatResponseViewModel { Reply = reply });
        }
    }
}