using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Utility.Library;

class Program
{
    private static DiscordSocketClient _client;
    private static InteractionService _ioService;
    public static async Task Main()
    {
        IConfiguration configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.DirectMessages | GatewayIntents.MessageContent,
            UseInteractionSnowflakeDate = false
        });

        _ioService = new InteractionService(_client.Rest);


        string token = configuration["token"];

        _client.Log += Logger.Instance.Log;

        await _ioService.AddModuleAsync<InputOutCommands>(null);

        _client.Ready += async () =>
        {
            await _ioService.RegisterCommandsGloballyAsync();
        };

        _client.InteractionCreated += async interaction =>
        {
            var ctx = new SocketInteractionContext(_client, interaction);
            await _ioService.ExecuteCommandAsync(ctx, null);
        };

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

}
