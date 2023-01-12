namespace Performance
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Threading;

    #region class PerformanceTimer
    /// <summary>
    /// Encapsulates access to system performance counter.
    /// </summary>
    public class PerformanceTimer
    {
        #region Constructors

        #region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceTimer"/> class.
        /// Timers is not started.
        /// </summary>
        public PerformanceTimer()
        {
            Reset();
        }
        #endregion

        #endregion

        #region class PerformanceTimerScope
        /// <summary>
        /// Utility class to use with <c>using</c> keyword. 
        /// See <see cref="PerformanceTimer.Scope()"/> for more information.
        /// Basicly, class which starts the timer when instantiated and stops then timer when disposed.
        /// </summary>
        internal sealed class PerformanceTimerScope : IDisposable
        {
            #region Fields

            #region fields
            private readonly PerformanceTimer _timer;
            #endregion

            #endregion

            #region Constructors

            #region constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="PerformanceTimerScope"/> class.
            /// </summary>
            /// <param name="timer">The timer to control.</param>
            public PerformanceTimerScope(PerformanceTimer timer)
            {
                _timer = timer;
                timer.Start();
            }
            #endregion

            #endregion

            #region Methods

            #region IDisposable Members
            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// In this case it just stops controlled timer.
            /// </summary>
            public void Dispose()
            {
                _timer.Stop();
            }
            #endregion

            #endregion
        }
        #endregion

        #region fields
        private int _startedCount;
        private long _startTime;
        private long _stopTime;
        private long _frequency;
        private long _elapsedTime;
        #endregion

        #region imports
        [DllImport("kernel32.dll")]
        internal static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("kernel32.dll")]
        internal static extern bool QueryPerformanceFrequency(out long lpFrequency);
        #endregion

        #region properties
        /// <summary>
        /// Returns the duration of the timer (in seconds)
        /// </summary>
        /// <value>The elapsed time in seconds.</value>
        public double Elapsed
        {
            get { return (double) (_elapsedTime)/(double) _frequency; }
        }

        /// <summary>
        /// Gets the elapsed TimeSpan.
        /// </summary>
        /// <value>The elapsed TimeSpan.</value>
        public TimeSpan ElapsedTimeSpan
        {
            get { return TimeSpan.FromSeconds(Elapsed); }
        }
        #endregion

        #region public interface
        /// <summary>
        /// Resets the timer. Timer is not started.
        /// </summary>
        public void Reset()
        {
            _startTime = _startTime = _elapsedTime = 0;
            _startedCount = 0;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            if (_startedCount == 0)
            {
                // lets waiting threads do their work
                // this thread will more likely go uninterrupted when doing measurement
                Thread.Sleep(0);

                if (!QueryPerformanceFrequency(out _frequency))
                {
                    throw new Win32Exception();
                }

                if (!QueryPerformanceCounter(out _startTime))
                {
                    throw new Win32Exception();
                }
            }
            _startedCount++;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start(bool reset)
        {
            if (reset)
            {
                if (_startedCount > 0)
                {
                    throw new ArgumentException("Cannot restart nested timer");
                }
                Reset();
            }
            Start();
        }

        /// <summary>
        /// Stops the timer. Elapsed time is added to total elapsed time.
        /// </summary>
        public double Stop()
        {
            if (_startedCount <= 0)
            {
                return Elapsed;
            }

            QueryPerformanceCounter(out _stopTime);
            _startedCount--;
            _elapsedTime += _stopTime - _startTime;
            _startTime = _stopTime;
            return Elapsed;
        }

        /// <summary>
        /// Simple utility to use with <c>using</c> keyword. See example.
        /// Timer is started when object is created and stops when object is disposed.
        /// </summary>
        /// <example><code>
        /// timer = new PerformanceTimer();
        /// while (!queue.Empty)
        /// {
        ///   object element = queue.Dequeue();
        ///   using (timer.Scope())
        ///   {
        ///     operation(element); // only time of this line counts towards total time
        ///   }
        /// }
        /// </code></example>
        /// <returns><see cref="IDisposable"/> object. You don't have to handle it.</returns>
        public IDisposable Scope()
        {
            return new PerformanceTimerScope(this);
        }

        /// <summary>Measures the time spent executing action.</summary>
        /// <param name="name">The name.</param>
        /// <param name="repeat">Number of repetiotions.</param>
        /// <param name="action">The action.</param>
        /// <param name="size">The size.</param>
        public static void Debug(string name, int repeat, Action action, double size = 0)
        {
            var timer = new PerformanceTimer();
            using (timer.Scope())
            {
                action();
            }
            var time = timer.Elapsed/repeat;
            Console.WriteLine("{0}: {1:0.0000}ms ({2:0.00}/s)", name, time*1000, size/time);
        }

        /// <summary>Measures the time spent executing action.</summary>
        /// <typeparam name="T">Type of result.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="repeat">Number of repetiotions.</param>
        /// <param name="action">The action.</param>
        /// <param name="size">The size.</param>
        /// <returns>Value returned by action.</returns>
        public static T Debug<T>(string name, int repeat, Func<T> action, double size = 0)
        {
            var result = default(T);
            var timer = new PerformanceTimer();
            using (timer.Scope())
            {
                for (int i = 0; i < repeat; i++)
                {
                    result = action();
                }
            }
            var time = timer.Elapsed/repeat;
            Console.WriteLine("{0}: {1:0.0000}ms ({2:0.00}/s)", name, time*1000, size/time);
            return result;
        }
        #endregion
    }
    #endregion
}
