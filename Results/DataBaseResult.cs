using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Kudos.DataBasing.Results
{
    public class
        DataBaseResult
    {
        public readonly DataBaseBenchmarkResult Benchmark;
        public readonly DataBaseErrorResult? Error;
        public readonly Boolean HasError;

        internal DataBaseResult(ref DataBaseErrorResult? dber) : this(ref dber, ref DataBaseBenchmarkResult.Empty) { }
        internal DataBaseResult(ref DataBaseBenchmarkResult dbbr) : this(ref DataBaseErrorResult.Null, ref dbbr) { }
        internal DataBaseResult(ref DataBaseErrorResult? dber, ref DataBaseBenchmarkResult dbbr)
        {
            HasError = (Error = dber) != null;
            Benchmark = dbbr.Stop();
        }
    }
}