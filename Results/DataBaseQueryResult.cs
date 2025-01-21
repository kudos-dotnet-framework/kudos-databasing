using System;
using System.Data;

namespace Kudos.DataBasing.Results
{
    public sealed class
        DataBaseQueryResult
    :
        DataBaseResult
    {
        public readonly DataTable? DataTable;
        public readonly Boolean HasDataTable;

        internal DataBaseQueryResult
        (
            ref DataTable? dt,
            ref DataBaseErrorResult? dber,
            ref DataBaseBenchmarkResult dbbr
        )
        :
        base
        (
            ref dber,
            ref dbbr
        )
        {
            HasDataTable = (DataTable = dt) != null; 
        }
    }
}