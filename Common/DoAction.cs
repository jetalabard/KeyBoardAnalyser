using System.Runtime.InteropServices;

namespace Common
{
    public class DoAction
    {
        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        private const string URL_GOOGLE = "http://www.google.com";

        public void Execute(string action)
        {
            switch (action)
            {
                case "standby":
                    SetSuspendState(false, true, true);
                    break;
                case "internet":
                    System.Diagnostics.Process.Start(URL_GOOGLE);
                    break;
                default:
                    break;
            }
        }
    }
}
