
namespace Common
{
    public class ShortcutAction
    {
        public string Shortcut { get; private set; }
        public string Action { get; private set; }

        public ShortcutAction(string Shortcut, string Action)
        {
            this.Shortcut = Shortcut;
            this.Action = Action;
        }
    }
}
