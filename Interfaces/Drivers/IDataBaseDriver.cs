using System;
using Kudos.DataBasing.Enums;
using Kudos.DataBasing.Interfaces.Executors;
using Kudos.DataBasing.Results;
using System.Threading.Tasks;

namespace Kudos.DataBasing.Interfaces.Drivers
{
	public interface IDataBaseDriver
	{
        EDataBaseType Type { get; }

        DataBaseResult OpenConnection();
        Task<DataBaseResult> OpenConnectionAsync();
        DataBaseResult CloseConnection();
        Task<DataBaseResult> CloseConnectionAsync();

        Boolean IsConnectionOpened();
        Boolean IsConnectionOpening();
        Boolean IsConnectionBroken();
        Boolean IsConnectionClosed();

        DataBaseResult ChangeSchema(String? s);
        Task<DataBaseResult> ChangeSchemaAsync(String? s);

        Boolean IsIntoTransaction();
        Task<DataBaseResult> BeginTransactionAsync();
        DataBaseResult BeginTransaction();
        Task<DataBaseResult> CommitTransactionAsync();
        DataBaseResult CommitTransaction();
        Task<DataBaseResult> RollbackTransactionAsync();
        DataBaseResult RollbackTransaction();

        IDataBaseExecutor RequestExecutor();
        //IDataBaseSetCommandTypeExecutor RequestTextExecutor();
        //IDataBaseSetCommandTypeExecutor RequestStoredProcedureExecutor();
    }
}

