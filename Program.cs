using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using dotenv.net;
using SpendTrackrTeleBot;

// Load environment variables from .env file
DotEnv.Load();

// Get bot token from environment variables
var botToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN") 
    ?? throw new Exception("TELEGRAM_BOT_TOKEN environment variable is not set");

var botClient = new TelegramBotClient(botToken);

// Start receiving updates
var me = await botClient.GetMeAsync();
Console.WriteLine($"Bot is running! Username: {me.Username}");

// Set up command handling
using var cts = new CancellationTokenSource();
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = []
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

// Keep the application running
Console.WriteLine("Press any key to exit");
Console.ReadKey();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message?.Text is not { } messageText)
        return;

    var chatId = update.Message.Chat.Id;
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    MainControl start = new();
    string feedback = await start.Run(messageText);
    
    // Echo the received message
    await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: feedback,
        cancellationToken: cancellationToken);
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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
