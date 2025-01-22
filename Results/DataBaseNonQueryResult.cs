using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Kudos.DataBasing.Results
{
    public sealed class
        DataBaseNonQueryResult
    :
        DataBaseResult<Object, DataBaseNonQueryResult>
    {
        public UInt32? UpdatedRows { get; private set; }
        public Boolean HasUpdatedRows { get; private set; }

        internal DataBaseNonQueryResult Eject(ref UInt32? ui32)
        {
            HasUpdatedRows = (UpdatedRows = ui32) != null;
            return this;
        }

        internal DataBaseNonQueryResult()
            : base() { }

        internal DataBaseNonQueryResult(ref DataBaseBenchmarkResult dbbr)
            : base(ref dbbr) { }
    }
}