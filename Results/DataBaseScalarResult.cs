using System;
using System.Data;

namespace Kudos.DataBasing.Results
{
    public sealed class
        DataBaseScalarResult
    :
        DataBaseResult<Object, DataBaseScalarResult>
    {
        internal DataBaseScalarResult()
            : base() { }
        internal DataBaseScalarResult(ref DataBaseBenchmarkResult dbbr)
            : base(ref dbbr) { }
    }
}