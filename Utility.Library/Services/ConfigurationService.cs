using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Library.Services
{
    public class ConfigurationService : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("configure", "intial command to be run to configure the server to be utility compatible")]
        public async Task ConfigureGuild()
        {
            await DeferAsync(ephemeral: true);

            await Context.Guild.CreateRoleAsync(
                "@utility-admin",
                new GuildPermissions(administrator: true),
                Color.Blue,
                false,
                false
            );

            await Context.Guild.CreateRoleAsync(
                "@utility-moderator",
                new GuildPermissions(
                      viewChannel: true,
                      readMessageHistory: true,
                      sendMessages: true,
                      connect: true,
                      speak: true,
                      manageMessages: true,
                      muteMembers: true,
                      deafenMembers: true,
                      moveMembers: true,
                      moderateMembers: true,
                      kickMembers: true,
                      viewAuditLog: true,
                      manageThreads: true,
                      createPublicThreads: true,
                      createPrivateThreads: true
                    ),
                Color.Blue,
                false,
                false
            );

            await Context.Guild.CreateRoleAsync(
                "@utility-member",
                new GuildPermissions(
                    viewChannel: true,
                    readMessageHistory: true,
                    sendMessages: true,
                    connect: true,
                    speak: true,
                    addReactions: true,
                    embedLinks: true,
                    attachFiles: true,
                    useExternalEmojis: true,
                    sendMessagesInThreads: true
                    ),
                Color.Blue,
                false,
                false
            );

            await Context.Guild.CreateRoleAsync(
                "@utility-visitor",
                new GuildPermissions(
                    viewChannel: true,
                    readMessageHistory: true,
                    sendMessages: false,
                    connect: true,
                    speak: false
                    ),
                Color.Blue,
                false,
                false
            );

            await FollowupAsync("Success!", ephemeral: true);

        }
    }
}
