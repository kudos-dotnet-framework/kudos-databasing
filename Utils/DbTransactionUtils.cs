using System;
using System.Data.Common;
using System.Threading.Tasks;
using Kudos.Coring.Constants;

namespace Kudos.DataBasing.Utils
{
	public static class DbTransactionUtils
	{
        public static Exception? Commit(DbTransaction? dbt)
        {
            if (dbt == null) return CException.ArgumentNullException;
            try { dbt.Commit(); return null; }
            catch (Exception exc) { return exc; }
        }

        public static async Task<Exception?> CommitAsync(DbTransaction? dbt)
		{
			if (dbt == null) return CException.ArgumentNullException;
			try { await dbt.CommitAsync(); return null; }
			catch (Exception exc) { return exc; }
        }

        public static Exception? Rollback(DbTransaction? dbt)
        {
            if (dbt == null) return CException.ArgumentNullException;
            try { dbt.Rollback(); return null; }
            catch (Exception exc) { return exc; }
        }

        public static async Task<Exception?> RollbackAsync(DbTransaction? dbt)
        {
            if (dbt == null) return CException.ArgumentNullException;
            try { await dbt.RollbackAsync(); return null; }
            catch (Exception exc) { return exc; }
        }

        public static Exception? Dispose(DbTransaction? dbt)
        {
            if (dbt == null) return CException.ArgumentNullException;
            try { dbt.Dispose(); return null; }
            catch (Exception exc) { return exc; }
        }

        public static async Task<Exception?> DisposeAsync(DbTransaction? dbt)
        {
            if (dbt == null) return CException.ArgumentNullException;
            try { await dbt.DisposeAsync(); return null; }
            catch (Exception exc) { return exc; }
        }
    }
}