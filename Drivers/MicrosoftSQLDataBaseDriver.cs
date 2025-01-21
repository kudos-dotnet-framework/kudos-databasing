using System;
using System.Data;
using Kudos.DataBasing.Enums;
using Kudos.DataBasing.Interfaces.Executors;
using Microsoft.Data.SqlClient;

namespace Kudos.DataBasing.Drivers
{
    public sealed class
        MicrosoftSQLDataBaseDriver
    :
        ADataBaseDriver
        <
            SqlConnection,
            SqlDbType
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

        protected override void _OnGetLastInsertedID(ref CommandType dbc, out ulong? l)
        {
            throw new NotImplementedException();
        }
    }
}