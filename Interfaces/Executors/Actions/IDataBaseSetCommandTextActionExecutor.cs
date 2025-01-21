using System;
namespace Kudos.DataBasing.Interfaces.Executors.Actions
{
	public interface
        IDataBaseSetCommandTextActionExecutor
    {
        IDataBaseSetCommandTextExecutor SetCommandText(String? s);
	}
}

