using System;
using Kudos.Coring.Constants;
using System.Data.Common;
using System.Threading.Tasks;
using Kudos.Coring.Types;

namespace Kudos.DataBasing.Utils
{
    public static class DbConnectionUtils
    {
        public static Exception? Open(DbConnection? dbc)
        {
            if (dbc == null) return CException.ArgumentNullException;
            try { dbc.Open(); return null; }
            catch (Exception exc) { return exc; }
        }

        public static async Task<Exception?> OpenAsync(DbConnection? dbc)
        {
            if (dbc == null) return CException.ArgumentNullException;
            try { await dbc.OpenAsync(); return null; }
            catch (Exception exc) { return exc; }
        }

        public static Exception? Close(DbConnection? dbc)
        {
            if (dbc == null) return CException.ArgumentNullException;
            try { dbc.Close(); return null; }
            catch (Exception exc) { return exc; }
        }

        public static async Task<Exception?> CloseAsync(DbConnection? dbc)
        {
            if (dbc == null) return CException.ArgumentNullException;
            try { await dbc.CloseAsync(); return null; }
            catch (Exception exc) { return exc; }
        }

        public static SmartResult<DbTransaction?> BeginTransaction(DbConnection? dbc)
        {
            if(dbc == null) return SmartResult<DbTransaction?>.ArgumentNullException;
            try { return new SmartResult<DbTransaction?>(dbc.BeginTransaction()); }
            catch (Exception exc) { return new SmartResult<DbTransaction?>(exc); }
        }

        public static async Task<SmartResult<DbTransaction?>> BeginTransactionAsync(DbConnection? dbc)
        {
            if (dbc == null) return SmartResult<DbTransaction?>.ArgumentNullException;
            try { return new SmartResult<DbTransaction?>(await dbc.BeginTransactionAsync()); }
            catch (Exception exc) { return new SmartResult<DbTransaction?>(exc); }
        }

        public static Exception? ChangeDatabase(DbConnection? dbc, String? s)
        {
            if (dbc == null || s == null) return CException.ArgumentNullException;
            try { dbc.ChangeDatabase(s); return null; }
            catch (Exception exc) { return exc; }
        }

        public static async Task<Exception?> ChangeDatabaseAsync(DbConnection? dbc, String? s)
        {
            if (dbc == null || s == null) return CException.ArgumentNullException;
            try { await dbc.ChangeDatabaseAsync(s); return null; }
            catch (Exception exc) { return exc; }
        }

        public static SmartResult<DbCommand?> CreateCommand(DbConnection? dbc)
        {
            if (dbc == null) return SmartResult<DbCommand?>.ArgumentNullException;
            try { return new SmartResult<DbCommand?>(dbc.CreateCommand()); }
            catch (Exception exc) { return new SmartResult<DbCommand?>(exc); }
        }

        public static Exception? Dispose(DbConnection? dbc)
        {
            if (dbc == null) return CException.ArgumentNullException;
            try { dbc.Dispose(); return null; }
            catch (Exception exc) { return exc; }
        }

        public static async Task<Exception?> DisposeAsync(DbConnection? dbc)
        {
            if (dbc == null) return CException.ArgumentNullException;
            try { await dbc.DisposeAsync(); return null; }
            catch (Exception exc) { return exc; }
        }
    }
}