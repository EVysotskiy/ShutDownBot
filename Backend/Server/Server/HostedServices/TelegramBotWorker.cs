using System.ComponentModel;
using Telegram.Bot;
using TelegramBot.Handler;

namespace Server.HostedServices;

public class TelegramBotWorker : IHostedService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private IServiceProvider _serviceProvider;
    private CancellationToken _stoppingToken;
    private BackgroundWorker _backgroundWorker;
    private readonly ILogger<TelegramBotWorker> _logger;

    public TelegramBotWorker(IServiceProvider serviceProvider,ITelegramBotClient telegramBotClient, ILogger<TelegramBotWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _telegramBotClient = telegramBotClient;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        StartBot();
         return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
    
    private void StartBot()
    {
        _backgroundWorker = new BackgroundWorker();
        _backgroundWorker.DoWork += OnBackgroundWorkerDoWork;
        if (_backgroundWorker.IsBusy is false)
        {
            _backgroundWorker.RunWorkerAsync();
        }
    } 
    
    private async void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
    {
        if (_telegramBotClient.BotId == null)
        {
            return;
        }
        
        int offset = 0;
        while (_stoppingToken.IsCancellationRequested is false)
        {
            await Task.Delay(1000, _stoppingToken);
            var updates = await _telegramBotClient.GetUpdatesAsync(offset);
            if (updates.Length <= 0)
            {
                continue;
            }

            foreach (var update in updates)
            {
                offset = update.Id + 1;
            }

            using var scope = _serviceProvider.CreateScope();
            var updatesHandler = scope.ServiceProvider.GetRequiredService<IUpdatesHandler>();
            await updatesHandler.Handle(updates);
        }
    }
    
}