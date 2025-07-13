using Discord.Commands;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Library
{
    public class InputOutCommands : InteractionModuleBase<SocketInteractionContext>
    {

        [SlashCommand("echo", "Echo an input")]
        public async Task Echo(string input)
        {
            await RespondAsync(input);
        }

        [Discord.Commands.RequireUserPermission(GuildPermission.ManageMessages)]
        [SlashCommand("delete-day", "deletes 1 days worth of data in a given channel")]
        public async Task ClearAllMessages()
        {
            await DeferAsync(ephemeral: true);
            var channel = Context.Channel as SocketTextChannel;

            if (channel == null)
            {
                await FollowupAsync("This command can only be used in text channels.", ephemeral: true);
                return;
            }

            var messages = await channel.GetMessagesAsync(100).FlattenAsync();

            var toDelete = messages.Where(m => (System.DateTimeOffset.UtcNow - m.Timestamp).TotalDays <= 1);

            await channel.DeleteMessagesAsync(toDelete);
            await FollowupAsync("Success!", ephemeral: true);
        }

    }
}
