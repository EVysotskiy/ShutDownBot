using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command;

public class CommandFactory : ICommandFactory
{
    private readonly ITelegramBotClient _telegramBotClient;

    public CommandFactory(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }
    public ICommand Create(Message message)
    {
        var commandType = GetTypeByMessage(message);

        return commandType switch
        {
            CommandType.ShutDown => new ShutDownCommand(message,_telegramBotClient),
            CommandType.None => new UnregisteredCommand(message,_telegramBotClient),
            _ => throw new ArgumentException("This command type has no handler")
        };
    }

    private CommandType GetTypeByMessage(Message message)
    {
        var isText = message.Text != null;
        
        if (isText)
        {
            return message.Text switch
            {
                "/shutdown" => CommandType.ShutDown,
                _ => CommandType.None
            };
        }

        return CommandType.None;
    }
}