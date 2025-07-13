using Discord;
using Discord.WebSocket;

class Program
{
    private static DiscordSocketClient _client;
    public static async Task Main()
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;

        //move to secrets
        var token = "";

        // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
        // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
        // var token = File.ReadAllText("token.txt");
        // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

}
