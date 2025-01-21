using System;
using Kudos.DataBasing.Enums;
using MySql.Data.MySqlClient;

namespace Kudos.DataBasing.Drivers.Descriptors
{
	public sealed class
        MySQLDataBaseDriverDescriptor
    :
        ADataBaseDriverDescriptor<MySQLDataBaseDriverDescriptor>
    {
        internal String? Host;
        internal EDataBaseCharacterSet? CharacterSet;
        internal Boolean? IsSessionPoolInteractive, IsConnectionResetEnabled, IsDnsSrvResolverEnabled;
        internal UInt16? Port;
        internal UInt32? KeepAlive;
        internal MySqlConnectionProtocol? ConnectionProtocol;

        internal MySQLDataBaseDriverDescriptor() { }
    }
}