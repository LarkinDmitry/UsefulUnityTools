using System;
using System.Collections.Generic;

namespace KAKuBCE.UsefulUnityTools
{
    [Serializable]
    public class TranslationData
    {
        public string eng;
        public string fra;
        public string por;
        public string rus;
        public string spa;
        public string tur;

        public TranslationData(string str, Language language)
        {
            switch (language)
            {
                case Language.Eng: eng = str; break;
                case Language.Fra: fra = str; break;
                case Language.Por: por = str; break;
                case Language.Rus: rus = str; break;
                case Language.Spa: spa = str; break;
                case Language.Tur: tur = str; break;
            }
        }

        public bool NotTranslatedTo(Language[] languagesCheckList)
        {
            foreach (var language in languagesCheckList)
            {
                if (string.IsNullOrEmpty(GetValue(language))) return true;
            }

            return false;
        }

        public string GetValue(Language language)
        {
            return language switch
            {
                Language.Eng => eng,
                Language.Fra => fra,
                Language.Por => por,
                Language.Rus => rus,
                Language.Spa => spa,
                Language.Tur => tur,
                _ => string.Empty,
            };
        }
    }

    [Serializable]
    public class JsonListWrapper<T>
    {
        public List<T> list;
        public JsonListWrapper(List<T> list) => this.list = list;
    }

    public enum Language { Eng, Fra, Por, Rus, Spa, Tur }
}
