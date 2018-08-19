using System;
using System.Windows.Threading;

namespace SpaceEngineers2D
{
    public static class Dispatch
    {
        public static void Exec(Action action)
        {
            var t = new DispatcherTimer();
            t.Tick += (sender, args) =>
            {
                action();
                t.Stop();
            };
            t.Start();
        }
    }
}
