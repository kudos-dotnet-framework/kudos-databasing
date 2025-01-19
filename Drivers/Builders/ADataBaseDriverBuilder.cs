using Kudos.Coring.Constants;
using Kudos.DataBasing.Enums;
using Kudos.Coring.Reflection.Utils;
using Kudos.Coring.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kudos.DataBasing.Interfaces.Drivers.Builders;
using MySql.Data.MySqlClient;
using Kudos.DataBasing.Interfaces.Drivers.Builders.Actions;
using Kudos.DataBasing.Constants;
using System.Runtime.InteropServices.JavaScript;
using Kudos.DataBasing.Drivers.Descriptors;
using Kudos.DataBasing.Interfaces.Drivers;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Kudos.DataBasing.Drivers.Builders
{
    public abstract class
        ADataBaseDriverBuilder
        <
            ConnectionType,
            CommandType,
            ConnectionStringBuilderType,
            DescriptorType,
            R,
            DriverType
        >
    :
        IDataBaseDriverBuilder
        <
            ConnectionType,
            CommandType,
            ConnectionStringBuilderType,
            DescriptorType,
            R,
            DriverType
        >,
        IDataBaseDriverBuilderBuildAction
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
        CommandType
    :
        DbCommand
    where
        DriverType
    :
        ADataBaseDriver
        <
            ConnectionType,
            CommandType
        >
    {
        protected /*readonly*/ DescriptorType _dsc;

        public R SetConnectionBehaviour(EDataBaseConnectionBehaviour? edcb) { _dsc.ConnectionBehaviour = edcb; return this as R; }
        public R SetCommandTimeout(uint? i) { _dsc.ConnectionTimeout = i; return this as R; }
        public R SetConnectionTimeout(uint? i) { _dsc.ConnectionTimeout = i; return this as R; }
        public R SetMaximumPoolSize(ushort? i) { _dsc.MaximumPoolSize = i; return this as R; }
        public R SetMinimumPoolSize(ushort? i) { _dsc.MinimumPoolSize = i; return this as R; }
        public R SetSchemaName(string? s) { _dsc.SchemaName = s; return this as R; }
        public R SetSessionPoolTimeout(uint? i) { _dsc.SessionPoolTimeout = i; return this as R; }
        public R SetUserName(string? s) { _dsc.UserName = s; return this as R; }
        public R SetUserPassword(string? s) { _dsc.UserPassword = s; return this as R; }

        public R IsAutomaticCommitEnabled(bool? b) { _dsc.IsAutomaticCommitEnabled = b; return this as R; }
        public R IsCompressionEnabled(bool? b) { _dsc.IsCompressionEnabled = b; return this as R; }
        public R IsLoggingEnabled(bool? b) { _dsc.IsLoggingEnabled = b; return this as R; }

        public IDataBaseDriver Build()
        {
            ConnectionStringBuilderType csb = new ConnectionStringBuilderType();
            _OnConnectionStringBuilderCompletize(ref _dsc, ref csb);
            String s = csb.ToString();
            ConnectionType c = new ConnectionType() { ConnectionString = csb.ToString() };
            DriverType dbd;
            _OnBuild(ref c, out dbd);
            return dbd;
        }

        protected abstract void _OnConnectionStringBuilderCompletize(ref DescriptorType dsc, ref ConnectionStringBuilderType csb);
        protected abstract void _OnBuild(ref ConnectionType c, out DriverType dbd);

        internal ADataBaseDriverBuilder(DescriptorType dsc) { _dsc = dsc; }
    }
}