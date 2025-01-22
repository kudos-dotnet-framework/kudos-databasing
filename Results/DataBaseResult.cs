using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Kudos.DataBasing.Results
{
    public class
        DataBaseResult<PayLoadType, ThisType>
    :
        DataBaseResult<ThisType>
    where
        ThisType : DataBaseResult<PayLoadType, ThisType>
    {
        public PayLoadType? PayLoad { get; private set; }
        public Boolean HasPayLoad { get; private set; }

        internal ThisType Eject(ref PayLoadType? pl)
        {
            HasPayLoad = (PayLoad = pl) != null;
            return this as ThisType;
        }

        internal DataBaseResult()
            : base() { }
        internal DataBaseResult(ref DataBaseBenchmarkResult dbbr)
            : base(ref dbbr) { }
    }

    public class
        DataBaseResult<ThisType>
    :
        DataBaseResult
    where
        ThisType : DataBaseResult<ThisType>
    {
        internal new ThisType Eject(ref DataBaseErrorResult? dber) { return base.Eject(ref dber) as ThisType; }

        internal DataBaseResult() : base() { }
        internal DataBaseResult(ref DataBaseBenchmarkResult dbbr) : base(ref dbbr) { }
    }

    public class
        DataBaseResult
    {
        public readonly DataBaseBenchmarkResult Benchmark;

        public DataBaseErrorResult? Error { get; private set; }
        public Boolean HasError { get; private set; }

        internal DataBaseResult Eject(ref DataBaseErrorResult? dber)
        {
            HasError = (Error = dber) != null;
            return this;
        }

        internal DataBaseResult() : this(ref DataBaseBenchmarkResult.Empty) { }
        internal DataBaseResult(ref DataBaseBenchmarkResult dbbr)
        {
            Benchmark = dbbr.Stop();
        }
    }
}