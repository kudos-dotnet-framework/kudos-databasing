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
            DbConnectionType,
            DbCommandType
        >
    :
        IDataBaseDriver
    where
        DbCommandType
    :
        DbCommand
    where
        DbConnectionType
    :
        DbConnection
    {
        private /*readonly*/ IDataBaseDriver _dbd;
        private /*readonly*/ DbConnectionType _dbcnn;
        private /*readonly*/ DbCommandType? _dbcmm;
        private readonly Boolean _bHasDbCommand;
        private /*readonly*/ SmartSemaphoreSlim _sss;

        public EDataBaseType Type { get; private set; }

        internal ADataBaseDriver
        (
            ref DbConnectionType dbcnn,
            ref EDataBaseType et
        )
        {
            _dbd = this;
            Type = et;
            _dbcnn = dbcnn;
            _sss = new SmartSemaphoreSlim();
            try { _dbcmm = _dbcnn.CreateCommand() as DbCommandType; } catch { }
            _bHasDbCommand = _dbcmm != null;
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
            DataBaseBenchmarkResult dbbr = new DataBaseBenchmarkResult().StartOnWaiting();
            await _sss.WaitSemaphoreAsync();
            dbbr.StopOnWaiting();

            DataBaseErrorResult?
                dber;

            if (!_bHasDbCommand)
                dber = DataBaseErrorResult.CommandNotInitialized;
            else if (IsConnectionOpened())
                dber = DataBaseErrorResult.ConnectionIsAlreadyOpened;
            else
            {
                dbbr.StartOnConnecting();
                Exception? exc = await DbConnectionUtils.OpenAsync(_dbcnn);
                dbbr.StopOnConnecting();
                _OnNewDataBaseErrorResult(ref exc, out dber);
            }

            _sss.ReleaseSemaphore();

            return
                new DataBaseResult(ref dbbr)
                    .Eject(ref dber);
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
            DataBaseBenchmarkResult dbbr = new DataBaseBenchmarkResult().StartOnWaiting();
            await _sss.WaitSemaphoreAsync();
            dbbr.StopOnWaiting();

            DataBaseErrorResult?
                dber;

            if (IsConnectionClosed())
                dber = DataBaseErrorResult.ConnectionIsAlreadyClosed;
            else
            {
                dbbr.StartOnExecuting();
                Exception? exc = await DbConnectionUtils.CloseAsync(_dbcnn);
                dbbr.StopOnExecuting();
                _OnNewDataBaseErrorResult(ref exc, out dber);
            }

            _sss.ReleaseSemaphore();

            return
                new DataBaseResult(ref dbbr)
                    .Eject(ref dber);
        }

        #endregion

        #region public Boolean IsConnection...()

        public Boolean IsConnectionOpening()
        {
            return _dbcnn.State == ConnectionState.Connecting;
        }
        public Boolean IsConnectionOpened()
        {
            return
                _dbcnn.State == ConnectionState.Open
                || _dbcnn.State == ConnectionState.Fetching
                || _dbcnn.State == ConnectionState.Executing;
        }
        public Boolean IsConnectionBroken()
        {
            return
                _dbcnn.State == ConnectionState.Broken;
        }
        public Boolean IsConnectionClosed()
        {
            return
                _dbcnn.State == ConnectionState.Closed;
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
            DataBaseBenchmarkResult dbbr = new DataBaseBenchmarkResult().StartOnWaiting();
            await _sss.WaitSemaphoreAsync();
            dbbr.StopOnWaiting();

            DataBaseErrorResult? dber;

            if (!IsConnectionOpened())
                dber = DataBaseErrorResult.ConnectionIsClosed;
            else if (String.IsNullOrWhiteSpace(s))
                dber = DataBaseErrorResult.ParameterIsInvalid;
            else
            {
                dbbr.StartOnExecuting();
                Exception? exc = await DbConnectionUtils.ChangeDatabaseAsync(_dbcnn, s);
                dbbr.StopOnExecuting();
                _OnNewDataBaseErrorResult(ref exc, out dber);
            }

            _sss.ReleaseSemaphore();

            return
                new DataBaseResult(ref dbbr)
                    .Eject(ref dber);
        }

        #endregion

        #endregion

        #region Transaction

        public Boolean IsIntoTransaction()
        {
            Boolean b;
            _sss.WaitSemaphore();
            b = _bHasDbCommand && _dbcmm.Transaction != null;
            _sss.ReleaseSemaphore();
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
            DataBaseBenchmarkResult dbbr = new DataBaseBenchmarkResult().StartOnWaiting();
            await _sss.WaitSemaphoreAsync();
            dbbr.StopOnWaiting();

            DataBaseErrorResult? dber;

            if (!_bHasDbCommand)
                dber = DataBaseErrorResult.CommandNotInitialized;
            else if (!IsConnectionOpened())
                dber = DataBaseErrorResult.ConnectionIsClosed;
            else if (IsIntoTransaction())
                dber = DataBaseErrorResult.AlreadyInTransaction;
            else
            {
                dbbr.StartOnExecuting();
                SmartResult<DbTransaction?> sr = await DbConnectionUtils.BeginTransactionAsync(_dbcnn);
                dbbr.StopOnExecuting();
                _OnNewDataBaseErrorResult(ref sr, out dber);
                if (dber == null) _dbcmm.Transaction = sr.Value;
            }

            _sss.ReleaseSemaphore();

            return
                new DataBaseResult(ref dbbr)
                    .Eject(ref dber);
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
            DataBaseBenchmarkResult dbbr = new DataBaseBenchmarkResult().StartOnWaiting();
            await _sss.WaitSemaphoreAsync();
            dbbr.StopOnWaiting();

            DataBaseErrorResult? dber;

            if (!IsConnectionOpened())
                dber = DataBaseErrorResult.ConnectionIsClosed;
            else if (!IsIntoTransaction())
                dber = DataBaseErrorResult.NotInTransaction;
            else
            {
                dbbr.StartOnExecuting();

                Exception? exc =
                    await
                    (
                        bIsCommit
                            ? DbTransactionUtils.CommitAsync(_dbcmm.Transaction)
                            : DbTransactionUtils.RollbackAsync(_dbcmm.Transaction)
                    );

                dbbr.StopOnExecuting();

                _OnNewDataBaseErrorResult(ref exc, out dber);

                if(dber == null)
                {
                    await DbTransactionUtils.DisposeAsync(_dbcmm.Transaction);
                    _dbcmm.Transaction = null;
                }
            }

            _sss.ReleaseSemaphore();

            return
                new DataBaseResult(ref dbbr)
                    .Eject(ref dber);
        }

        #endregion

        #region Executor

        public IDataBaseExecutor RequestExecutor()
        {
            IDataBaseExecutor dbe;
            _OnNewRequestExecutor(ref _dbd, ref _dbcmm, ref _sss, out dbe);
            return dbe;
        }

        //public IDataBaseSetCommandTypeExecutor RequestTextExecutor()
        //{
        //    return
        //        RequestExecutor()
        //            .SetCommandType(CommandType.Text);
        //}

        //public IDataBaseSetCommandTypeExecutor RequestStoredProcedureExecutor()
        //{
        //    return
        //        RequestExecutor()
        //            .SetCommandType(CommandType.StoredProcedure);
        //}

        protected abstract void _OnNewRequestExecutor(ref IDataBaseDriver dbd, ref DbCommandType? dbc, ref SmartSemaphoreSlim sss, out IDataBaseExecutor dbe);

        #endregion

        #region Dispose()

        public void Dispose()
        {
            DisposeAsync().Wait();
        }

        public async Task DisposeAsync()
        {
            if (_dbcmm != null)
            {
                await DbTransactionUtils.RollbackAsync(_dbcmm.Transaction);
                await DbTransactionUtils.DisposeAsync(_dbcmm.Transaction);
            }
            await DbCommandUtils.DisposeAsync(_dbcmm);
            await DbConnectionUtils.DisposeAsync(_dbcnn);
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