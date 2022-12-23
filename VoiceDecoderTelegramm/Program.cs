using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VoiceDecoderTelegramm.Configuration;
using VoiceDecoderTelegramm.Controllers;
using VoiceDecoderTelegramm.Services;
using VoiceTexterBot;
using VoiceTexterBot.Controllers;
using VoiceTexterBot.Services;

public class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.Unicode;

        // Объект, отвечающий за постоянный жизненный цикл приложения
        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
            .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
            .Build(); // Собираем

        Console.WriteLine("Сервис запущен");
        // Запускаем сервис
        await host.RunAsync();
        Console.WriteLine("Сервис остановлен");
    }

    static void ConfigureServices(IServiceCollection services)
    {
        AppSettings appSettings = BuildAppSettings();
        services.AddSingleton(BuildAppSettings());

        services.AddSingleton<IStorage, MemoryStorage>();

        services.AddTransient<DefaultMessageController>();
        services.AddTransient<VoiceMessageController>();
        services.AddTransient<TextMessageController>();
        services.AddTransient<InlineKeyboardController>();
        services.AddSingleton<IStorage, MemoryStorage>();
        services.AddSingleton<IFileHandler, AudioFileHandler>();
        // Регистрируем объект TelegramBotClient c токеном подключения
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("5665630812:AAFBDNgaTccs529KntBZ7q_v2AQfgEwl2ok"));
        // Регистрируем постоянно активный сервис бота
        services.AddHostedService<Bot>();
    }

    static AppSettings BuildAppSettings()
    {
        
        
        return new AppSettings()
                {
                    DownloadsFolder = "C:\\Users\\Stank\\source\\repos\\VoiceDecoderTelegramm",
                    BotToken = "5665630812:AAFBDNgaTccs529KntBZ7q_v2AQfgEwl2ok",
                    AudioFileName = "audio",
                    InputAudioFormat = "ogg",
                    OutputAudioFormat = "wav",
        };
   
    }
}
