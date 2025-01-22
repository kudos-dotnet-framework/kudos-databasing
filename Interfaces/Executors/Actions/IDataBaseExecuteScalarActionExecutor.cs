using System;
using Kudos.DataBasing.Results;
using System.Threading.Tasks;

namespace Kudos.DataBasing.Interfaces.Executors.Actions
{
    public interface
        IDataBaseExecuteScalarActionExecutor
    {
        DataBaseScalarResult ExecuteScalar();
        Task<DataBaseScalarResult> ExecuteScalarAsync();
    }
}

