using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KAKuBCE.UsefulUnityTools
{
    public static class StringLocalizator
    {
        public static event Action LanguageIsChanged;

        private static Language developmentLanguage;
        private static Language? selectedLanguage;

        private static List<TranslationData> translationLib;

        public static void Initialization(Language developmentLanguage, bool isNeedLibraryCheck = true)
        {
            StringLocalizator.developmentLanguage = developmentLanguage;

            try
            {
                string jsonData = Resources.Load("translationLibrary").ToString();
                translationLib = JsonConvert.DeserializeObject<List<TranslationData>>(jsonData);
            }
            catch
            {
                translationLib = new();
                UpdateTranslateLib();
            }

            if (isNeedLibraryCheck)
            {
                CheckTranslateData();
            }
        }

        public static void SetTranslateLanguage(Language language)
        {
            selectedLanguage = language;
            LanguageIsChanged?.Invoke();
        }

        public static string Translate(this string str)
        {
            if (selectedLanguage != null)
            {
                return TranslateTo(str, selectedLanguage.Value);
            }
            else
            {
                Debug.LogWarning($"Translate language not set!");
                return str;
            }            
        }

        public static string TranslateTo(this string str, Language language)
        {
            if(string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (translationLib == null)
            {
                Debug.LogWarning($"Translation library missing!");
                return str;
            }

            foreach(var data in translationLib)
            {
                if(Equals(str, data.GetValue(developmentLanguage)))
                {
                    string result = data.GetValue(language);

                    if (string.IsNullOrEmpty(result))
                    {
                        Debug.LogWarning($"\"{str}\" has no translation to Language.{language}");
                        return str;
                    }
                    else
                    {
                        return data.GetValue(language);
                    }
                }
            }

            Debug.LogWarning($"Translation library does not contain a translation for \"{str}\"");
            translationLib.Add(new TranslationData(str, developmentLanguage));
            UpdateTranslateLib();
            return str;
        }

        private static void UpdateTranslateLib()
        {
#if UNITY_EDITOR
            string data = JsonConvert.SerializeObject(translationLib, Formatting.Indented);
            string dataPath = Path.Combine("Assets", "Resources", "translationLibrary.json");            
            File.WriteAllText(dataPath, data);
#endif
        }

        private static void CheckTranslateData()
        {
#if UNITY_EDITOR
            var needTranslateElements = translationLib.Where(e => e.IsNotFullTranslate());

            if (needTranslateElements.Count() != 0)
            {
                Debug.LogWarning($"{needTranslateElements.Count()} element(s) does not have a full translation");
            }
#endif
        }
    }
}