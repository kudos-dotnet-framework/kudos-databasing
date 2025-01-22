using System;
using System.Data;
using Kudos.Coring.Utils.Numerics;
using Kudos.DataBasing.Drivers.Descriptors;
using Microsoft.Data.SqlClient;

namespace Kudos.DataBasing.Drivers.Builders
{
    public sealed class
        MicrosoftSQLDataBaseDriverBuilder
    :
        ADataBaseDriverBuilder
        <
            MicrosoftSQLDataBaseDriverBuilder,
            MicrosoftSQLDataBaseDriverDescriptor,
            SqlConnectionStringBuilder,
            SqlConnection,
            SqlCommand,
            MicrosoftSQLDataBaseDriver
        >
    {
        internal MicrosoftSQLDataBaseDriverBuilder() : base(new MicrosoftSQLDataBaseDriverDescriptor()) { }

        public MicrosoftSQLDataBaseDriverBuilder SetSource(String? s) { _dsc.Source = s; return this; }

        protected override void _OnConnectionStringBuilderCompletize(ref MicrosoftSQLDataBaseDriverDescriptor dsc, ref SqlConnectionStringBuilder csb)
        {
            if (dsc.UserName != null) csb.UserID = dsc.UserName;
            if (dsc.UserPassword != null) csb.Password = dsc.UserPassword;
            if (dsc.SchemaName != null) csb.InitialCatalog = dsc.SchemaName;
            if (dsc.Source != null) csb.DataSource = dsc.Source;

            csb.Pooling = dsc.HasValidMinimumPoolSize || dsc.HasValidMaximumPoolSize;
            if (dsc.HasValidMinimumPoolSize) csb.MinPoolSize = dsc.MinimumPoolSize.Value;
            if (dsc.HasValidMaximumPoolSize) csb.MaxPoolSize = dsc.MaximumPoolSize.Value;

            if (dsc.ConnectionTimeout != null) csb.ConnectTimeout = Int32Utils.NNParse(dsc.ConnectionTimeout.Value);
            if (dsc.SessionPoolTimeout != null) csb.LoadBalanceTimeout = Int32Utils.NNParse(dsc.SessionPoolTimeout.Value);
            if (dsc.CommandTimeout != null) csb.CommandTimeout = Int32Utils.NNParse(dsc.CommandTimeout.Value);
            if (dsc.IsCompressionEnabled != null) csb.Encrypt = dsc.IsCompressionEnabled.Value;
        }

        protected override void _OnNewDriver(ref SqlConnection c, out MicrosoftSQLDataBaseDriver dbd) { dbd = new MicrosoftSQLDataBaseDriver(ref c); }
    }
}