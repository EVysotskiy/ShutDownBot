using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Command;

namespace TelegramBot.Handler;

public class UpdatesHandler : IUpdatesHandler
{
    private readonly ICommandFactory _commandFactory;

    public UpdatesHandler(ICommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }

    public override async Task Handle(IReadOnlyList<Update> updates)
    {
        foreach (var update in updates)
        {
            var isMessage = update.Type == UpdateType.Message;

            if (isMessage is false || ReferenceEquals(update.Message,null))
            {
                return;
            }

            var message = update.Message;
            var command = _commandFactory.Create(message);
            await command.Execute();
        }
    }
}