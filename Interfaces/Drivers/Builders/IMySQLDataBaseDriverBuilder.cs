using System;
using Kudos.DataBasing.Drivers;
using Kudos.DataBasing.Drivers.Builders;
using Kudos.DataBasing.Drivers.Descriptors;
using Kudos.DataBasing.Enums;
using Kudos.DataBasing.Interfaces.Drivers.Builders.Actions;
using MySql.Data.MySqlClient;

namespace Kudos.DataBasing.Interfaces.Drivers.Builders
{
    public interface
        IMySQLDataBaseDriverBuilder
    :
        IDataBaseDriverBuilder
        <
            MySqlConnection,
            MySqlCommand,
            MySqlConnectionStringBuilder,
            MySQLDataBaseDriverDescriptor,
            MySQLDataBaseDriverBuilder,
            MySQLDataBaseDriver
        >
    {
        IMySQLDataBaseDriverBuilder SetCharacterSet(EDataBaseCharacterSet? e);
        IMySQLDataBaseDriverBuilder SetHost(String? s);
        IMySQLDataBaseDriverBuilder SetPort(UInt16? i);
        IMySQLDataBaseDriverBuilder SetKeepAlive(UInt32? i);
        IMySQLDataBaseDriverBuilder SetConnectionProtocol(MySqlConnectionProtocol? e);
        IMySQLDataBaseDriverBuilder IsDnsSrvResolverEnabled(Boolean? b);
        IMySQLDataBaseDriverBuilder IsConnectionResetEnabled(Boolean? b);
        IMySQLDataBaseDriverBuilder IsSessionPoolInteractive(Boolean? b);
    }
}