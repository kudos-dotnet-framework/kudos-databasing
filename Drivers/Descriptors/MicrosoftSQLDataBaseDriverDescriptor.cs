using System;

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