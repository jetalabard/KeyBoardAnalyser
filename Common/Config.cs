using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Common
{
    public static class Config
    {
        public static ICollection<ShortcutAction> Shortcuts()
        {
            ICollection<ShortcutAction> list = new List<ShortcutAction>();
            NameValueCollection actions = (NameValueCollection)ConfigurationSettings.GetConfig("Action");
            NameValueCollection shortcuts = (NameValueCollection)ConfigurationSettings.GetConfig("Shortcut");

            for(int i= 0; i < actions.Count;i++)
            {
                string key = actions.Keys[i];
                string action = actions.GetValues(key)[0];
                string shortcut = shortcuts.GetValues(key)[0];
                list.Add(new ShortcutAction(shortcut,action));
            }
            return list;
        }
    }
}
