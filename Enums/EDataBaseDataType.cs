using Kudos.Coring.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kudos.DataBasing.Enums
{
    public enum EDataBaseDataType
    {
        #region Numerical

        TinyInteger,            // 1 byte               MySqlDbType
        UnsignedTinyInteger,    // 1 byte   SqlDbType   MySqlDbType

        SmallInteger,           // 2 byte   SqlDbType   MySqlDbType
        UnsignedSmallInteger,   // 2 byte               MySqlDbType

        MediumInteger,          // 3 byte               MySqlDbType
        UnsignedMediumInteger,  // 3 byte               MySqlDbType

        Integer,                // 4 byte   SqlDbType   MySqlDbType
        UnsignedInteger,        // 4 byte               MySqlDbType

        BigInteger,             // 8 byte   SqlDbType   MySqlDbType
        UnsignedBigInteger,     // 8 byte               MySqlDbType

        Single,                 //          SqlDbType   MySqlDbType
        Double,                 //          SqlDbType   MySqlDbType
        Decimal,                //          SqlDbType   MySqlDbType

        #endregion

        #region Textual

        NChar,                  //          SqlDbType  
        NVarChar,               //          SqlDbType  
        Char,                   //          SqlDbType  
        VarChar,                //          SqlDbType   MySqlDbType

        String,                 //          SqlDbType   MySqlDbType

        TinyText,               // 2^8                  MySqlDbType
        Text,                   // 2^16                 MySqlDbType
        MediumText,             // 2^24                 MySqlDbType
        LongText,               // 2^32                 MySqlDbType

        #endregion

        #region Bit

        Bit

        #endregion

        //Byte,
        //Timestamp,
        //Date,
        //Time,
        //DateTime,
        //Year,
        //Newdate,
        //VarString,
        //Bit,
        //Vector,
        //JSON,
        //NewDecimal,
        //Enum,
        //Set,
        //TinyBlob,
        //MediumBlob,
        //LongBlob,
        //Blob,
        //VarChar,
        //String,
        //Geometry,
        //UByte,
        //Binary,
        //VarBinary,
        //TinyText,
        //MediumText,
        //LongText,
        //Text,
        //Guid
    }



    //public enum EDataBaseDataType
    //{
    //    TinyInteger,
    //    UnsignedTinyInteger,

    //    SmallInteger,
    //    UnsignedSmallInteger,

    //    MediumInteger,
    //    UnsignedMediumInteger,

    //    Integer,
    //    UnsignedInteger,

    //    BigInteger,
    //    UnsignedBigInteger,

    //    Double,
    //    UnsignedDouble,

    //    Boolean,

    //    VariableChar,
    //    Text,
    //    MediumText,
    //    LongText,

    //    Json,

    //    Byte
    //}
}
