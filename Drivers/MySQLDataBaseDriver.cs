using System;
using System.Data;
using Kudos.DataBasing.Enums;
using Kudos.DataBasing.Executors;
using Kudos.DataBasing.Interfaces.Drivers;
using Kudos.DataBasing.Interfaces.Executors;
using Kudos.Threading.Types;
using MySql.Data.MySqlClient;

namespace Kudos.DataBasing.Drivers
{
    public sealed class
        MySQLDataBaseDriver
    :
        ADataBaseDriver
        <
            MySqlConnection,
            MySqlCommand
        >
    {
        #region ... static ...

        private static EDataBaseType
            __eType;

        static MySQLDataBaseDriver()
        {
            __eType = EDataBaseType.MySQL;
        }

        #endregion

        internal MySQLDataBaseDriver(ref MySqlConnection dbc) : base(ref dbc, ref __eType) { }

        protected override void _OnNewRequestExecutor(ref IDataBaseDriver dbd, ref MySqlCommand? dbc, ref SmartSemaphoreSlim sss, out IDataBaseExecutor dbe)
        {
            dbe = new MySQLDataBaseExecutor(ref dbd, ref dbc, ref sss);
        }
    }
}