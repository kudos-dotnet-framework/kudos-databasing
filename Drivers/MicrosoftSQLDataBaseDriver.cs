using System;
using Kudos.Coring.Utils.Numerics;
using Kudos.DataBasing.Drivers.Descriptors;
using Kudos.DataBasing.Enums;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Kudos.DataBasing.Drivers
{
    public sealed class
        MicrosoftSQLDataBaseDriver
    :
        ADataBaseDriver<SqlConnection, SqlCommand>
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

        protected override void _OnGetLastInsertedID(ref SqlCommand dbcmm, out UInt64? l)
        {
            l = null;
        }
    }
}