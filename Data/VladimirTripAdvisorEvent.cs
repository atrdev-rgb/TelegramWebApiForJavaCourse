using System.Runtime.CompilerServices;

namespace TelegramWebApIForJavaCourse.Data
{
    public class VladimirTripAdvisorEvent
    {
        public string? ChannelName { get; set; }
        public string? StartDate { get; set; }
        public string? PlaceName { get; set; }
        public long? PlaceId { get; set; }
        public string? ImageBase64 { get; set; }

        public bool isValid()
        {
            if (ChannelName == null || StartDate == null
                || PlaceName == null || PlaceId == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
