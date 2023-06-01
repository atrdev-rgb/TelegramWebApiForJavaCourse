using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TelegramWebApIForJavaCourse.Data;

namespace TelegramWebApIForJavaCourse.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        [HttpPost]
        public async Task<JsonResult> CreateTelegramChannelAsync(VladimirTripAdvisorEvent vladimirTripAdvisorEvent)
        {
            if (vladimirTripAdvisorEvent.isValid())
            {
                Telegram telegram = new Telegram();
                TelegramGroup telegramGroup = await telegram.CreateTelegramChannel(vladimirTripAdvisorEvent);
                return new JsonResult(Ok(telegramGroup));
            }
            else
            {
                return new JsonResult(BadRequest());
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetChatMessagesCount(long chatId)
        {
            if(chatId == 0)
                return new JsonResult(BadRequest());
            
            //Thread.Sleep(2000);
            Telegram telegram = new Telegram();
            int messagesCount = await telegram.GetChatAllMessages(chatId);
            return new JsonResult(Ok(messagesCount));
            
        }

        [HttpPost]
        public async Task<JsonResult> ChangeChatPhoto(ChatPhoto chatPhoto)
        {
            if (chatPhoto.chatId == 0 || chatPhoto.imageBase64 == null)
                return new JsonResult(BadRequest());

            Telegram telegram = new Telegram();
            await telegram.ChangeChatPhoto(chatPhoto.chatId, chatPhoto.imageBase64);
            return new JsonResult(Ok());

        }
    }
}
