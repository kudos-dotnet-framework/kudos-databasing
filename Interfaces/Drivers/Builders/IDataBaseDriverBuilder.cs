using System;
using System.Data.Common;
using Kudos.DataBasing.Drivers;
using Kudos.DataBasing.Drivers.Builders;
using Kudos.DataBasing.Drivers.Descriptors;
using Kudos.DataBasing.Enums;

namespace Kudos.DataBasing.Interfaces.Drivers.Builders
{
    public interface
        IDataBaseDriverBuilder
        <
            ConnectionType,
            CommandType,
            ConnectionStringBuilderType,
            DescriptorType,
            R,
            DriverType
        >
    where
        DescriptorType
    :
        ADataBaseDriverDescriptor<DescriptorType>
    where
        R
    :
        ADataBaseDriverBuilder
        <
            ConnectionType,
            CommandType,
            ConnectionStringBuilderType,
            DescriptorType,
            R,
            DriverType
        >
    where
        ConnectionType
    :
        DbConnection, new()
    where
        ConnectionStringBuilderType
    :
        DbConnectionStringBuilder, new()
    where
        DriverType
    :
        ADataBaseDriver<ConnectionType, CommandType>
    where
        CommandType
    :
        DbCommand
    {
        R SetSchemaName(String? s);
        R SetUserName(String? s);
        R SetUserPassword(String? s);
        R SetSessionPoolTimeout(UInt32? i);
        R SetConnectionBehaviour(EDataBaseConnectionBehaviour? edcb);
        R SetCommandTimeout(UInt32? i);
        R SetConnectionTimeout(UInt32? i);
        R SetMinimumPoolSize(UInt16? i);
        R SetMaximumPoolSize(UInt16? i);

        R IsCompressionEnabled(Boolean? b);
        R IsAutomaticCommitEnabled(Boolean? b);
        R IsLoggingEnabled(Boolean? b);
    }
}

