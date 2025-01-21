using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using Kudos.Coring.Constants;
using Kudos.Coring.Types;
using Kudos.DataBasing.Interfaces.Executors;
using Kudos.DataBasing.Interfaces.Executors.Actions;
using Kudos.DataBasing.Results;
using Kudos.DataBasing.Utils;
using Kudos.Dating.Utils;

namespace Kudos.DataBasing.Executors
{
	internal sealed class
		DataBaseExecutor
    :
		IDataBaseExecutor,
        IDataBaseSetCommandTypeExecutor,
        IDataBaseSetCommandTypeActionExecutor,
        IDataBaseSetCommandTextExecutor,
        IDataBaseSetCommandTextActionExecutor,
        IDataBaseAddParameterExecutor,
        IDataBaseAddParameterActionExecutor,
        IDataBaseExecuteAsNonQueryActionExecutor,
        IDataBaseExecuteAsQueryActionExecutor
    {
        private DbCommand? _cmm;

		internal DataBaseExecutor(ref DbCommand? cmm) { _cmm = cmm; }

        #region IDataBaseSetCommandTypeActionExecutor

        public IDataBaseSetCommandTypeExecutor SetCommandType(CommandType? ct)
        {
            DbCommandUtils.SetCommandType(_cmm, ct);
            return this;
        }

        #endregion

        #region IDataBaseSetCommandTextActionExecutor

        public IDataBaseSetCommandTextExecutor SetCommandText(string? s)
        {
            DbCommandUtils.SetCommandText(_cmm, s);
            return this;
        }

        #endregion

        #region IDataBaseAddParameterActionExecutor

        public IDataBaseAddParameterExecutor AddParameter(string? s, object? o)
        {
            s = DbParameterUtils.PrepareKey(s);
            if (s == null) return this;
            SmartResult<DbParameter?> sr = DbCommandUtils.CreateParameter(_cmm);
            if (!sr.HasValue) return this;
            sr.Value.ParameterName = s;
            sr.Value.Value = o;
            DbCommandUtils.AddParameter(_cmm, sr.Value);
            return this;
        }

        #endregion

        #region IDataBaseExecuteAsNonQueryActionExecutor

        public DataBaseNonQueryResult ExecuteAsNonQuery()
        {
            return null;
        }

        public async Task<DataBaseNonQueryResult> ExecuteAsNonQueryAsync()
        {
            return null;
        }

        #endregion

        #region IDataBaseExecuteAsQueryActionExecutor

        public DataBaseQueryResult ExecuteAsQuery()
        {
            Task<DataBaseQueryResult> tdr = ExecuteAsQueryAsync();
            tdr.Wait();
            return tdr.Result;
        }

        public async Task<DataBaseQueryResult> ExecuteAsQueryAsync()
        {
            DataBaseErrorResult? dber;
            DataTable? dt;

            DataBaseBenchmarkResult
                dbbr =
                    new DataBaseBenchmarkResult()
                        .StartOnExecuting();

            SmartResult<DbDataReader?>
                srdbdr =
                    await DbCommandUtils
                        .ExecuteReaderAsync(_cmm);

            dbbr
                .StopOnExecuting();

            _OnNewDataBaseErrorResult(ref srdbdr, out dber);

            if (dber != null)
            {
                dt = null;
                return new DataBaseQueryResult(ref dt, ref dber, ref dbbr);
            }

            SmartResult<DataTable?>
                srdt =
                    await DataTableUtils
                        .NewAsync(srdbdr.Value);

            _OnNewDataBaseErrorResult(ref srdt, out dber);

            dt = srdt.Value;
            return new DataBaseQueryResult(ref dt, ref dber, ref dbbr);
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

