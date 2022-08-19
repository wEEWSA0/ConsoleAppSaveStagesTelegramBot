using ConsoleAppSaveStagesTelegramBot;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient("5693456259:AAF2NafPdFnC3llBrOYU7WdYrhZAip4Ucgg");

using var cts = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

BotPlayersStatistic.LoadPlayersStats();

botClient.StartReceiving(
updateHandler: BotHandlers.HandleUpdateAsync,
pollingErrorHandler: BotHandlers.HandlePollingErrorAsync,
receiverOptions: receiverOptions,
cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine("Bot started");
Console.WriteLine($"Start listening for @{me.Username}");
Console.WriteLine("Press enter for stop");
Console.ReadKey();

BotPlayersStatistic.SavePlayersStats();

cts.Cancel();

Console.WriteLine("Bot stopped");