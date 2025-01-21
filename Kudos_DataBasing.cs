using System;
using Kudos.DataBasing.Drivers.Builders;

namespace Kudos.DataBasing
{
	public static class KudosDataBasing
	{
        #region public static DataBaseDriverBuilder Request...DataBaseDriverBuilder()

        public static MySQLDataBaseDriverBuilder RequestMySQLDataBaseDriverBuilder() { return new MySQLDataBaseDriverBuilder(); }
        public static MicrosoftSQLDataBaseDriverBuilder RequestMicrosoftSQLDataBaseDriverBuilder() { return new MicrosoftSQLDataBaseDriverBuilder(); }

        #endregion
    }
}