using System;
using Kudos.DataBasing.Enums;
using Kudos.DataBasing.Interfaces.Drivers.Builders;

namespace Kudos.DataBasing.Drivers.Descriptors
{
	public sealed class
        MicrosoftSQLDataBaseDriverDescriptor
    :
        ADataBaseDriverDescriptor<MicrosoftSQLDataBaseDriverDescriptor>
    {
        internal String? Source;

        internal MicrosoftSQLDataBaseDriverDescriptor() { }
    }
}