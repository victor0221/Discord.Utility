using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Utility.Library;

class Program
{
    private static DiscordSocketClient _client;
    public static async Task Main()
    {
        IConfiguration configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        _client = new DiscordSocketClient();

        _client.Log += Logger.Instance.Log;

        string token = configuration["token"];

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

}
