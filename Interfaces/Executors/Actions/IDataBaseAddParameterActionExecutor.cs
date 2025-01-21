using System;
using System.Data;

namespace Kudos.DataBasing.Interfaces.Executors.Actions
{
	public interface
		IDataBaseAddParameterActionExecutor
    {
        IDataBaseAddParameterExecutor AddParameter(String? s, Object? o);
        //IDataBaseAddParameterExecutor AddParameter(String? s, Object? o, ParameterDirection? epd);
    }
}

