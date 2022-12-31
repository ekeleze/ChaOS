using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IL2CPU.API.Attribs;

namespace ChaOS.Misc {
    class Langs {
        [ManifestResourceStream(ResourceName = "ChaOS.ChaOS.Misc.Languages.EN_US.json")]
        static byte[] EN_US_BYTE; public static string EN_US = Convert.ToString(EN_US_BYTE);
    }

    



















    class Test {
        public static void SetLanguage() {
            //dynamic language = JObject.Parse(Langs.EN_US);
            //Console.WriteLine(language.Startup);
        }
    }
}
