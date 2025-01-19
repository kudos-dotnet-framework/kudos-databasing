using System;
using System.Runtime.Intrinsics.X86;
using Kudos.Coring.Constants;
using Kudos.Coring.Utils.Collections;

namespace Kudos.DataBasing.Utils
{
	public static class DataBaseTableUtils
	{
        #region NormalizeNames

        #region private static void __NormalizeNames(...)

        private static void __NormalizeNames(ref String? s, ref String? ssn, ref String? sn)
        {
            if
            (
                s == null
                || !s.Contains(CCharacter.Dot)
            )
                return;

            string[]
                a = s.Split(CCharacter.Dot);

            if (CollectionUtils.IsValidIndex(a, 1))
            {
                ssn = a[0]; sn = a[1];
            }
            else
                ssn = a[0];
        }

        #endregion

        #region public static String?[]? NormalizeNames(...)

        public static String?[]? NormalizeNames(String? sSchemaName, String? sName)
        {
            String? ssn = sSchemaName, sn = sName;
            __NormalizeNames(ref ssn, ref ssn, ref sn);
            __NormalizeNames(ref sn, ref ssn, ref sn);

            return ssn != null || sn != null
                ? new String?[] { ssn, sn }
                : null;
        }

        #endregion

        #endregion
    }
}

