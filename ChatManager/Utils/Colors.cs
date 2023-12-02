using System.Reflection;
using CounterStrikeSharp.API.Modules.Utils;

namespace ChatManager.Utils;

public abstract class Colors
{
    
    public static string Tags(string message, int teamNum = 0)
    {
        
        if (message.Contains('{'))
        {
            
            string modifiedValue = message;
            
            foreach (FieldInfo field in typeof(ChatColors).GetFields())
            {
                string pattern = $"{{{field.Name}}}";
                if (message.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    modifiedValue = modifiedValue.Replace(pattern, field.GetValue(null)!.ToString(), StringComparison.OrdinalIgnoreCase);
                }
            }
            
            return modifiedValue;
            
        }

        return message;
    }
    
}