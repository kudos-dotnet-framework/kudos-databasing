using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kudos.DataBasing.Constants
{
    internal static class CDataBaseErrorCode
    {
        private static readonly Int32
            __i = 100000;

        internal static readonly Int32
            ConnectionIsClosed = __i,
            ConnectionIsAlreadyClosed = __i + 1,
            ConnectionIsAlreadyOpened = __i + 2,
            ParameterIsInvalid = __i + 3,
            AlreadyInTransaction = __i + 4,
            NotInTransaction = __i + 5,
            InternalFailure = __i + 6,
            CommandNotInitialized = __i + 7;
    }
}