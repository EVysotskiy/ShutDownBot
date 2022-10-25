using Telegram.Bot.Types;

namespace TelegramBot.Handler;

public abstract class IUpdatesHandler
{
    public abstract Task Handle(IReadOnlyList<Update> updates);
}