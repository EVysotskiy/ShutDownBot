using Telegram.Bot;
using Telegram.Bot.Types;
using System.Management;

namespace TelegramBot.Command;

public class ShutDownCommand :ICommand
{
    private readonly Message _message;
    private readonly ITelegramBotClient _telegramBotClient;

    private const string MESSAGE = "Computer turned off";

    public ShutDownCommand(Message message, ITelegramBotClient telegramBotClient)
    {
        _message = message;
        _telegramBotClient = telegramBotClient;
    }
    public async Task Execute()
    {
        var chatId = _message.Chat.Id;
        Shutdown();
        await _telegramBotClient.SendTextMessageAsync(chatId, MESSAGE);
    }

    
    //https://stackoverflow.com/questions/102567/how-to-shut-down-the-computer-from-c-sharp
    private void Shutdown()
    {
        ManagementBaseObject mboShutdown = null;
        ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
        mcWin32.Get();
        
        mcWin32.Scope.Options.EnablePrivileges = true;
        ManagementBaseObject mboShutdownParams =
            mcWin32.GetMethodParameters("Win32Shutdown");
        
        mboShutdownParams["Flags"] = "1";
        mboShutdownParams["Reserved"] = "0";
        foreach (ManagementObject manObj in mcWin32.GetInstances())
        {
            mboShutdown = manObj.InvokeMethod("Win32Shutdown", 
                mboShutdownParams, null);
        }
    }
}