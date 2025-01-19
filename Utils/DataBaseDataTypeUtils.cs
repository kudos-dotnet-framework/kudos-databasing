using Kudos.Coring.Constants;
using Kudos.DataBasing.Enums;
using Kudos.Coring.Utils.Numerics;
using System;
using System.Collections.Generic;

namespace Kudos.DataBasing.Utils
{
    internal static class DataBaseDataTypeUtils
    {
        private static readonly Int32
           __iInt8__MinValue = -128,
           __iInt8__MaxValue = 127,
           __iInt24_MinValue = -8388607,
           __iInt24_MaxValue = 8388607;

        private static readonly UInt32
           __iUInt8__MaxValue = 255,
           __iUInt24_MaxValue = UInt32Utils.NNParse(Math.Pow(2, 24)) - 1;

        private static readonly Dictionary<EDataBaseDataType, Double>
            __dEnums2MinValues = new Dictionary<EDataBaseDataType, Double>()
            {
                { EDataBaseDataType.UnsignedTinyInteger, UInt16.MinValue },
                { EDataBaseDataType.TinyInteger, __iInt8__MinValue },
                { EDataBaseDataType.UnsignedSmallInteger, UInt16.MinValue },
                { EDataBaseDataType.SmallInteger, Int16.MinValue },
                { EDataBaseDataType.UnsignedMediumInteger, UInt16.MinValue },
                { EDataBaseDataType.MediumInteger, __iInt24_MinValue },
                { EDataBaseDataType.UnsignedInteger, UInt32.MinValue },
                { EDataBaseDataType.Integer, Int32.MinValue },
                { EDataBaseDataType.UnsignedBigInteger, UInt64.MinValue },
                { EDataBaseDataType.BigInteger, Int64.MinValue },
                { EDataBaseDataType.UnsignedDouble, UInt64.MinValue },
                { EDataBaseDataType.Double, Double.MinValue }
            };

        private static readonly Dictionary<EDataBaseDataType, Double>
            __dEnums2MaxValues = new Dictionary<EDataBaseDataType, Double>()
            {
                { EDataBaseDataType.UnsignedTinyInteger, __iUInt8__MaxValue },
                { EDataBaseDataType.TinyInteger, __iInt8__MaxValue },
                { EDataBaseDataType.UnsignedSmallInteger, UInt16.MaxValue },
                { EDataBaseDataType.SmallInteger, Int16.MaxValue },
                { EDataBaseDataType.UnsignedMediumInteger, __iUInt24_MaxValue },
                { EDataBaseDataType.MediumInteger, __iInt24_MaxValue },
                { EDataBaseDataType.UnsignedInteger, UInt32.MaxValue },
                { EDataBaseDataType.Integer, Int32.MaxValue },
                { EDataBaseDataType.UnsignedBigInteger, UInt64.MaxValue },
                { EDataBaseDataType.BigInteger, Int64.MaxValue },
                { EDataBaseDataType.UnsignedDouble, Double.MaxValue },
                { EDataBaseDataType.Double, Double.MaxValue }
            };

        internal static Double? GetMinValue(ref EDataBaseDataType e)
        {
            Double d;
            return
                __dEnums2MinValues.TryGetValue(e, out d)
                    ? d
                    : null;
        }

        internal static Double? GetMaxValue(ref EDataBaseDataType e)
        {
            Double d;
            return
                __dEnums2MaxValues.TryGetValue(e, out d)
                    ? d
                    : null;
        }

        public static EDataBaseDataType? GetEnum(Type? t)
        {
            if (t == CType.String)
                return EDataBaseDataType.VariableChar;
            else if (t == CType.Int16)
                return EDataBaseDataType.SmallInteger;
            else if (t == CType.UInt16)
                return EDataBaseDataType.UnsignedSmallInteger;
            else if (t == CType.Int32)
                return EDataBaseDataType.Integer;
            else if (t == CType.UInt32)
                return EDataBaseDataType.UnsignedInteger;
            else if (t == CType.Int64)
                return EDataBaseDataType.BigInteger;
            else if (t == CType.UInt64)
                return EDataBaseDataType.UnsignedBigInteger;
            else if (t == CType.Double)
                return EDataBaseDataType.Double;
            else if (t == CType.Decimal)
                return EDataBaseDataType.Double;
            else if (t == CType.Single)
                return EDataBaseDataType.Double;
            else if (t == CType.Boolean)
                return EDataBaseDataType.Boolean;
            else if (t == CType.Object)
                return EDataBaseDataType.Json;
            else
                return null;
        }
    }
}
