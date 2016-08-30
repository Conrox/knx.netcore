using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KNXLibCore.CoreWrapper
{
    public class Timer
    {
        System.Threading.Timer _timer;

        internal bool AutoReset;
        internal Action<object, ElapsedEventArgs> Elapsed;
        private int stateRequestTimerInterval;

        public Timer(int stateRequestTimerInterval)
        {
            this.stateRequestTimerInterval = stateRequestTimerInterval;

            _timer = new System.Threading.Timer(TimerCallback, null, this.stateRequestTimerInterval, System.Threading.Timeout.Infinite);
            this.Enabled = true;
            //Elapsed = new Action<object, ElapsedEventArgs>()
        }

        private void TimerCallback(object state)
        {
            if (Enabled)
            {
                Elapsed?.Invoke(state, new ElapsedEventArgs());
            }
        }

        public bool Enabled { get; set; }

        
    }

    internal class ElapsedEventArgs
    {
    }
}
