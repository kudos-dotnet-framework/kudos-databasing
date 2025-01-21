using System;
using Kudos.Coring.Utils.Numerics;
using Kudos.DataBasing.Enums;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;

namespace Kudos.DataBasing.Utils
{
    internal static class MySqlDbTypeUtils
    {
        private static readonly Dictionary<MySqlDbType, EDataBaseDataType>
            __d = new Dictionary<MySqlDbType, EDataBaseDataType>()
            {
                #region Numerical

                { MySqlDbType.Byte,         EDataBaseDataType.TinyInteger },
                { MySqlDbType.UByte,        EDataBaseDataType.UnsignedTinyInteger },
                { MySqlDbType.Int16,        EDataBaseDataType.SmallInteger },
                { MySqlDbType.UInt16,       EDataBaseDataType.UnsignedSmallInteger },
                { MySqlDbType.Int24,        EDataBaseDataType.MediumInteger },
                { MySqlDbType.UInt24,       EDataBaseDataType.UnsignedMediumInteger },
                { MySqlDbType.Int32,        EDataBaseDataType.Integer },
                { MySqlDbType.UInt32,       EDataBaseDataType.UnsignedInteger },
                { MySqlDbType.Int64,        EDataBaseDataType.BigInteger },
                { MySqlDbType.UInt64,       EDataBaseDataType.UnsignedBigInteger },
                { MySqlDbType.Float,        EDataBaseDataType.Single },
                { MySqlDbType.Double,       EDataBaseDataType.Double },
                { MySqlDbType.Decimal,      EDataBaseDataType.Decimal },

                #endregion

                #region Textual

                { MySqlDbType.VarChar,      EDataBaseDataType.VarChar },
                { MySqlDbType.VarString,    EDataBaseDataType.VarChar },
                { MySqlDbType.String,       EDataBaseDataType.String },

                { MySqlDbType.TinyText,     EDataBaseDataType.TinyText },
                { MySqlDbType.Text,         EDataBaseDataType.Text },
                { MySqlDbType.MediumText,   EDataBaseDataType.MediumText },
                { MySqlDbType.LongText,     EDataBaseDataType.LongText },

                #endregion

                #region Bit

                { MySqlDbType.Bit,           EDataBaseDataType.Bit },

                #endregion
            };
    }
}

