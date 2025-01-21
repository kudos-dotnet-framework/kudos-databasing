using System;
using Kudos.DataBasing.Enums;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Kudos.DataBasing.Utils
{
    internal static class SqlDbTypeUtils
    {
        private static readonly Dictionary<SqlDbType, EDataBaseDataType>
            __d = new Dictionary<SqlDbType, EDataBaseDataType>()
            {
                #region Numerical

                { SqlDbType.TinyInt,    EDataBaseDataType.UnsignedTinyInteger },
                { SqlDbType.SmallInt,   EDataBaseDataType.SmallInteger },
                { SqlDbType.Int,        EDataBaseDataType.Integer },
                { SqlDbType.BigInt,     EDataBaseDataType.BigInteger },
                { SqlDbType.Float,      EDataBaseDataType.Single },
                { SqlDbType.Real,       EDataBaseDataType.Double },
                { SqlDbType.Decimal,    EDataBaseDataType.Decimal },

                #endregion

                #region Textual

                { SqlDbType.Char,       EDataBaseDataType.Char },
                { SqlDbType.VarChar,    EDataBaseDataType.VarChar },
                { SqlDbType.NChar,      EDataBaseDataType.NChar },
                { SqlDbType.NVarChar,   EDataBaseDataType.NVarChar },

                { SqlDbType.Text,       EDataBaseDataType.LongText },
                { SqlDbType.NText,      EDataBaseDataType.LongText },

                #endregion

                #region Bit

                { SqlDbType.Bit,        EDataBaseDataType.Bit }

                #endregion
            };
    }
}

