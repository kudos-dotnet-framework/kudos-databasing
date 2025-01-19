using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Kudos.DataBasing.Results
{
    public sealed class
        DataBaseNonQueryResult
    :
        DataBaseResult
    {
        public readonly UInt32? UpdatedRows;
        public readonly Boolean HasUpdatedRows;
        public readonly UInt64? LastInsertedID;
        public readonly Boolean HasLastInsertedID;

        internal DataBaseNonQueryResult
        (
            ref UInt64? lLastInsertedID,
            ref UInt32? iUpdateRows,
            ref DataBaseErrorResult? dber,
            ref DataBaseBenchmarkResult dbbr
        )
        :
            base(ref dber, ref dbbr)
        {
            HasLastInsertedID = (LastInsertedID = lLastInsertedID) != null;
            HasUpdatedRows = (UpdatedRows = iUpdateRows) != null;
        }
    }
}