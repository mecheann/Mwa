using HandlebarsDotNet;
using System;
using System.Net;

namespace Dci.Mnm.Mwa.Infrastructure
{
    public static class HandlebarsHelpers
    {
        public static void RegisterHelpers()
        {
            Handlebars.RegisterHelper("formatDate", (writer, context, args) =>
            {
                if (args.Length >= 1)
                {
                    var dateString = args[0].ToString();
                    string format = null;
                    if (args.Length >= 2)
                    {
                        format = args[1].ToString();
                    }

                    if (DateTimeOffset.TryParse(dateString, out DateTimeOffset dateOffet))
                    {
                        var formattedDateString = format != null ? dateOffet.ToString(format) : dateOffet.Date.ToShortDateString();
                        writer.WriteSafeString(formattedDateString);
                    }

                    else if (DateTime.TryParse(dateString, out DateTime date))
                    {
                        var formattedDateString = format != null ? date.ToString(format) : date.ToShortDateString();
                        writer.WriteSafeString(formattedDateString);
                    }
                }
            });

            Handlebars.RegisterHelper("formatNumber", (writer, context, args) =>
            {
                if (args.Length >= 1)
                {
                    var numberString = args[0].ToString();
                    if (args.Length >= 2)
                    {
                        var format = args[1].ToString();

                        if (Decimal.TryParse(numberString, out Decimal number))
                        {
                            var formattedNumberString = number.ToString(format);
                            writer.WriteSafeString(formattedNumberString);
                        }
                    }
                }
            });

            Handlebars.RegisterHelper("yaml", (writer, context, args) =>
            {
                if (args.Length >= 1)
                {
                    var obj = args[0];

                    if (obj != null)
                    {
                        //writer.WriteSafeString(WebUtility.HtmlEncode(obj.ToYamlString()));
                    }
                }
            });

            Handlebars.RegisterHelper("propertyValueString", (writer, context, args) =>
            {
                if (args.Length >= 1)
                {
                    var obj = args[0];

                    if (obj != null)
                    {
                        //writer.WriteSafeString(WebUtility.HtmlEncode(obj.ToPropertyValueString(4)));
                    }
                }
            });
        }
    }
}









