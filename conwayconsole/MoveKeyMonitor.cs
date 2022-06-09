using System;
using System.Threading;

namespace ConwayConsole
{

    internal static class MoveKeyMonitor
    {
        public static event EventHandler<MovementEventArgs> Movement;

        public static void Start()
        {
            new Thread(Monitor) { IsBackground = true }.Start();
        }

        private static void Monitor()
        {
            for (; ; )
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        Movement?.Invoke(null, new MovementEventArgs(0, -1));
                        break;

                    case ConsoleKey.RightArrow:
                        Movement?.Invoke(null, new MovementEventArgs(0, 1));
                        break;
                        
                    case ConsoleKey.UpArrow:
                        Movement?.Invoke(null, new MovementEventArgs(-1, 0));
                        break;
                        
                    case ConsoleKey.DownArrow:
                        Movement?.Invoke(null, new MovementEventArgs(1, 0));
                        break;
                        
                    default:
                        break;
                }
            }
        }
    }
}