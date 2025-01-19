using System;
using System.Security.Cryptography;
using Kudos.Coring.Reflection.Utils;
using Kudos.Coring.Utils;
using Kudos.DataBasing.Enums;
using MySql.Data.MySqlClient;

namespace Kudos.DataBasing.Drivers.Descriptors
{
	public abstract class
        ADataBaseDriverDescriptor<R>
    where
        R : ADataBaseDriverDescriptor<R>
	{
        internal String? UserName, UserPassword, SchemaName;
        internal UInt32? CommandTimeout, ConnectionTimeout, SessionPoolTimeout;

        #region MinimumPoolSize

        private UInt16? _iMinimumPoolSize;
        internal Boolean HasValidMinimumPoolSize { get; private set; }
        internal UInt16? MinimumPoolSize
        {
            get { return _iMinimumPoolSize; }
            set { HasValidMinimumPoolSize = (_iMinimumPoolSize = value) != null && _iMinimumPoolSize > 0; }
        }

        #endregion

        #region MaximumPoolSize

        private UInt16? _iMaximumPoolSize;
        internal Boolean HasValidMaximumPoolSize { get; private set; }
        internal UInt16? MaximumPoolSize
        {
            get { return _iMaximumPoolSize; }
            set { HasValidMaximumPoolSize = (_iMaximumPoolSize = value) != null && _iMaximumPoolSize > 0; }
        }

        #endregion

        internal Boolean? IsAutomaticCommitEnabled, IsCompressionEnabled, IsLoggingEnabled;
        internal EDataBaseConnectionBehaviour? ConnectionBehaviour;

        internal ADataBaseDriverDescriptor() { }
    }
}

