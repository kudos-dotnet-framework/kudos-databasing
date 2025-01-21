using System;
using System.Data;
using Kudos.DataBasing.Enums;
using Kudos.DataBasing.Executors;
using Kudos.DataBasing.Interfaces.Executors;
using MySql.Data.MySqlClient;

namespace Kudos.DataBasing.Drivers
{
    public sealed class
        MySQLDataBaseDriver
    :
        ADataBaseDriver
        <
            MySqlConnection,
            MySqlDbType
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

        protected override void _OnGetLastInsertedID(ref CommandType dbc, out ulong? l)
        {
            throw new NotImplementedException();
        }
    }
}