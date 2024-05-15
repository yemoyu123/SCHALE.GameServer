using SCHALE.Common.Database;
using SCHALE.GameServer.Services.Irc;
using System.ComponentModel;
using System.Reflection;

namespace SCHALE.GameServer.Commands
{
    [CommandHandler("setaccount", "Command to change player's account data", "/setaccount <|Level|Nickname|RaidSeasonId|Property|...> <Value>")]
    internal class SetAccountCommand : Command
    {
        public SetAccountCommand(IrcConnection connection, string[] args, bool validate = true) : base(connection, args, validate) { }

        [Argument(0, @"^[a-zA-Z]+$", "The Account Property you want to change.", ArgumentFlags.IgnoreCase)]
        public string Property { get; set; } = string.Empty;

        [Argument(1, @"", "The value you want to change it to, must match the property type.", ArgumentFlags.IgnoreCase)]
        public string Value { get; set; } = string.Empty;

        public override void Execute()
        {
            PropertyInfo? targetProperty = typeof(AccountDB).GetProperty(Property);
            
            if (targetProperty != null)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(targetProperty.PropertyType);

                if (converter != null && converter.CanConvertFrom(typeof(string)))
                {
                    try
                    {
                        object targetValue = converter.ConvertFromString(Value);

                        targetProperty.SetValue(connection.Account, targetValue);
                    } catch (Exception)
                    {
                        throw new ArgumentException("Invalid Value");
                    }
                } 
            } else
            {
                throw new ArgumentException("Invalid Player Property!");
            }


        }
    }
}
