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

        [SlashCommand("open-ticket", "opens a support ticket")]
        public async Task OpenTicketChannel(string title, string description)
        {
            await DeferAsync(ephemeral: true);
            var channel = Context.Channel as SocketTextChannel;

            if (channel == null)
            {
                await FollowupAsync("This command can only be used in text channels.", ephemeral: true);
                return;
            }
            var category_exists = Context.Guild.CategoryChannels.FirstOrDefault(c => c.Name == "@utility-tickets");

            if (category_exists == null)
            {
                await Context.Guild.CreateCategoryChannelAsync("@utility-tickets");
            }

            var name = $"ticket-{Random.Shared.Next(120000000)}";

            await Context.Guild.CreateTextChannelAsync(name, properties =>
            {
                properties.CategoryId = Context.Guild.CategoryChannels.FirstOrDefault(c => c.Name == "@utility-tickets").Id;
                properties.Topic = description;
                properties.PermissionOverwrites = new List<Overwrite>
                {
                    new Overwrite(Context.Guild.EveryoneRole.Id, PermissionTarget.Role, new OverwritePermissions(viewChannel: PermValue.Deny)),
                    new Overwrite(Context.User.Id, PermissionTarget.User, new OverwritePermissions(viewChannel: PermValue.Allow))
                };
            });

            if(Context.Guild.Channels.FirstOrDefault(c => c.Name == name) is not null)
            {
                var card = new EmbedBuilder()
                .WithTitle("🎫 New Support Ticket")
                .WithDescription("A new ticket has been created.")
                .AddField("Author", $"<@{Context.User.Id}>", true)
                .AddField("Title", title, true)
                .AddField("Description", description)
                .WithColor(Color.Blue)
                .WithTimestamp(DateTimeOffset.Now)
                .WithFooter(footer => footer.Text = "Ticket System")
                .Build();

                var ticketChannel = Context.Guild.GetTextChannel(Context.Guild.Channels.FirstOrDefault(c => c.Name == name).Id) as SocketTextChannel;
                await ticketChannel.SendMessageAsync(embed: card);
            }



            await FollowupAsync("DONE", ephemeral: true);

        }

        [SlashCommand("close-ticket", "Closes this support ticket")]
        public async Task CloseTicket()
        {
            var channel = Context.Channel as SocketTextChannel;

            if (channel == null)
            {
                await RespondAsync("This command can only be used in text channels.", ephemeral: true);
                return;
            }

            string ticketCategoryName = "@utility-tickets";
            if (channel.Category?.Name != ticketCategoryName)
            {
                await RespondAsync("This command can only be used inside a ticket channel.", ephemeral: true);
                return;
            }

            var user = Context.User as SocketGuildUser;
            bool isStaff = user.GuildPermissions.ManageChannels;

            if (!isStaff && !channel.Name.Contains(user.Username, StringComparison.OrdinalIgnoreCase))
            {
                await RespondAsync("Only the ticket creator or staff can close this ticket.", ephemeral: true);
                return;
            }

            await RespondAsync("Closing ticket...", ephemeral: true);

            //allow time for closing ticket text to show
            await Task.Delay(2000);

            await channel.DeleteAsync();
        }


    }
}
