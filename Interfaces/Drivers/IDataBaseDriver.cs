using System;
using Kudos.DataBasing.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kudos.DataBasing.Enums;
using System.Data.Common;
using Kudos.DataBasing.Interfaces.Executors;

namespace Kudos.DataBasing.Interfaces.Drivers
{
    public interface IDataBaseDriver : IDisposable
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

        IDataBaseExecutor RequestQueryExecutor();
        IDataBaseExecutor RequestNonQueryExecutor();

        //DataBaseQueryResult ExecuteQuery(String? s, params KeyValuePair<String, Object?>[]? a);
        //Task<DataBaseQueryResult> ExecuteQueryAsync(String? s, params KeyValuePair<String, Object?>[]? a);
        //DataBaseQueryResult ExecuteQuery(String? s, Int32? iExpectedRowsNumber, params KeyValuePair<String, Object?>[]? a);
        //Task<DataBaseQueryResult> ExecuteQueryAsync(String? s, Int32? iExpectedRowsNumber, params KeyValuePair<String, Object?>[]? a);

        //DataBaseNonQueryResult ExecuteNonQuery(String? s, params KeyValuePair<String, Object?>[]? a);
        //Task<DataBaseNonQueryResult> ExecuteNonQueryAsync(String? s, params KeyValuePair<String, Object?>[]? a);

        //DataBaseTableDescriptor? GetTableDescriptor(String? sTableName);
        //DataBaseTableDescriptor? GetTableDescriptor(String? sSchemaName, String? sTableName);
        //Task<DataBaseTableDescriptor?> GetTableDescriptorAsync(String? sTableName);
        //Task<DataBaseTableDescriptor?> GetTableDescriptorAsync(String? sSchemaName, String? sTableName);

        Task DisposeAsync();
    }
}

