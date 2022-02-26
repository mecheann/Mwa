using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dci.Mnm.Mwa.Core
{
    public partial class AppConst
    {
        public class Operations
        {
            public static string IS_AUTENTICATED;

            static Operations()
            {
                var stringFields = (typeof(Operations)).GetFields().Where(x => x.FieldType == typeof(string));


                foreach (var field in stringFields)
                {
                    field.SetValue(new Operations(), GetActionValue(field));
                }
            }
            public static List<string> GetAll()
            {
                return (typeof(Operations)).GetFields().Where(x => x.FieldType == typeof(string))
                .Select(x => GetActionValue(x)).ToList();
            }

            private static string GetActionValue(FieldInfo x)
            {
                if (String.IsNullOrEmpty(x.GetValue(new Operations()) as string))
                {
                    return x.Name.ToLower().Humanize(LetterCasing.Title);
                }
                else
                {
                    return x.GetValue(new Operations()) as string;
                }
            }

        }
    }
}
