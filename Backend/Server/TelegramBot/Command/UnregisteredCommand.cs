using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command;

public class UnregisteredCommand : ICommand
{
    private readonly Message _message;
    private readonly ITelegramBotClient _telegramBotClient;

    private const string ANSWER_TEXT = "Sorry! I don't know this command.";
    
    public UnregisteredCommand(Message message, ITelegramBotClient telegramBotClient)
    {
        _message = message;
        _telegramBotClient = telegramBotClient;
    }

    public async Task Execute()
    {
        var messageText = ANSWER_TEXT;
        var chatId = _message.Chat.Id;
        await _telegramBotClient.SendTextMessageAsync(chatId, messageText);
    }
}