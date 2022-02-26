using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RT.Comb;
using SharpYaml.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dci.Mnm.Mwa.Core
{
    public static partial class Utility
    {
        public static Dictionary<string, string> GetPropertyValues(this object atype)
        {
            if (atype == null) return new Dictionary<string, string>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(atype, new object[] { });
                dict.Add(prp.Name, value.ToString());
            }
            return dict;
        }

        public static Guid CreateNewIdFromString(string v)
        {
            Guid id;
            using (MD5 hasher = MD5.Create())
            {
                var hashbytes = hasher.ComputeHash(Encoding.Default.GetBytes(v));
                id = new Guid(hashbytes);
            }
            return id;
        }

        public static string ToJson(this object me)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(me);
        }

        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                        else
                        {
                            type.GetEnumName(val);
                        }
                    }
                }
            }

            return null; // could also return string.Empty
        }


        public static String GetInnerMessages(this Exception exception, int? maxDepth = null)
        {
            if (maxDepth == null) maxDepth = 5;

            StringBuilder message = new StringBuilder();
            message.Append(exception.Message);

            if (exception.InnerException != null && maxDepth > 0)
            {
                message.AppendFormat("with Inner Exception: {0}", exception.InnerException.GetInnerMessages(--maxDepth));
                return message.ToString();
            }
            else
            {
                return message.ToString();
            }
        }

        public static async Task<T[]> ReadArrayFromFile<T>(string filePath)
        {
            var textData = await File.ReadAllTextAsync(filePath);
            return JArray.Parse(textData).Select(x => x.ToObject<T>()).ToArray();

        }

        public static class Validator
        {
            public static void ThrowIf(Boolean condition, String message, params string[] messageParams)
            {
                if (condition) throw new MwaException(String.Format(message, messageParams));
            }

            public static void ThrowNoPermissionIf(Boolean condition, String message, params string[] messageParams)
            {
                if (condition) throw new MwaNoPermissonException(String.Format(message, messageParams));
            }

            public static void ThrowSystemIf(Boolean condition, String message, params string[] messageParams)
            {
                if (condition) throw new SystemException(String.Format(message, messageParams));
            }

            public static void ThrowIfStringEmpty(String value, String message, params string[] messageParams)
            {
                Validator.ThrowIf(String.IsNullOrEmpty(value), message, messageParams);
            }

            public static void ThrowIfValueNull(object value, String message, params string[] messageParams)
            {
                Validator.ThrowIf(value == null, message, messageParams);
            }

            public static void ThrowIfStringNotSameIgnoreCase(string right, string left, String message, params string[] messageParams)
            {
                ThrowIf(String.Compare(right, left, true) != 0, message, messageParams);
            }

            public static void ThrowIfStringNotSameCaseSenative(string right, string left, String message, params string[] messageParams)
            {
                ThrowIf(String.Compare(right, left, false) != 0, message, messageParams);
            }

            public static void ThrowIfUserDoesHavePermission(IPrincipal principal, string action, String message = null, params string[] messageParams)
            {
                if (String.IsNullOrEmpty(message))
                {
                    message = $"{principal.UserName()} does not have permission to {action} operation";
                }
                ThrowNoPermissionIf(principal.Cannot(action), message, messageParams);
            }

        }

        public static string JoinUsingSeperator(this IEnumerable<object> objects, string seperator)
        {
            if (objects == null) return String.Empty;

            return String.Join(seperator, objects.Select(x => x.ToString()));
        }

        public static RT.Comb.SqlCombProvider GuidGenerator = new RT.Comb.SqlCombProvider(new RT.Comb.SqlDateTimeStrategy());

        public static Guid CreateNewId()
        {
            return GuidGenerator.Create();
        }

        public static DateTime GetLastDateOfMonthDate(this DateTime inputDate)
        {
            var lastDateOfmonth = DateTime.DaysInMonth(inputDate.Year, inputDate.Month);
            return new DateTime(inputDate.Year,
                                inputDate.Month,
                                lastDateOfmonth,
                                inputDate.Hour,
                                inputDate.Minute,
                                inputDate.Second,
                                inputDate.Millisecond);
        }

        public static int GetLastDateOfMonth(this DateTime inputDate)
        {
            return DateTime.DaysInMonth(inputDate.Year, inputDate.Month);
        }

        public static int GenerateRandomNumber(int minValue = 1000, int maxValue = 9999)
        {
            var random = new Random();
            return random.Next(minValue, maxValue);
        }

        public static string GetSafeFileName(string fileName)
        {
            return string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
        }
        public static string ToJsonString(this object obj, params JsonConverter[] converters)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace });
        }

        public static string ToXmlString(this object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        public static T FromJsonString<T>(this string jsonString, params JsonConverter[] converters)
        {
            return JsonConvert.DeserializeObject<T>(jsonString, converters);
        }

        public static T FromXmlString<T>(this string xmlString)
            where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            var stringReader = new StringReader(xmlString);
            return serializer.Deserialize(stringReader) as T;
        }

        public static TObject Clone<TObject>(this TObject obj, params JsonConverter[] converters)
        {
            return JsonConvert.DeserializeObject<TObject>(obj.ToJsonString(converters), converters);
        }

        public static JObject ToJsonObject(this object obj)
        {
            if (obj == null) return null;

            var jObject = JObject.FromObject(obj);
            jObject.Add("_Type_", obj.GetType().Name);

            return jObject;
        }

        public static string ToYamlString(this object obj)
        {
            var serializer = new Serializer(new SerializerSettings { EmitTags = false });
            var yaml = serializer.Serialize(obj);

            return yaml;
        }

        public static string ToPropertyValueString(this object theObject, int indentSpaces = 2, bool ignoreNullValues = false)
        {
            string GenerateIndentString(int theLevel) =>
                    new string(' ', theLevel * indentSpaces);

            var builder = new StringBuilder();
            var visitedObjects = new List<object>();

            void BuildPropertyValueString(object obj, int level = 0)
            {
                if (obj == null) return;

                visitedObjects.Add(obj);

                string indentString = GenerateIndentString(level);

                Type objType = obj.GetType();
                PropertyInfo[] properties = objType.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    object propValue = property.GetValue(obj, null);

                    if (visitedObjects.Contains(propValue)) continue;

                    if (ignoreNullValues && propValue == null) continue;

                    if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                    {
                        builder.AppendLine($"{indentString}{property.Name}: {propValue}");
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                    {
                        builder.AppendLine($"{indentString}{property.Name}:");

                        if (propValue == null) continue;

                        IEnumerable enumerable = (IEnumerable)propValue;

                        int itemNumber = 1;
                        string childIndentString = GenerateIndentString(level + 1);

                        foreach (object item in enumerable)
                        {
                            builder.AppendLine($"{childIndentString}{itemNumber.ToString()}:");
                            BuildPropertyValueString(item, level + 2);
                            itemNumber++;
                        }
                    }
                    else
                    {
                        builder.AppendLine($"{indentString}{property.Name}:");
                        BuildPropertyValueString(propValue, level + 1);
                    }
                }
            }

            BuildPropertyValueString(theObject);

            return builder.ToString();
        }
    }
}
