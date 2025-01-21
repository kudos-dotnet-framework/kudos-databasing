using System;
using Kudos.Coring.Constants;

namespace Kudos.DataBasing.Utils
{
	public static class DbParameterUtils
	{
		public static String? PrepareKey(String? s)
		{
            if (s == null) return null;
            s = s.Trim();
            while (s.StartsWith(CCharacter.At)) s = s.Substring(1);
			return s;
        }
	}
}