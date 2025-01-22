using Kudos.DataBasing.Enums;
using Kudos.DataBasing.Executors;
using Kudos.DataBasing.Interfaces.Drivers;
using Kudos.DataBasing.Interfaces.Executors;
using Kudos.Threading.Types;
using Microsoft.Data.SqlClient;

namespace Kudos.DataBasing.Drivers
{
    public sealed class
        MicrosoftSQLDataBaseDriver
    :
        ADataBaseDriver
        <
            SqlConnection,
            SqlCommand
        >
    {
        #region ... static ...

        private static EDataBaseType
            __eType;

        static MicrosoftSQLDataBaseDriver()
        {
            __eType = EDataBaseType.MicrosoftSQL;
        }

        #endregion

        internal MicrosoftSQLDataBaseDriver(ref SqlConnection dbc) : base(ref dbc, ref __eType) { }

        protected override void _OnNewRequestExecutor(ref IDataBaseDriver dbd, ref SqlCommand? dbc, ref SmartSemaphoreSlim sss, out IDataBaseExecutor dbe)
        {
            dbe = new MicrosoftSQLDataBaseExecutor(ref dbd, ref dbc, ref sss);
        }
    }
}