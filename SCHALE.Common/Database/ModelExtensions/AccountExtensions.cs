namespace SCHALE.Common.Database.ModelExtensions
{
    public static class AccountExtensions
    {
        public static List<CharacterDB> AddCharacters(this AccountDB account, SCHALEContext context, params CharacterDB[] characters)
        {
            foreach (var character in characters)
            {
                character.AccountServerId = account.ServerId;
                context.Characters.Add(character);
            }
            
            return [.. characters];
        }
    }
}
