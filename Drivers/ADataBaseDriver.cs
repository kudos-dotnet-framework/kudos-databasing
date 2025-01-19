using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Kudos.DataBasing.Constants;
using System.Windows.Input;
using Kudos.DataBasing.Drivers.Descriptors;
using Kudos.DataBasing.Enums;
using Kudos.DataBasing.Interfaces.Drivers;
using Kudos.DataBasing.Results;
using Kudos.Threading.Types;
using System.Collections.Generic;
using Kudos.Coring.Constants;
using Kudos.Coring.Utils.Numerics;
using Kudos.DataBasing.Interfaces.Executors;
using Kudos.DataBasing.Interfaces.Executors.Actions;

namespace Kudos.DataBasing.Drivers
{
    public abstract class
        ADataBaseDriver<ConnectionType, CommandType>
    :
        SemaphorizedObject,
        IDataBaseDriver,
        IDataBaseExecutor,
        IDataBaseSetSQLActionExecutor,
        IDataBaseSetSQLExecutor,
        IDataBaseSetParameterActionExecutor,
        IDataBaseExecuteActionExecutor
    where
        ConnectionType
    :
        DbConnection
    where
        CommandType
    :
        DbCommand
    {
        #region ... static ...

        private static Boolean
            __bTrue,
            __bFalse;

        static ADataBaseDriver()
        {
            __bFalse = false;
            __bTrue = true;
        }

        #endregion

        private readonly ConnectionType _cnn;
        private CommandType _cmm;

        public EDataBaseType Type { get; private set; }

        internal ADataBaseDriver
        (
            ref ConnectionType dbc,
            ref EDataBaseType et
        )
        {
            Type = et;
            _cnn = dbc;
        }

        #region IDataBaseDriver

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

            if (!IsConnectionOpened())
            {
                try
                {
                    await _cnn.OpenAsync();
                    dber = null;
                }
                catch (Exception e)
                {
                    dber = new DataBaseErrorResult(ref e);
                }

                if (dber == null)
                {
                    try
                    {
                        _cmm = _cnn.CreateCommand() as CommandType;
                    }
                    catch (Exception e)
                    {
                        dber = new DataBaseErrorResult(ref e);
                    }

                    if
                    (
                        dber == null
                        && _cmm == null
                    )
                        dber = DataBaseErrorResult.CreateCommandFailed;
                }
            }
            else
                dber = DataBaseErrorResult.ConnectionIsAlreadyOpened;

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

            if (!IsConnectionClosed())
            {
                try
                {
                    await _cnn.CloseAsync();
                    dber = null;
                }
                catch (Exception e)
                {
                    dber = new DataBaseErrorResult(ref e);
                }

                try { await _cmm.DisposeAsync(); } catch { }
            }
            else
                dber = DataBaseErrorResult.ConnectionIsAlreadyClosed;

            dbbr
                .StopOnConnecting();

            _ReleaseSemaphore();

            return new DataBaseResult(ref dber, ref dbbr);
        }

        #endregion

        #region public Boolean IsConnection...()

        public Boolean IsConnectionOpening()
        {
            return
                _cnn.State == ConnectionState.Connecting
                && _cmm != null;
        }

        public Boolean IsConnectionOpened()
        {
            return
                (
                    _cnn.State == ConnectionState.Open
                    || _cnn.State == ConnectionState.Fetching
                    || _cnn.State == ConnectionState.Executing
                )
                && _cmm != null;
        }

