using System;
using Kudos.Coring.Constants;
using System.Data.Common;
using System.Threading.Tasks;
using System.Data;
using Kudos.Coring.Types;

namespace Kudos.DataBasing.Utils
{
	public static class DbCommandUtils
	{
        public static Exception? Dispose(DbCommand? dbc)
        {
            if (dbc == null) return CException.ArgumentNullException;
            try { dbc.Dispose(); return null; }
            catch (Exception exc) { return exc; }
        }

        public static async Task<Exception?> DisposeAsync(DbCommand? dbc)
        {
            if (dbc == null) return CException.ArgumentNullException;
            try { await dbc.DisposeAsync(); return null; }
            catch (Exception exc) { return exc; }
        }

        public static Exception? SetCommandType(DbCommand? dbc, CommandType? ct)
        {
            if (dbc == null || ct == null) return CException.ArgumentNullException;
            try { dbc.CommandType = ct.Value; return null; } catch (Exception exc) { return exc; }
        }

        public static Exception? SetCommandText(DbCommand? dbc, String? s)
        {
            if (dbc == null || s == null) return CException.ArgumentNullException;
            try { dbc.CommandText = s; return null; } catch (Exception exc) { return exc; }
        }

        public static SmartResult<DbParameter?> CreateParameter(DbCommand? dbc)
        {
            if (dbc == null) return SmartResult<DbParameter?>.ArgumentNullException;
            try { return new SmartResult<DbParameter?>(dbc.CreateParameter()); }
            catch (Exception exc) { return new SmartResult<DbParameter?>(exc); }
        }

        public static Exception? AddParameter(DbCommand? dbc, DbParameter? dbp)
        {
            if (dbc == null || dbp == null) return CException.ArgumentNullException;
            try { dbc.Parameters.Add(dbp); return null; }
            catch (Exception exc) { return exc; }
        }

        public static Exception? ClearParameters(DbCommand? dbc)
        {
            if (dbc == null) return CException.ArgumentNullException;
            try { dbc.Parameters.Clear(); return null; }
            catch (Exception exc) { return exc; }
        }

        public static async Task<SmartResult<DbDataReader?>> ExecuteReaderAsync(DbCommand? dbc)
        {
            if (dbc == null) return SmartResult<DbDataReader?>.ArgumentNullException;
            try { return new SmartResult<DbDataReader?>(await dbc.ExecuteReaderAsync()); }
            catch (Exception exc) { return new SmartResult<DbDataReader?>(exc); }
        }
    }
}