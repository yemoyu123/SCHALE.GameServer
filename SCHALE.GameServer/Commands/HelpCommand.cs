using SCHALE.Common.Database;
using SCHALE.GameServer.Services.Irc;
using Serilog;
using System.Reflection;
using System.Text;

namespace SCHALE.GameServer.Commands
{

    [CommandHandler("help", "Show this help.", "/help")]
    internal class HelpCommand : Command
    {
        public HelpCommand(IrcConnection connection, string[] args, bool validate = true) : base(connection, args, validate) { }

        public override void Execute()
        {   // can't use newline, not gonna print args help for now
            foreach (var command in CommandFactory.commands.Keys)
            {
                var cmdAtr = (CommandHandlerAttribute?)Attribute.GetCustomAttribute(CommandFactory.commands[command], typeof(CommandHandlerAttribute));

                Command? cmd = CommandFactory.CreateCommand(command, connection, args, false);

                if (cmd is not null)
                {
                    connection.SendChatMessage($"{command} - {cmdAtr.Hint} (Usage: {cmdAtr.Usage})");
                }
            }
        }
    }

}
