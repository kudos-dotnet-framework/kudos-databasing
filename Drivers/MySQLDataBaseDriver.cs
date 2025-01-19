using System;
using Kudos.Coring.Utils.Numerics;
using Kudos.DataBasing.Drivers.Descriptors;
using Kudos.DataBasing.Enums;
using Kudos.DataBasing.Results;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Kudos.DataBasing.Drivers
{
    public sealed class
        MySQLDataBaseDriver
    :
        ADataBaseDriver<MySqlConnection, MySqlCommand>
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

        protected override void _OnGetLastInsertedID(ref MySqlCommand dbcmm, out UInt64? l)
        {
            l = dbcmm.LastInsertedId > -1
                ? UInt64Utils.NNParse(dbcmm.LastInsertedId)
                : null;
        }
    }
}