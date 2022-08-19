using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ConsoleAppSaveStagesTelegramBot
{
    internal class BotTextLogic
    {
        const string emptyMessage = "empty message";
        const string startMessage = "Бот позволяет повышать игровой баланс";
        public const string menuMessage = "Возможности:\n" +
            "'Статистика' или '/stats' - показывает вашу текущую статистику (баланс)\n" +
            "'Возможности' или '/menu' - показывает данное меню\n" +
            "'Работать' или '/work' - увеличиваете ваш игровой баланс";
        const string findMessage = "Поиск собеседника (вас уведомят, когда он найдется)";
        const string undefinedMessage = "Извините, но команда не распознанна";
        const string foundGroupMessage = "Собеседник нашелся. Вы подключены к нему";
        const string newPlayerMessage = "\nНа ваш счет поступило 1000k!";

        public async void RecieveMessage(Message message)
        {
            string messageText = "" + message.Text;
            string textResult = emptyMessage;

            Console.WriteLine($"Received a '{messageText}' message in chat {message.Chat.Id}.");

            switch (messageText.ToLower())
            {
                case "/start":
                    {
                        if (BotPlayersStatistic.AddNewPlayer(message.Chat.Id))
                        {
                            textResult = Mes(startMessage, menuMessage, newPlayerMessage);
                        }
                        else
                        {
                            textResult = Mes(startMessage, menuMessage);
                        }
                    }
                    break;
                case "/menu":
                case "возможности":
                    {
                        textResult = menuMessage;
                    }
                    break;
                case "/stats":
                case "статистика":
                    {
                        textResult = BotPlayersStatistic.GetPlayerStatisticByChatId(message.Chat.Id);
                    }
                    break;
                case "/work":
                case "работать":
                    {
                        int count = 25;

                        var chatId = message.Chat.Id;
                        BotPlayersStatistic.AddBalanceValueToPlayerByChatId(chatId, count);

                        textResult = $"Вы получили {count}k /work";
                    }
                    break;
                default:
                    {
                        textResult = undefinedMessage + "\n" + menuMessage;
                    }
                    break;
            }

            if (textResult == null) { throw new ArgumentException("textResult equals null"); }

            if (textResult != emptyMessage)
            {
                await BotMessageManager.SendMessageWithOptions(message.Chat.Id, textResult);
            }
        }

        private string Mes(params string[] str)
        {
            string newStr = str[0];

            for (int i = 1; i < str.Length; i++)
            {
                newStr += "\n" + str[i];
            }

            return newStr;
        }
    }
}
