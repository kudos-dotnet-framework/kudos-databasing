using Google.Protobuf;
using Kudos.DataBasing.Constants;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Kudos.DataBasing.Results
{
    public sealed class
        DataBaseErrorResult
    {
        internal static /*readonly*/ DataBaseErrorResult?
            Null;

        internal static readonly DataBaseErrorResult
            Empty,
            InternalFailure,
            ImpossibleToBeginTransaction,
            AlreadyInTransaction,
            NotInTransaction,
            CreateCommandFailed,
            ConnectionIsAlreadyOpened,
            ConnectionIsClosed,
            ConnectionIsAlreadyClosed,
            ParameterIsInvalid;

        static DataBaseErrorResult()
        {
            Null = null;

            Empty =
                new DataBaseErrorResult
                (
                    0,
                    String.Empty
                );

            ConnectionIsAlreadyOpened =
                new DataBaseErrorResult
                (
                    CDataBaseErrorCode.ConnectionIsAlreadyOpened,
                    CDataBaseErrorMessage.ConnectionIsAlreadyOpened
                );

            ConnectionIsAlreadyClosed =
                new DataBaseErrorResult
                (
                    CDataBaseErrorCode.ConnectionIsAlreadyClosed,
                    CDataBaseErrorMessage.ConnectionIsAlreadyClosed
                );

            ConnectionIsClosed =
                new DataBaseErrorResult
                (
                    CDataBaseErrorCode.ConnectionIsClosed,
                    CDataBaseErrorMessage.ConnectionIsClosed
                );

            InternalFailure =
                new DataBaseErrorResult
                (
                    CDataBaseErrorCode.InternalFailure,
                    CDataBaseErrorMessage.InternalFailure
                );

            ImpossibleToBeginTransaction =
                new DataBaseErrorResult
                (
                    CDataBaseErrorCode.ImpossibleToBeginTransaction,
                    CDataBaseErrorMessage.ImpossibleToBeginTransaction
                );

            AlreadyInTransaction =
                new DataBaseErrorResult
                (
                    CDataBaseErrorCode.AlreadyInTransaction,
                    CDataBaseErrorMessage.AlreadyInTransaction
                );

            NotInTransaction =
                new DataBaseErrorResult
                (
                    CDataBaseErrorCode.NotInTransaction,
                    CDataBaseErrorMessage.NotInTransaction
                );

            CreateCommandFailed =
                new DataBaseErrorResult
                (
                    CDataBaseErrorCode.CreateCommandFailed,
                    CDataBaseErrorMessage.CreateCommandFailed
                );

            ParameterIsInvalid =
                new DataBaseErrorResult
                (
                    CDataBaseErrorCode.ParameterIsInvalid,
                    CDataBaseErrorMessage.ParameterIsInvalid
                );
        }

        public readonly Int32 ID;
        public readonly String Message;

        internal DataBaseErrorResult(int iID, string sMessage)
        {
            ID = iID;
            Message = sMessage;
        }

        internal DataBaseErrorResult(ref Exception e)
        {
            DbException dbe = e as DbException;

            if (dbe != null)
            {
                ID = dbe.ErrorCode;
                Message = dbe.Message;
                return;
            }

            ID = 0;
            Message = e.Message;
        }
    }
}