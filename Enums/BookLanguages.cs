using System.ComponentModel;

namespace BookstoreTester.Mvc.Enums
{
    public enum BookLanguages
    {
        [Description("en_US")]
        [DefaultValue(BookLanguages.English)]
        English,
        [Description("ru")]
        Russian,
        [Description("ja")]
        Japanese
    }
}