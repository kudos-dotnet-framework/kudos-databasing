using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Kudos.DataBasing.Interfaces.Drivers;
using Kudos.DataBasing.Results;
using Kudos.Threading.Types;
using MySql.Data.MySqlClient;

namespace Kudos.DataBasing.Executors
{
	internal sealed class
        MySQLDataBaseExecutor
	:
		ADataBaseExecutor<MySqlCommand>
	{
        #region ... static ...

        private static readonly String
            __sLastInsertedScalarCommandText;

        static MySQLDataBaseExecutor()
        {
            __sLastInsertedScalarCommandText = "SELECT LAST_INSERT_ID()";
        }

        #endregion

        internal MySQLDataBaseExecutor(ref IDataBaseDriver dbd, ref MySqlCommand? dbc, ref SmartSemaphoreSlim sss)
            : base(ref dbd, ref dbc, ref sss) { }

        protected override void _OnFastGetLastInsertedScalar(ref MySqlCommand dbc, out Exception? exc, out object? o)
        {
            exc = null;
            o = dbc.LastInsertedId;
        }

        protected override void _OnGetLastInsertedScalarCommandText(out string s)
        {
            s = __sLastInsertedScalarCommandText;
        }
    }
}

