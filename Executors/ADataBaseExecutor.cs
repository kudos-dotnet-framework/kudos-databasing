using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using Kudos.Coring.Constants;
using Kudos.Coring.Types;
using Kudos.Coring.Utils.Numerics;
using Kudos.DataBasing.Constants;
using Kudos.DataBasing.Interfaces.Drivers;
using Kudos.DataBasing.Interfaces.Executors;
using Kudos.DataBasing.Interfaces.Executors.Actions;
using Kudos.DataBasing.Results;
using Kudos.DataBasing.Utils;
using Kudos.Dating.Utils;
using Kudos.Threading.Types;

namespace Kudos.DataBasing.Executors
{
	internal abstract class
		ADataBaseExecutor
        <
            DbCommandType
        >
    :
		IDataBaseExecutor,
        IDataBaseSetCommandTypeExecutor,
        IDataBaseSetCommandTypeActionExecutor,
        IDataBaseSetCommandTextExecutor,
        IDataBaseSetCommandTextActionExecutor,
        IDataBaseAddParameterExecutor,
        IDataBaseAddParameterActionExecutor,
        IDataBaseExecuteNonQueryActionExecutor,
        IDataBaseExecuteScalarActionExecutor,
        IDataBaseExecuteQueryActionExecutor
    where
        DbCommandType
    :
        DbCommand
    {
        private readonly IDataBaseDriver _dbd;
        private /*readonly*/ DbCommandType? _dbc;
        private readonly SmartSemaphoreSlim _sss;

		internal ADataBaseExecutor(ref IDataBaseDriver dbd, ref DbCommandType? dbc, ref SmartSemaphoreSlim sss)
        {
            _dbd = dbd;
            _dbc = dbc;
            _sss = sss;
        }

        #region IDataBaseSetCommandTypeActionExecutor

        public IDataBaseSetCommandTypeExecutor SetCommandType(CommandType? ct)
        {
            DbCommandUtils.SetCommandType(_dbc, ct);
            return this;
        }

        #endregion

        #region IDataBaseSetCommandTextActionExecutor

        public IDataBaseSetCommandTextExecutor SetCommandText(string? s)
        {
            DbCommandUtils.SetCommandText(_dbc, s);
            return this;
        }

        #endregion

        #region IDataBaseAddParameterActionExecutor

        public IDataBaseAddParameterExecutor AddParameter(string? s, object? o)
        {
            s = DbParameterUtils.PrepareKey(s);
            if (s == null) return this;
            SmartResult<DbParameter?> sr = DbCommandUtils.CreateParameter(_dbc);
            if (!sr.HasValue) return this;
            sr.Value.ParameterName = s;
            sr.Value.Value = o;
            DbCommandUtils.AddParameter(_dbc, sr.Value);
            return this;
        }

        #endregion

        #region IDataBaseExecuteNonQueryActionExecutor

        public DataBaseNonQueryResult ExecuteNonQuery()
        {
            Task<DataBaseNonQueryResult> tdr = ExecuteNonQueryAsync();
            tdr.Wait();
            return tdr.Result;
        }

        public async Task<DataBaseNonQueryResult> ExecuteNonQueryAsync()
        {
            DataBaseBenchmarkResult dbbr = new DataBaseBenchmarkResult().StartOnWaiting();
            await _sss.WaitSemaphoreAsync();
            dbbr
                .StopOnWaiting()
                .StartOnExecuting();

            SmartResult<Int32?>
                sri =
                    await DbCommandUtils
                        .ExecuteNonQueryAsync(_dbc);

            dbbr
                .StopOnExecuting();
            
            DataBaseErrorResult? dber;
            _OnNewDataBaseErrorResult(ref sri, out dber);

            DataBaseNonQueryResult
                dbnqr = new DataBaseNonQueryResult(ref dbbr);

            if (dber != null)
            {
                dbnqr.Eject(ref dber);
                goto RELEASE_SEMAPHORE;
            }

            UInt32? iUpdatedRows = UInt32Utils.Parse(sri.Value);
            dbnqr.Eject(ref iUpdatedRows);

            RELEASE_SEMAPHORE:

            CommandType?
                eCommandType;
            String?
                sCommandText;

            if(_dbc != null)
            {
                eCommandType = _dbc.CommandType;
                sCommandText = _dbc.CommandText;
            }
            else
            {
                eCommandType = null;
                sCommandText = null;
            }    

            _sss.ReleaseSemaphore();

            if
            (
                eCommandType != CommandType.Text
                || sCommandText == null
                || !sCommandText.StartsWith(CDataBaseClausole.INSERT, StringComparison.OrdinalIgnoreCase)
            )
                return dbnqr;

            Exception? exc; Object? o;
            _OnFastGetLastInsertedScalar(ref _dbc, out exc, out o);

            if(exc == null)
                return dbnqr.Eject(ref o);
                
            String s;
            _OnGetLastInsertedScalarCommandText(out s);

            DataBaseScalarResult
                dbsr =
                    await
                        _dbd.RequestExecutor()
                            .SetCommandType(CommandType.Text)
                            .SetCommandText(s)
                            .ExecuteScalarAsync();

            if (dbsr.HasError)
            {
                dber = dbsr.Error;
                return dbnqr.Eject(ref dber);
            }

            dbnqr.Benchmark.ElapsedTimeOnConnecting.Add(dbsr.Benchmark.ElapsedTimeOnConnecting);
            dbnqr.Benchmark.ElapsedTimeOnExecuting.Add(dbsr.Benchmark.ElapsedTimeOnExecuting);
            dbnqr.Benchmark.ElapsedTimeOnPreparing.Add(dbsr.Benchmark.ElapsedTimeOnPreparing);
            dbnqr.Benchmark.ElapsedTimeOnWaiting.Add(dbsr.Benchmark.ElapsedTimeOnWaiting);

            o = dbsr.PayLoad;
            dbnqr.Eject(ref o);

            return dbnqr;
        }

        protected abstract void _OnFastGetLastInsertedScalar(ref DbCommandType dbc, out Exception? exc, out Object? o);
        protected abstract void _OnGetLastInsertedScalarCommandText(out String s);

        #endregion

        #region IDataBaseExecuteScalarActionExecutor

        public DataBaseScalarResult ExecuteScalar()
        {
            Task<DataBaseScalarResult> tdr = ExecuteScalarAsync();
            tdr.Wait();
            return tdr.Result;
        }

        public async Task<DataBaseScalarResult> ExecuteScalarAsync()
        {
            DataBaseBenchmarkResult
                dbbr =
                    new DataBaseBenchmarkResult()
                        .StartOnExecuting();

            SmartResult<Object?>
                sr =
                    await DbCommandUtils
                        .ExecuteScalarAsync(_dbc);

            dbbr
                .StopOnExecuting();

            DataBaseErrorResult? dber;
            _OnNewDataBaseErrorResult(ref sr, out dber);

            DataBaseScalarResult
                dbsr = new DataBaseScalarResult(ref dbbr);

            if (dber != null) return dbsr.Eject(ref dber);

            Object? o = sr.Value;

            dbsr.Eject(ref o);

            return dbsr;
        }

        #endregion

        #region IDataBaseExecuteQueryActionExecutor

        public DataBaseQueryResult ExecuteQuery()
        {
            Task<DataBaseQueryResult> tdr = ExecuteQueryAsync();
            tdr.Wait();
            return tdr.Result;
        }

        public async Task<DataBaseQueryResult> ExecuteQueryAsync()
        {
            DataBaseBenchmarkResult dbbr = new DataBaseBenchmarkResult().StartOnWaiting();
            await _sss.WaitSemaphoreAsync();
            dbbr
                .StopOnWaiting()
                .StartOnExecuting();

            SmartResult<DbDataReader?>
                srdbdr =
                    await DbCommandUtils
                        .ExecuteReaderAsync(_dbc);

            dbbr.StopOnExecuting();

            DataBaseErrorResult? dber;
            _OnNewDataBaseErrorResult(ref srdbdr, out dber);

            DataBaseQueryResult
                dbqr = new DataBaseQueryResult(ref dbbr);

            if (dber != null)
            {
                dbqr.Eject(ref dber);
                goto RELEASE_SEMAPHORE;
            }

            dbbr.StartOnPreparing();

            SmartResult<DataTable?>
                srdt =
                    await DataTableUtils
                        .NewAsync(srdbdr.Value);

            dbbr.StopOnPreparing();

            _OnNewDataBaseErrorResult(ref srdt, out dber);

            if (dber != null)
            {
                dbqr.Eject(ref dber);
                goto RELEASE_SEMAPHORE;
            }

            DataTable? dt = srdt.Value;
            dbqr.Eject(ref dt);

            RELEASE_SEMAPHORE:

            _sss.ReleaseSemaphore();

            return dbqr;
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

