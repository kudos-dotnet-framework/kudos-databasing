using System;
using Kudos.DataBasing.Drivers.Builders;
using Kudos.DataBasing.Drivers.Descriptors;
using Kudos.DataBasing.Interfaces.Drivers.Builders;

namespace Kudos.DataBasing
{
	public static class DataBase
	{
        #region public static IMySQLDataBaseDriverBuilder Request...DriverBuilder()

        public static IMySQLDataBaseDriverBuilder RequestMySQLDriverBuilder() { return new MySQLDataBaseDriverBuilder(); }
        public static IMicrosoftSQLDataBaseDriverBuilder RequestMicrosoftSQLDriverBuilder() { return new MicrosoftSQLDataBaseDriverBuilder(); }

        #endregion
    }
}