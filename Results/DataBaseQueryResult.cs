using System;
using System.Data;

namespace Kudos.DataBasing.Results
{
    public class DataBaseQueryResult : DataBaseResult
    {
        public readonly DataTable? Data;
        public readonly Boolean HasData;

        internal DataBaseQueryResult
        (
            ref DataTable? dt,
            ref DataBaseErrorResult? dber,
            ref DataBaseBenchmarkResult dbbr
        )
        : base(ref dber, ref dbbr)
        {
            HasData = (Data = dt) != null; 
        }
    }
}