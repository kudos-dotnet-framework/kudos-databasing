using System;
namespace Kudos.DataBasing.Interfaces.Executors.Actions
{
	public interface
        IDataBaseSetSQLActionExecutor
    {
		IDataBaseSetSQLExecutor SetSQL(String? s);
	}
}

