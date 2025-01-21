using System;
namespace Kudos.DataBasing.Constants
{
    internal static class CDataBaseErrorMessage
    {
        internal static readonly String
            ConnectionIsClosed = nameof(ConnectionIsClosed),
            ConnectionIsAlreadyOpened = nameof(ConnectionIsAlreadyOpened),
            ConnectionIsAlreadyClosed = nameof(ConnectionIsAlreadyClosed),
            ParameterIsInvalid = nameof(ParameterIsInvalid),
            AlreadyInTransaction = nameof(AlreadyInTransaction),
            NotInTransaction = nameof(NotInTransaction),
            InternalFailure = nameof(InternalFailure),
            CommandNotInitialized = nameof(CommandNotInitialized);
    }
}

