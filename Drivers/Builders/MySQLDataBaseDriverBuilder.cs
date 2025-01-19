using System;
using Kudos.DataBasing.Constants;
using System.Runtime.InteropServices.JavaScript;
using Kudos.DataBasing.Drivers.Descriptors;
using Kudos.DataBasing.Enums;
using Kudos.DataBasing.Interfaces;
using Kudos.DataBasing.Interfaces.Drivers.Builders;
using MySql.Data.MySqlClient;
using Kudos.DataBasing.Interfaces.Drivers;
using Microsoft.Data.SqlClient;
using Kudos.Coring.Utils.Numerics;

namespace Kudos.DataBasing.Drivers.Builders
{
    public sealed class
		MySQLDataBaseDriverBuilder
	:
		ADataBaseDriverBuilder
        <
            MySqlConnection,
            MySqlCommand,
            MySqlConnectionStringBuilder,
            MySQLDataBaseDriverDescriptor,
            MySQLDataBaseDriverBuilder,
            MySQLDataBaseDriver
        >,
        IMySQLDataBaseDriverBuilder
    {
        private readonly MySQLDataBaseDriverDescriptor _dsc;

        public IMySQLDataBaseDriverBuilder IsDnsSrvResolverEnabled(bool? b) { _dsc.IsDnsSrvResolverEnabled = b; return this; }
        public IMySQLDataBaseDriverBuilder IsConnectionResetEnabled(bool? b) { _dsc.IsConnectionResetEnabled = b; return this; }
        public IMySQLDataBaseDriverBuilder IsSessionPoolInteractive(bool? b) { _dsc.IsSessionPoolInteractive = b; return this; }
        public IMySQLDataBaseDriverBuilder SetCharacterSet(EDataBaseCharacterSet? e) { _dsc.CharacterSet = e; return this; }
        public IMySQLDataBaseDriverBuilder SetConnectionProtocol(MySqlConnectionProtocol? e) { _dsc.ConnectionProtocol = e; return this; }
        public IMySQLDataBaseDriverBuilder SetHost(string? s) { _dsc.Host = s; return this; }
        public IMySQLDataBaseDriverBuilder SetKeepAlive(uint? i) { _dsc.KeepAlive = i; return this; }
        public IMySQLDataBaseDriverBuilder SetPort(ushort? i) { _dsc.Port = i; return this; }

        protected override void _OnConnectionStringBuilderCompletize(ref MySQLDataBaseDriverDescriptor dsc, ref MySqlConnectionStringBuilder csb)
        {
            if (dsc.IsLoggingEnabled != null) csb.Logging = dsc.IsLoggingEnabled.Value;
            if (dsc.KeepAlive != null) csb.Keepalive = dsc.KeepAlive.Value;

            if (dsc.Host != null) csb.Server = dsc.Host;
            if (dsc.Port != null) csb.Port = dsc.Port.Value;

            if (dsc.UserName != null) csb.UserID = dsc.UserName;
            if (dsc.UserPassword != null) csb.Password = dsc.UserPassword;
            if (dsc.SchemaName != null) csb.Database = dsc.SchemaName;

            if (dsc.IsDnsSrvResolverEnabled != null) csb.DnsSrv = dsc.IsDnsSrvResolverEnabled.Value;

            csb.Pooling = dsc.HasValidMinimumPoolSize || dsc.HasValidMaximumPoolSize;
            if (dsc.HasValidMinimumPoolSize) csb.MinimumPoolSize = dsc.MinimumPoolSize.Value;
            if (dsc.HasValidMaximumPoolSize) csb.MaximumPoolSize = dsc.MaximumPoolSize.Value;

            if (dsc.CharacterSet != null)
                switch (dsc.CharacterSet)
                {
                    case EDataBaseCharacterSet.utf8:
                        csb.CharacterSet = CDataBaseCharacterSet.utf8;
                        break;
                    case EDataBaseCharacterSet.utf8mb4:
                        csb.CharacterSet = CDataBaseCharacterSet.utf8mb4;
                        break;
                }

            if (dsc.ConnectionTimeout != null) csb.ConnectionTimeout = dsc.ConnectionTimeout.Value;
            if (dsc.SessionPoolTimeout != null) csb.ConnectionLifeTime = dsc.SessionPoolTimeout.Value;
            if (dsc.IsSessionPoolInteractive != null) csb.InteractiveSession = dsc.IsSessionPoolInteractive.Value;
            if (dsc.CommandTimeout != null) csb.DefaultCommandTimeout = dsc.CommandTimeout.Value;
            if (dsc.IsCompressionEnabled != null) csb.UseCompression = dsc.IsCompressionEnabled.Value;
            if (dsc.IsConnectionResetEnabled != null) csb.ConnectionReset = dsc.IsConnectionResetEnabled.Value;

            if (dsc.ConnectionProtocol != null) csb.ConnectionProtocol = dsc.ConnectionProtocol.Value;
        }

        protected override void _OnBuild(ref MySqlConnection c, out MySQLDataBaseDriver dbd)
        {
            dbd = new MySQLDataBaseDriver(ref c);
        }

        internal MySQLDataBaseDriverBuilder() : base(new MySQLDataBaseDriverDescriptor()) { }
    }
}

