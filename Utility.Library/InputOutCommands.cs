using Discord.Interactions;
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

    }
}
