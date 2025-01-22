using Kudos.DataBasing.Constants;
using Kudos.DataBasing.Drivers.Descriptors;
using Kudos.DataBasing.Enums;
using MySql.Data.MySqlClient;

namespace Kudos.DataBasing.Drivers.Builders
{
    public sealed class
        MySQLDataBaseDriverBuilder
	:
		ADataBaseDriverBuilder
        <
            MySQLDataBaseDriverBuilder,
            MySQLDataBaseDriverDescriptor,
            MySqlConnectionStringBuilder,
            MySqlConnection,
            MySqlCommand,
            MySQLDataBaseDriver
        >
    {
        internal MySQLDataBaseDriverBuilder() : base(new MySQLDataBaseDriverDescriptor()) { }

        public MySQLDataBaseDriverBuilder IsDnsSrvResolverEnabled(bool? b) { _dsc.IsDnsSrvResolverEnabled = b; return this; }
        public MySQLDataBaseDriverBuilder IsConnectionResetEnabled(bool? b) { _dsc.IsConnectionResetEnabled = b; return this; }
        public MySQLDataBaseDriverBuilder IsSessionPoolInteractive(bool? b) { _dsc.IsSessionPoolInteractive = b; return this; }
        public MySQLDataBaseDriverBuilder SetCharacterSet(EDataBaseCharacterSet? e) { _dsc.CharacterSet = e; return this; }
        public MySQLDataBaseDriverBuilder SetConnectionProtocol(MySqlConnectionProtocol? e) { _dsc.ConnectionProtocol = e; return this; }
        public MySQLDataBaseDriverBuilder SetHost(string? s) { _dsc.Host = s; return this; }
        public MySQLDataBaseDriverBuilder SetKeepAlive(uint? i) { _dsc.KeepAlive = i; return this; }
        public MySQLDataBaseDriverBuilder SetPort(ushort? i) { _dsc.Port = i; return this; }

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

        protected override void _OnNewDriver(ref MySqlConnection c, out MySQLDataBaseDriver dbd) { dbd = new MySQLDataBaseDriver(ref c); }
    }
}

