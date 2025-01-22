using System;
using System.Data;

namespace Kudos.DataBasing.Results
{
    public sealed class
        DataBaseQueryResult
    :
        DataBaseResult<DataTable, DataBaseQueryResult>
    {
        internal DataBaseQueryResult()
            : base() { }
        internal DataBaseQueryResult(ref DataBaseBenchmarkResult dbbr)
            : base(ref dbbr) { }
    }
}