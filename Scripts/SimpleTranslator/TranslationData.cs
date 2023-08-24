using System;

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

        public TranslationData(string str, Language language)
        {
            switch (language)
            {
                case Language.Eng: eng = str; break;
                case Language.Fra: fra = str; break;
                case Language.Por: por = str; break;
                case Language.Rus: rus = str; break;
                case Language.Spa: spa = str; break;
            }

            eng = str;
        }

        public bool IsNotFullTranslate()
        {
            return string.IsNullOrEmpty(eng)
                || string.IsNullOrEmpty(fra)
                || string.IsNullOrEmpty(por)
                || string.IsNullOrEmpty(rus)
                || string.IsNullOrEmpty(spa);
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
                _ => string.Empty,
            };
        }
    }

    public enum Language { Eng, Fra, Por, Rus, Spa }
}