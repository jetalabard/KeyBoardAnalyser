using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Common
{
    public class Main
    {
        private static ICollection<ShortcutAction> Actions;

        private static string word;

        private static int maxWordLength;
        private static int minWordLength;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public Main()
        {
            Actions = Config.Shortcuts();
            maxWordLength = Actions.Select(action => action.Shortcut).Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;
            string maxword = string.Empty;
            for(int i=0;i< maxWordLength+1; i++)
            {
                maxword += "X";
            }
            minWordLength = Actions.Select(action => action.Shortcut).Aggregate(maxword, (min, cur) => min.Length < cur.Length ? min : cur).Length;
        }

        public void Analyse()
        {
            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }


        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                // Handle the key press here
                var text = ((Keys)vkCode).ToString();

                word += text;
                if(word.Length >= minWordLength)
                {
                    checkWord();
                }
                
                if(word.Length >= 1000)
                {
                    word = string.Empty;
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static void checkWord()
        {
            ShortcutAction shortcut = null;
            try
            {
                shortcut = Actions.Single(a => word.ToUpper().IndexOf(a.Shortcut.ToUpper()) != -1);
                new DoAction().Execute(shortcut.Action);
                word = string.Empty;

            }
            catch
            {
                //Single ne renvoit auccune sequence
            }
            
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }


    }

}
