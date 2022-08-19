using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace ConsoleAppSaveStagesTelegramBot
{
    internal class BotHandlers
    {
        private static BotTextLogic _textLogic = new BotTextLogic();
        private static BotMessageManager _sender = new BotMessageManager();

        public async static Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Инициализация скрипта, отпраляющего сообщения
            if (!_sender.IsInitialize)
            {
                _sender = new BotMessageManager(botClient, cancellationToken);
            }

            if (update.CallbackQuery != null)
            {
                if (update.CallbackQuery.Data != null)
                {
                    // работа с кнопками была вырезана
                    return;
                }
            }

            if (update.Message is not { } message)
                return;

            if (message.Text != null)
            {
                var chatId = message.Chat.Id;

                _textLogic.RecieveMessage(message);
            }
        }
        #region Errors Handler
        public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        #endregion
    }
}
