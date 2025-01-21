﻿using Kudos.DataBasing.Enums;
using System;
using Kudos.DataBasing.Drivers.Descriptors;
using System.Data.Common;

namespace Kudos.DataBasing.Drivers.Builders
{
    public abstract class
        ADataBaseDriverBuilder
        <
            ThisType,
            DescriptorType,
            ConnectionStringBuilderType,
            ConnectionType,
            DriverType,
            DbType
        >
    where
        ThisType
    :
        ADataBaseDriverBuilder
        <
            ThisType,
            DescriptorType,
            ConnectionStringBuilderType,
            ConnectionType,
            DriverType,
            DbType
        >
    where
        DescriptorType
    :
        ADataBaseDriverDescriptor<DescriptorType>
    where
        ConnectionType
    :
        DbConnection,
        new()
    where
        ConnectionStringBuilderType
    :
        DbConnectionStringBuilder,
        new()
    where
        DriverType
    :
        ADataBaseDriver
        <
            ConnectionType,
            DbType
        >
    where
        DbType
    :
        Enum
    {
        protected /*readonly*/ DescriptorType _dsc;

        public ThisType SetConnectionBehaviour(EDataBaseConnectionBehaviour? edcb) { _dsc.ConnectionBehaviour = edcb; return this as ThisType; }
        public ThisType SetCommandTimeout(uint? i) { _dsc.ConnectionTimeout = i; return this as ThisType; }
        public ThisType SetConnectionTimeout(uint? i) { _dsc.ConnectionTimeout = i; return this as ThisType; }
        public ThisType SetMaximumPoolSize(ushort? i) { _dsc.MaximumPoolSize = i; return this as ThisType; }
        public ThisType SetMinimumPoolSize(ushort? i) { _dsc.MinimumPoolSize = i; return this as ThisType; }
        public ThisType SetSchemaName(string? s) { _dsc.SchemaName = s; return this as ThisType; }
        public ThisType SetSessionPoolTimeout(uint? i) { _dsc.SessionPoolTimeout = i; return this as ThisType; }
        public ThisType SetUserName(string? s) { _dsc.UserName = s; return this as ThisType; }
        public ThisType SetUserPassword(string? s) { _dsc.UserPassword = s; return this as ThisType; }

        public ThisType IsAutomaticCommitEnabled(bool? b) { _dsc.IsAutomaticCommitEnabled = b; return this as ThisType; }
        public ThisType IsCompressionEnabled(bool? b) { _dsc.IsCompressionEnabled = b; return this as ThisType; }
        public ThisType IsLoggingEnabled(bool? b) { _dsc.IsLoggingEnabled = b; return this as ThisType; }

        public DriverType Build()
        {
            ConnectionStringBuilderType csb = new ConnectionStringBuilderType(); _OnConnectionStringBuilderCompletize(ref _dsc, ref csb);
            ConnectionType c = new ConnectionType() { ConnectionString = csb.ToString() };
            DriverType dbd; _OnNewDriver(ref c, out dbd);
            return dbd;
        }

        protected abstract void _OnConnectionStringBuilderCompletize(ref DescriptorType dsc, ref ConnectionStringBuilderType csb);
        protected abstract void _OnNewDriver(ref ConnectionType c, out DriverType dbd);

        internal ADataBaseDriverBuilder(DescriptorType dsc) { _dsc = dsc; }
    }
}