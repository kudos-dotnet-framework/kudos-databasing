using System;
namespace Kudos.DataBasing.Interfaces.Executors.Actions
{
	public interface
		IDataBaseSetParameterActionExecutor
	{
		void SetParameter(String? s, Object? o);
	}
}

