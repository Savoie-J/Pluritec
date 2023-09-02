using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace P.Web.Business
{
    public static class EnumExtensions
    {
        private static readonly Regex _splitWords = new Regex(@"([a-z])([A-Z])", RegexOptions.Compiled);            //"FooBar" -> "Foo Bar"
        private static readonly Regex _splitTrailingNum = new Regex(@"([A-Za-z])([0-9])", RegexOptions.Compiled);   //"Foo123" -> "Foo 123"
        private static readonly Regex _splitLeadingNum = new Regex(@"([0-9])([A-Za-z])", RegexOptions.Compiled);    //"123Foo" -> "123 Foo"
        private static readonly Regex _splitCaps = new Regex(@"(?<!^)(?<! )([A-Z][a-z])", RegexOptions.Compiled);   //"FOOBar" -> "FOO Bar"

        public static string GetDescription(this Enum enumValue)
        {
            //Look for DescriptionAttributes on the enum field
            object[] attr = enumValue
                .GetType().GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            // a DescriptionAttribute exists; use it
            if (attr.Length > 0)
                return ((DescriptionAttribute)attr[0]).Description;

            // "humanize" an UpperCamelCased Enum identifier
            string result = enumValue.ToString();

            //"FooBar" -> "Foo Bar"
            result = _splitWords.Replace(result, "$1 $2");

            //"Foo123" -> "Foo 123"
            result = _splitTrailingNum.Replace(result, "$1 $2");

            //"123Foo" -> "123 Foo"
            result = _splitLeadingNum.Replace(result, "$1 $2");

            //"FOOBar" -> "FOO Bar"
            result = _splitCaps.Replace(result, " $1");

            return result;
        }
    }
}