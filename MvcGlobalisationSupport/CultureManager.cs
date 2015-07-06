using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;

namespace MvcGlobalisationSupport
{
    public static class CultureManager
    {        
        const string UnitedStatesCultureName = "en-US";
        const string TurkishCultureName = "tr";
        const string SwidishCultureName = "se";
        const string SpanishCultureName = "es";
        const string RussianCultureName = "ru";
        const string PortugueseCultureName = "pt";
        const string PolishCultureName = "pl";
        const string JapaneseCultureName = "ja";
        const string ItalianCultureName = "it";
        const string GermanCultureName = "de";
        const string FrenchCultureName = "fr";
        const string EnglishCultureName = "en";
        const string DutchCultureName = "nl";
        const string DanishCultureName = "da";

        static CultureInfo DefaultCulture
        {
            get
            {
                return SupportedCultures[EnglishCultureName];
            }
        }

        static Dictionary<string, CultureInfo> SupportedCultures { get; set; }


        static void AddSupportedCulture(string name)
        {
            SupportedCultures.Add(name, CultureInfo.CreateSpecificCulture(name));
        }

        static void InitializeSupportedCultures()
        {
            SupportedCultures = new Dictionary<string, CultureInfo>();
            AddSupportedCulture(UnitedStatesCultureName);
            AddSupportedCulture(TurkishCultureName);
            AddSupportedCulture(SwidishCultureName);
            AddSupportedCulture(SpanishCultureName);
            AddSupportedCulture(RussianCultureName);
            AddSupportedCulture(PortugueseCultureName);
            AddSupportedCulture(PolishCultureName);
            AddSupportedCulture(JapaneseCultureName);
            AddSupportedCulture(ItalianCultureName);
            AddSupportedCulture(GermanCultureName);
            AddSupportedCulture(FrenchCultureName);
            AddSupportedCulture(EnglishCultureName);
            AddSupportedCulture(DutchCultureName);
            AddSupportedCulture(DanishCultureName);
        }

        static string ConvertToShortForm(string code)
        {
            return code.Substring(0, 2);
        }

        static bool CultureIsSupported(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;
            code = code.ToLowerInvariant();
            if (code.Length == 2)
                return SupportedCultures.ContainsKey(code);
            return CultureFormatChecker.FormattedAsCulture(code) && SupportedCultures.ContainsKey(ConvertToShortForm(code));
        }

        static CultureInfo GetCulture(string code)
        {
            if (!CultureIsSupported(code))
                return DefaultCulture;
            string shortForm = ConvertToShortForm(code).ToLowerInvariant(); ;
            return SupportedCultures[shortForm];
        }

        public static void SetCulture(string code)
        {
            CultureInfo cultureInfo = GetCulture(code);
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
        }

        static CultureManager()
        {
            InitializeSupportedCultures();
        }
    }
}