        public Boolean IsConnectionBroken()
        {
            return
                _cnn.State == ConnectionState.Broken
                || _cmm == null;
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
                try
                {
                    await _cnn.ChangeDatabaseAsync(s);
                    dber = null;
                }
                catch (Exception e)
                {
                    dber = new DataBaseErrorResult(ref e);
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
            return
                _cmm != null
                && _cmm.Transaction != null;
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

            if (!IsConnectionOpened())
                dber = DataBaseErrorResult.ConnectionIsClosed;
            else if (_cmm.Transaction != null)
                dber = DataBaseErrorResult.AlreadyInTransaction;
            else
                try
                {
                    dber = (_cmm.Transaction = await _cnn.BeginTransactionAsync()) != null
                        ? null
                        : DataBaseErrorResult.ImpossibleToBeginTransaction;
                }
                catch (Exception e)
                {
                    dber = new DataBaseErrorResult(ref e);
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

        private async Task<DataBaseResult> _CommitRollbackTransactionAsync(Boolean bCommit)
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
            else if (_cmm.Transaction == null)
                dber = DataBaseErrorResult.NotInTransaction;
            else
            {
                try
                {
                    if (bCommit) await _cmm.Transaction.CommitAsync();
                    else await _cmm.Transaction.RollbackAsync();
                    dber = null;
                }
                catch (Exception e)
                {
                    dber = new DataBaseErrorResult(ref e);
                }

                if (dber == null)
                {
                    try { await _cmm.Transaction.DisposeAsync(); } catch { }
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

        public IDataBaseExecutor RequestQueryExecutor()
        {
            return this;
        }

        public IDataBaseExecutor RequestNonQueryExecutor()
        {
            return this;
        }

        #endregion

        #endregion

        #region IDataBaseExecutor

        public IDataBaseSetSQLExecutor SetSQL(String? s)
        {
            return this;
        }

        #endregion

        #region IDataBaseSetParameterActionExecutor

        public void SetParameter(String? s, Object? o)
        {

        }

        #endregion

        #region IDataBaseExecuteActionExecutor

        public void Execute()
        {

        }

        #endregion

        #region Execute

        #region public ... ExecuteQuery...(...)

        public DataBaseQueryResult ExecuteQuery
        (
            String? s,
            params KeyValuePair<String?, Object?>[]? kvpa
        )
        {
            return ExecuteQuery(s, null, kvpa);
        }
        public async Task<DataBaseQueryResult> ExecuteQueryAsync
        (
            String? s,
            params KeyValuePair<String?, Object?>[]? kvpa
        )
        {
            return await ExecuteQueryAsync(s, null, kvpa);
        }
        public DataBaseQueryResult ExecuteQuery
        (
            String? s,
            Int32? iExpectedRowsNumber,
            params KeyValuePair<String?, Object?>[]? kvpa
        )
        {
            Task<DataBaseQueryResult> t = ExecuteQueryAsync(s, iExpectedRowsNumber, kvpa);
            t.Wait();
            return t.Result;
        }
        public async Task<DataBaseQueryResult> ExecuteQueryAsync
        (
            String? s,
            Int32? iExpectedRowsNumber,
            params KeyValuePair<String?, Object?>[]? kvpa
        )
        {
            DataBaseBenchmarkResult
                dbbr =
                    new DataBaseBenchmarkResult()
                        .StartOnWaiting();

            await _WaitSemaphoreAsync();

            dbbr
                .StopOnWaiting()
                .StartOnPreparing();

            DataBaseErrorResult? dber;
            _PrepareCommand
            (
                ref s,
                ref kvpa,
                out dber
            );

            dbbr
                .StopOnPreparing();

            DataTable? dt;

            if (dber != null)
                dt = null;
            else
            {
                dbbr.StartOnExecuting();

                try
                {
                    DbDataReader dbdr = await _cmm.ExecuteReaderAsync();

                    if (dbdr.HasRows)
                    {
                        dt = new DataTable();

                        if (iExpectedRowsNumber != null && iExpectedRowsNumber > 0)
                            dt.MinimumCapacity = iExpectedRowsNumber.Value;

                        dt.Load(dbdr);
                    }
                    else
                        dt = null;

                    await dbdr.DisposeAsync();
                }
                catch (Exception e)
                {
                    dt = null;
                    dber = new DataBaseErrorResult(ref e);
                }

                dbbr.StopOnExecuting();
            }

            _ReleaseSemaphore();

            return new DataBaseQueryResult(ref dt, ref dber, ref dbbr);
        }

        #endregion

        #region public ... ExecuteNonQuery...(...)

        public DataBaseNonQueryResult ExecuteNonQuery
        (
            String? s,
            params KeyValuePair<String?, Object?>[]? kvpa
        )
        {
            Task<DataBaseNonQueryResult> t = ExecuteNonQueryAsync(s, kvpa);
            t.Wait();
            return t.Result;
        }
        public async Task<DataBaseNonQueryResult> ExecuteNonQueryAsync
        (
            String? s,
            params KeyValuePair<String?, Object?>[]? kvpa
        )
        {
            DataBaseBenchmarkResult
                dbbr =
                    new DataBaseBenchmarkResult()
                        .StartOnWaiting();

            await _WaitSemaphoreAsync();

            dbbr
                .StopOnWaiting()
                .StartOnPreparing();

            DataBaseErrorResult? dber;
            _PrepareCommand
            (
                ref s,
                ref kvpa,
                out dber
            );

            dbbr
                .StopOnPreparing();

            UInt64? lLastInsertedID;
            UInt32? iUpdatedRows;

            if (dber != null)
            {
                lLastInsertedID = null;
                iUpdatedRows = null;
            }
            else
            {
                dbbr.StartOnExecuting();

                try
                {
                    Int32 i = await _cmm.ExecuteNonQueryAsync();
                    iUpdatedRows = i > -1 ? UInt32Utils.NNParse(i) : null;
                    _OnGetLastInsertedID(ref _cmm, out lLastInsertedID);
                }
                catch (Exception e)
                {
                    lLastInsertedID = null;
                    iUpdatedRows = null;
                    dber = new DataBaseErrorResult(ref e);
                }

                dbbr.StopOnExecuting();
            }

            _ReleaseSemaphore();

            return
                new DataBaseNonQueryResult
                (
                    ref lLastInsertedID,
                    ref iUpdatedRows,
                    ref dber,
                    ref dbbr
                );
        }

        #endregion

        #endregion

        protected abstract void _OnGetLastInsertedID(ref CommandType dbc, out UInt64? l);

        #region Command

        private void _PrepareCommand
        (
            ref String? s,
            ref KeyValuePair<String?, Object?>[]? kvpa,
            out DataBaseErrorResult? dber
        )
        {
            if (!IsConnectionOpened())
            {
                dber = DataBaseErrorResult.ConnectionIsClosed;
                return;
            }
            else if (String.IsNullOrWhiteSpace(s))
            {
                dber = DataBaseErrorResult.ParameterIsInvalid;
                return;
            }

            try
            {
                _cmm.CommandType = System.Data.CommandType.Text;
                _cmm.CommandText = s;
                _cmm.Parameters.Clear();
                dber = null;
            }
            catch (Exception e)
            {
                dber = new DataBaseErrorResult(ref e);
            }

            if
            (
                dber != null
                || kvpa == null
            )
                return;

            DbParameter dbpi;
            String si;
            for (int i = 0; i < kvpa.Length; i++)
            {
                if (kvpa[i].Key == null) continue;
                si = kvpa[i].Key.Trim();

                while (si.StartsWith(CCharacter.At))
                    si = si.Substring(1);

                try
                {
                    dbpi = _cmm.CreateParameter();
                    dbpi.ParameterName = si;
                    dbpi.Value = kvpa[i].Value;
                    _cmm.Parameters.Add(dbpi);
                }
                catch (Exception e)
                {
                    dber = new DataBaseErrorResult(ref e);
                    return;
                }
            }
        }

        #endregion

        #region Dispose()

        public void Dispose()
        {
            DisposeAsync().Wait();
        }

        public async Task DisposeAsync()
        {
            if (_cmm != null)
            {
                if (_cmm.Transaction != null) try { await _cmm.Transaction.RollbackAsync(); } catch { }
                try { await _cmm.DisposeAsync(); } catch { }
            }
            if (_cnn != null) try { await _cnn.DisposeAsync(); } catch { }
        }

        #endregion
    }
}