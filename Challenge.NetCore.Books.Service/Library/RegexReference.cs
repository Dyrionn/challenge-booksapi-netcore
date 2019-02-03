using System;
using System.Collections.Generic;
using System.Text;

namespace Challenge.NetCore.Books.Service.Library {
    public static class RegexReference
    {
        public static readonly string GetOnlyNumbers = "\\D+";
        public static readonly string TotallyRemoveHtmlTags = "<(.|\n)*?>";
        public static readonly string GetContentBetweenEscapedDoubleQuotes = "\"([^\"]*)\"";
        public static readonly string GetOnlyNumbers3 = "";
        public static readonly string GetOnlyNumbers4 = "";

    }
}
