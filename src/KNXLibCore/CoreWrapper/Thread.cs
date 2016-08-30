using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading; 

namespace KNXLibCore.CoreWrapper
{
    public class Thread 
    {
        private Action threadFlow;
        private System.Threading.Thread t;

        public Thread(Action threadFlow)
        {
            this.ThreadIsAborted = false;

            this.threadFlow = threadFlow;
            var ts = new ThreadStart(TFlow);
            t = new System.Threading.Thread(ts);
            

            
        }

        private void TFlow() {
            
            threadFlow?.Invoke();
            
        }

        public bool ThreadIsAborted { get; set; }
        public bool IsBackground { get; internal set; }
        public string Name { get; internal set; }
        public System.Threading.ThreadState ThreadState { get {return t.ThreadState; } }

        internal static void ResetAbort()
        {
            
        }

        internal void Start()
        {
            t.IsBackground = IsBackground;
            t.Name = Name;
            t.Start();
        }

        internal void Abort()
        {
            this.ThreadIsAborted = true;
        }

        internal static void Sleep(int v)
        {
            System.Threading.Thread.Sleep(v);
            
        }
    }
}
