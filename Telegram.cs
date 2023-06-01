using System;
using TelegramWebApIForJavaCourse.Data;
using TL;
using WTelegram;

namespace TelegramWebApIForJavaCourse
{
    public class Telegram
    {
        private static Client client = new WTelegram.Client(Config);

        private static string Config(string what)
        {
            switch (what)
            {
                case "api_id": return "#######";
                case "api_hash": return "############################";
                case "phone_number": return "#######";
                case "verification_code": Console.Write("Code: "); return Console.ReadLine();
                case "first_name": return "John";      // if sign-up is required
                case "last_name": return "Doe";        // if sign-up is required
                case "password": return "#####################";     // if user has enabled 2FA
                default: return null;                  // let WTelegramClient decide the default config
            }
        }

        public async Task<TelegramGroup?> CreateTelegramChannel(VladimirTripAdvisorEvent vladimirTripAdvisorEvent)
        {
            if (client != null)
            {
                await client.ConnectAsync();

                var users = new InputUser[1];
                for (int i = 0; i < users.Length; i++)
                {
                    users[i] = new InputUser(5802032660, -1117118480243990476);
                }
                var chat = await client.Messages_CreateChat(users, vladimirTripAdvisorEvent.ChannelName);
                long chatId = chat.Chats.First().Value.ID;


                var telegraMessage = await client.Messages_SendMessage(new InputPeerChat(chatId), $"Приветствую. Спасибо что присоединилсь к мероприятию." +
                    $"\nЦель посещения: {vladimirTripAdvisorEvent.PlaceName}\nДата посещения: {vladimirTripAdvisorEvent.StartDate}\nВпрочем, время и дату посещения вы всегда можете сами обсудить" +
                    $"\nПосле посещения места, пожалуйста, оставьте отзыв по ссылке сообщением ниже:", new Random().NextInt64());
                await client.Messages_SendMessage(new InputPeerChat(chatId), $"https://localhost:8080/object/view-object/{vladimirTripAdvisorEvent.PlaceId}", new Random().NextInt64());

                await client.Messages_UpdatePinnedMessage(new InputPeerChat(chatId), (telegraMessage.UpdateList.First() as UpdateMessageID).id);

                ChatInviteExported exportedChatInvite = (ChatInviteExported)await client.Messages_ExportChatInvite(new InputPeerChat(chatId));

                TelegramGroup telegramGroup = new TelegramGroup();
                telegramGroup.TelegramLink = exportedChatInvite.link;
                telegramGroup.TelegramGroupId = chatId.ToString();
                return telegramGroup;
            }
            return null;
        }

        public async Task<int> GetChatAllMessages(long chatId)
        {
            if (client != null)
            {
                await client.ConnectAsync();

                var chats = await client.Messages_GetAllChats();
                InputPeer inputPeer = chats.chats[chatId];
                var messages = await client.Messages_GetHistory(inputPeer, 0);
                return messages.Count;
            }
            return 0;
        }


        public async Task ChangeChatPhoto(long chatId, string imageBase64)
        {
            using (var client = new Client(Config))
            {
                await client.ConnectAsync();
                var chats = await client.Messages_GetAllChats();
                var bytes = Convert.FromBase64String(imageBase64);
                var contents = new MemoryStream(bytes);

                //var file = await client.UploadFileAsync(contents, vladimirTripAdvisorEvent.ChannelName, null);

                InputPeer peer = chats.chats[chatId];

                //var t = await client.Upload_SaveFilePart(imageId, 0, Convert.FromBase64String(vladimirTripAdvisorEvent.ImageBase64));

                InputChatPhotoBase inputChatUploadedPhoto = new InputChatUploadedPhoto()
                {
                    file = await client.UploadFileAsync(contents, chatId + ".jpeg")
                };

                await client.EditChatPhoto(peer, inputChatUploadedPhoto);
            }
        }


        public async Task CheckTelegramClientConnectionAsync()
        {
            using (var client = new WTelegram.Client(Config))
            {
                await client.ConnectAsync();
                var chats = await client.Messages_GetAllChats();
                Console.WriteLine("This user has joined the following:");
                foreach (var (id, chat) in chats.chats)
                    if (chat.IsActive)
                        Console.WriteLine($"{id,10}: {chat}");
            }
        }

        public async Task LoginInClientAsync()
        {
            using var client = new WTelegram.Client();
            var myself = await client.LoginUserIfNeeded();
            Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");
        }
    }
}
