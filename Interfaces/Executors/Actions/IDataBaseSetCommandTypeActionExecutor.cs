using System;
using System.Data;

namespace Kudos.DataBasing.Interfaces.Executors.Actions
{
    public interface
		IDataBaseSetCommandTypeActionExecutor
    {
        IDataBaseSetCommandTypeExecutor SetCommandType(CommandType? ect);
    }
}