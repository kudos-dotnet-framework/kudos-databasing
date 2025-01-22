﻿using System;
using System.Threading.Tasks;
using Kudos.DataBasing.Results;

namespace Kudos.DataBasing.Interfaces.Executors.Actions
{
	public interface
		IDataBaseExecuteQueryActionExecutor
	{
		DataBaseQueryResult ExecuteQuery();
		Task<DataBaseQueryResult> ExecuteQueryAsync();
    }
}

