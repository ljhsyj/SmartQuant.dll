using System;
using System.Threading;

namespace TestLogic
{
    class GracefullyExit : IDisposable
    {
        private volatile bool exit;
        private Thread thread;
        public GracefullyExit()
        {
            this.exit = false;
            this.thread = new Thread(new ThreadStart(this.Run));
            this.thread.Name = "Event Manager Thread";
            this.thread.IsBackground = true;
            this.thread.Start();
            while (!thread.IsAlive)
                Thread.Sleep(1);
        }
        private void Run()
        {
            while (!this.exit)
            {
                Console.WriteLine("Doing work...");
                Thread.Sleep(10);
            }
            Console.WriteLine("Thread exit gracefully.");
        }

        public void Dispose()
        {
            this.exit = true;
            this.thread.Join();
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            using (var t = new GracefullyExit())
            {
                Console.WriteLine("In Main thread");
                Thread.Sleep(100);
            }
            Console.WriteLine("Main thread done.");
        }
    }
}
