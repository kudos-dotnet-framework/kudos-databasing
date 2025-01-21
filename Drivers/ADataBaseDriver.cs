using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Kudos.DataBasing.Enums;
using Kudos.DataBasing.Results;
using Kudos.Threading.Types;
using System.Collections.Generic;
using Kudos.Coring.Constants;
using Kudos.DataBasing.Interfaces.Executors;
using Kudos.DataBasing.Interfaces.Drivers;
using Kudos.DataBasing.Utils;
using Kudos.Coring.Types;
using Kudos.DataBasing.Executors;

namespace Kudos.DataBasing.Drivers
{
    public abstract class
        ADataBaseDriver
        <
            ConnectionType,
            DbType
        >
    :
        SemaphorizedObject,
        IDataBaseDriver
    where
        ConnectionType
    :
        DbConnection
    where
        DbType
    :
        Enum
    {
        private /*readonly*/ ConnectionType _cnn;
        private /*readonly*/ DbCommand? _cmm;
        private readonly Boolean _bHasDbCommand;

        public EDataBaseType Type { get; private set; }

        internal ADataBaseDriver
        (
            ref ConnectionType cnn,
            ref EDataBaseType et
        )
        {
            Type = et;
            _cnn = cnn;
            try { _cmm = _cnn.CreateCommand(); } catch { }
            _bHasDbCommand = _cmm != null;
        }

        #region Connection

        #region public ... ...DatabaseResult... OpenConnection...()

        public DataBaseResult OpenConnection()
        {
            Task<DataBaseResult> tdr = OpenConnectionAsync();
            tdr.Wait();
            return tdr.Result;
        }

        public async Task<DataBaseResult> OpenConnectionAsync()
        {
            DataBaseBenchmarkResult
                dbbr =
                    new DataBaseBenchmarkResult()
                        .StartOnWaiting();

            await _WaitSemaphoreAsync();

            dbbr
                .StopOnWaiting()
                .StartOnConnecting();

            DataBaseErrorResult?
                dber;

            if (!_bHasDbCommand)
                dber = DataBaseErrorResult.CommandNotInitialized;
            else if (IsConnectionOpened())
                dber = DataBaseErrorResult.ConnectionIsAlreadyOpened;
            else
            {
                Exception? exc = await DbConnectionUtils.OpenAsync(_cnn);
                _OnNewDataBaseErrorResult(ref exc, out dber);
            }

            dbbr
                .StopOnConnecting();

            _ReleaseSemaphore();

            return new DataBaseResult(ref dber, ref dbbr);
        }

        #endregion

        #region public ... ...DatabaseResult... CloseConnection...()

        public DataBaseResult CloseConnection()
        {
            Task<DataBaseResult> tdr = CloseConnectionAsync();
            tdr.Wait();
            return tdr.Result;
        }

        public async Task<DataBaseResult> CloseConnectionAsync()
        {
            DataBaseBenchmarkResult
                dbbr =
                    new DataBaseBenchmarkResult()
                        .StartOnWaiting();

            await _WaitSemaphoreAsync();

            dbbr
                .StopOnWaiting()
                .StartOnConnecting();

            DataBaseErrorResult?
                dber;

            if (IsConnectionClosed())
                dber = DataBaseErrorResult.ConnectionIsAlreadyClosed;
            else
            {
                Exception? exc = await DbConnectionUtils.CloseAsync(_cnn);
                _OnNewDataBaseErrorResult(ref exc, out dber);
            }

            dbbr
                .StopOnConnecting();

            _ReleaseSemaphore();

            return new DataBaseResult(ref dber, ref dbbr);
        }

        #endregion

        #region public Boolean IsConnection...()

        public Boolean IsConnectionOpening()
        {
            return _cnn.State == ConnectionState.Connecting;
        }
        public Boolean IsConnectionOpened()
        {
            return
                _cnn.State == ConnectionState.Open
                || _cnn.State == ConnectionState.Fetching
                || _cnn.State == ConnectionState.Executing;
        }
        public Boolean IsConnectionBroken()
        {
            return
                _cnn.State == ConnectionState.Broken;
        }
        public Boolean IsConnectionClosed()
        {
            return
                _cnn.State == ConnectionState.Closed;
        }

        #endregion

        #endregion

        #region Schema

        #region public ... ...DataBaseResult... ChangeSchema...(...)

        public DataBaseResult ChangeSchema(String? s)
        {
            Task<DataBaseResult> t = ChangeSchemaAsync(s);
            t.Wait();
            return t.Result;
        }
        public async Task<DataBaseResult> ChangeSchemaAsync(String? s)
        {
            DataBaseBenchmarkResult
                dbbr =
                    new DataBaseBenchmarkResult()
                        .StartOnWaiting();

            await _WaitSemaphoreAsync();

            dbbr
                .StopOnWaiting()
                .StartOnExecuting();

            DataBaseErrorResult? dber;

            if (!IsConnectionOpened())
                dber = DataBaseErrorResult.ConnectionIsClosed;
            else if (String.IsNullOrWhiteSpace(s))
                dber = DataBaseErrorResult.ParameterIsInvalid;
            else
            {
                Exception? exc = await DbConnectionUtils.ChangeDatabaseAsync(_cnn, s);
                _OnNewDataBaseErrorResult(ref exc, out dber);
            }

            dbbr
                .StopOnExecuting();

            _ReleaseSemaphore();

            return new DataBaseResult(ref dber, ref dbbr);
        }

        #endregion

        #endregion

        #region Transaction

        public Boolean IsIntoTransaction()
        {
            Boolean b;
            _WaitSemaphore();
            b = _bHasDbCommand && _cmm.Transaction != null;
            _ReleaseSemaphore();
            return b;
        }

        public DataBaseResult BeginTransaction()
        {
            Task<DataBaseResult> t = BeginTransactionAsync();
            t.Wait();
            return t.Result;
        }
        public async Task<DataBaseResult> BeginTransactionAsync()
        {
            DataBaseBenchmarkResult
                dbbr =
                    new DataBaseBenchmarkResult()
                        .StartOnWaiting();

            await _WaitSemaphoreAsync();

            DataBaseErrorResult? dber;

            dbbr
                .StopOnWaiting()
                .StartOnExecuting();

            if (!_bHasDbCommand)
                dber = DataBaseErrorResult.CommandNotInitialized;
            else if (!IsConnectionOpened())
                dber = DataBaseErrorResult.ConnectionIsClosed;
            else if (IsIntoTransaction())
                dber = DataBaseErrorResult.AlreadyInTransaction;
            else
            {
                SmartResult<DbTransaction?> sr = await DbConnectionUtils.BeginTransactionAsync(_cnn);
                _OnNewDataBaseErrorResult(ref sr, out dber);
                if (dber == null) _cmm.Transaction = sr.Value;
            }

            dbbr
                .StopOnExecuting();

            _ReleaseSemaphore();

            return new DataBaseResult(ref dber, ref dbbr);
        }

        public DataBaseResult CommitTransaction()
        {
            Task<DataBaseResult> t = CommitTransactionAsync();
            t.Wait();
            return t.Result;
        }
        public async Task<DataBaseResult> CommitTransactionAsync()
        {
            return await _CommitRollbackTransactionAsync(true);
        }

        public DataBaseResult RollbackTransaction()
        {
            Task<DataBaseResult> t = RollbackTransactionAsync();
            t.Wait();
            return t.Result;
        }
        public async Task<DataBaseResult> RollbackTransactionAsync()
        {
            return await _CommitRollbackTransactionAsync(false);
        }
        private async Task<DataBaseResult> _CommitRollbackTransactionAsync(Boolean bIsCommit)
        {
            DataBaseBenchmarkResult
                dbbr =
                    new DataBaseBenchmarkResult()
                        .StartOnWaiting();

            await _WaitSemaphoreAsync();

            DataBaseErrorResult? dber;

            dbbr
                .StopOnWaiting()
                .StartOnExecuting();

            if (!IsConnectionOpened())
                dber = DataBaseErrorResult.ConnectionIsClosed;
            else if (!IsIntoTransaction())
                dber = DataBaseErrorResult.NotInTransaction;
            else
            {
                Exception? exc =
                    await
                    (
                        bIsCommit
                            ? DbTransactionUtils.CommitAsync(_cmm.Transaction)
                            : DbTransactionUtils.RollbackAsync(_cmm.Transaction)
                    );

                _OnNewDataBaseErrorResult(ref exc, out dber);

                if(dber == null)
                {
                    await DbTransactionUtils.DisposeAsync(_cmm.Transaction);
                    _cmm.Transaction = null;
                }
            }

            dbbr
                .StopOnExecuting();

            _ReleaseSemaphore();

            return new DataBaseResult(ref dber, ref dbbr);
        }

        #endregion

        #region Executor

        public IDataBaseExecutor RequestExecutor() { return new DataBaseExecutor(ref _cmm); }

        #endregion

        protected abstract void _OnGetLastInsertedID(ref CommandType dbc, out UInt64? l);

        #region Dispose()

        public void Dispose()
        {
            DisposeAsync().Wait();
        }

        public async Task DisposeAsync()
        {
            if (_cmm != null)
            {
                await DbTransactionUtils.RollbackAsync(_cmm.Transaction);
                await DbTransactionUtils.DisposeAsync(_cmm.Transaction);
            }
            await DbCommandUtils.DisposeAsync(_cmm);
            await DbConnectionUtils.DisposeAsync(_cnn);
        }

        #endregion

        private void _OnNewDataBaseErrorResult(ref Exception? exc, out DataBaseErrorResult? dber)
        {
            dber = exc != null
                ? new DataBaseErrorResult(ref exc)
                : null;
        }

        private void _OnNewDataBaseErrorResult<T>(ref SmartResult<T> sr, out DataBaseErrorResult? dber)
        {
            Exception? exc = sr.Exception;
            _OnNewDataBaseErrorResult(ref exc, out dber);
        }
    }
}