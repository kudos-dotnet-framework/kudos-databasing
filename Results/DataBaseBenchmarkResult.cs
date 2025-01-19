using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kudos.DataBasing.Results
{
    public sealed class
        DataBaseBenchmarkResult
    {
        #region ... static ...

        internal static /*readonly*/ DataBaseBenchmarkResult
            Empty;

        static DataBaseBenchmarkResult()
        {
            Empty = new DataBaseBenchmarkResult();
        }

        #endregion

        public TimeSpan ElapsedTimeOnWaiting { get { return _swOnWaiting.Elapsed; } }
        public TimeSpan ElapsedTimeOnConnecting { get { return _swOnConnecting.Elapsed; } }
        public TimeSpan ElapsedTimeOnExecuting { get { return _swOnExecuting.Elapsed; } }
        public TimeSpan ElapsedTimeOnPreparing { get { return _swOnPreparing.Elapsed; } }

        private readonly Stopwatch
            _swOnConnecting,
            _swOnExecuting,
            _swOnWaiting,
            _swOnPreparing;

        internal DataBaseBenchmarkResult()
        {
            _swOnWaiting = new Stopwatch();
            _swOnExecuting = new Stopwatch();
            _swOnConnecting = new Stopwatch();
            _swOnPreparing = new Stopwatch();
        }

        internal DataBaseBenchmarkResult StartOnWaiting()
        {
            _swOnWaiting.Start();
            return this;
        }

        internal DataBaseBenchmarkResult StartOnConnecting()
        {
            _swOnConnecting.Start();
            return this;
        }

        internal DataBaseBenchmarkResult StartOnPreparing()
        {
            _swOnPreparing.Start();
            return this;
        }

        internal DataBaseBenchmarkResult StartOnExecuting()
        {
            _swOnExecuting.Start();
            return this;
        }

        internal DataBaseBenchmarkResult StopOnWaiting()
        {
            if (_swOnWaiting.IsRunning) _swOnWaiting.Stop();
            return this;
        }

        internal DataBaseBenchmarkResult StopOnPreparing()
        {
            if (_swOnPreparing.IsRunning) _swOnPreparing.Stop();
            return this;
        }

        internal DataBaseBenchmarkResult StopOnConnecting()
        {
            if (_swOnConnecting.IsRunning) _swOnConnecting.Stop();
            return this;
        }

        internal DataBaseBenchmarkResult StopOnExecuting()
        {
            if (_swOnExecuting.IsRunning) _swOnExecuting.Stop();
            return this;
        }

        internal DataBaseBenchmarkResult Stop()
        {
            return StopOnWaiting().StopOnConnecting().StopOnPreparing().StopOnExecuting();
        }
    }
}
