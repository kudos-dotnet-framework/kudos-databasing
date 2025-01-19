using System;
using Kudos.DataBasing.Drivers;
using Kudos.DataBasing.Drivers.Builders;
using Kudos.DataBasing.Drivers.Descriptors;
using Kudos.DataBasing.Interfaces.Drivers.Builders.Actions;
using Microsoft.Data.SqlClient;

namespace Kudos.DataBasing.Interfaces.Drivers.Builders
{
    public interface
        IMicrosoftSQLDataBaseDriverBuilder
    :
        IDataBaseDriverBuilder
        <
            SqlConnection,
            SqlCommand,
            SqlConnectionStringBuilder,
            MicrosoftSQLDataBaseDriverDescriptor,
            MicrosoftSQLDataBaseDriverBuilder,
            MicrosoftSQLDataBaseDriver
        >
    {
        IMicrosoftSQLDataBaseDriverBuilder SetSource(String? s);
    }
}