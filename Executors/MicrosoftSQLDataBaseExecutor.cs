using System;
using Kudos.Coring.Constants;
using Kudos.DataBasing.Interfaces.Drivers;
using Kudos.Threading.Types;
using Microsoft.Data.SqlClient;

namespace Kudos.DataBasing.Executors
{
    internal sealed class
        MicrosoftSQLDataBaseExecutor
    :
        ADataBaseExecutor<SqlCommand>
    {
        #region ... static ...

        private static readonly String
            __sLastInsertedScalarCommandText;

        static MicrosoftSQLDataBaseExecutor()
        {
            __sLastInsertedScalarCommandText = "SELECT SCOPE_IDENTITY()";
        }

        #endregion

        internal MicrosoftSQLDataBaseExecutor(ref IDataBaseDriver dbd, ref SqlCommand? dbc, ref SmartSemaphoreSlim sss)
            : base(ref dbd, ref dbc, ref sss) { }

        protected override void _OnFastGetLastInsertedScalar(ref SqlCommand dbc, out Exception? exc, out object? o)
        {
            exc = CException.NotImplementedException;
            o = null;
        }

        protected override void _OnGetLastInsertedScalarCommandText(out string s)
        {
            s = __sLastInsertedScalarCommandText;
        }
    }
}

