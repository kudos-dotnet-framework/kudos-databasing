﻿using System;
using Kudos.DataBasing.Interfaces.Executors.Actions;

namespace Kudos.DataBasing.Interfaces.Executors
{
	public interface
		IDataBaseAddParameterExecutor
	:
		IDataBaseAddParameterActionExecutor,
		IDataBaseExecuteAsNonQueryActionExecutor,
		IDataBaseExecuteAsQueryActionExecutor
    {
	}
}