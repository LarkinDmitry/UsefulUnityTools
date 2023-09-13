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

        private static Language? developmentLanguage;
        private static Language? selectedLanguage;
        private static List<TranslationData> translationLib;

        /// <summary>
        /// Select development language (default value - Eng), if you need to check the completeness of the translation,
        /// specify the list of languages requiring verification (default value - no validation)
        /// </summary>
        /// <param name="developmentLanguage"></param>
        /// <param name="languagesCheckList"></param>
        public static void Initialization(Language developmentLanguage = Language.Eng, Language[] languagesCheckList = null)
        {
            if (StringLocalizator.developmentLanguage == null)
            {
                StringLocalizator.developmentLanguage = developmentLanguage;

                try
                {
                    string jsonData = Resources.Load("translationLibrary").ToString();
                    translationLib = JsonUtility.FromJson<JsonListWrapper<TranslationData>>(jsonData).list;
                }
                catch
                {
                    translationLib = new();
                    UpdateTranslateLib();
                }

                if (languagesCheckList != null)
                {
                    CheckTranslateData(languagesCheckList);
                }
            }
            else
            {
                Debug.LogError("<color=red>The system has already been initiated!</color>");
            }
        }

        /// <summary>
        /// Set the translation language for the ".Translate()" function and for the "AutoTranslation" script
        /// </summary>
        /// <param name="language"></param>
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
                Debug.LogWarning("<color=red>Translate language not set!</color>");
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
                string message = developmentLanguage == null ? "StringLocalizator is not initialized" : "Translation library missing!";
                Debug.LogWarning($"<color=red>{message}</color>");
                return str;
            }

            foreach(var data in translationLib)
            {
                if(Equals(str, data.GetValue(developmentLanguage.Value)))
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
            translationLib.Add(new TranslationData(str, developmentLanguage.Value));
            UpdateTranslateLib();
            return str;
        }

        private static void UpdateTranslateLib()
        {
#if UNITY_EDITOR
            string data = JsonUtility.ToJson(new JsonListWrapper<TranslationData>(translationLib), true);
            string dataPath = Path.Combine("Assets", "Resources", "translationLibrary.json");            
            File.WriteAllText(dataPath, data);
#endif
        }

        private static void CheckTranslateData(Language[] languagesCheckList)
        {
#if UNITY_EDITOR
            var needTranslateElements = translationLib.Where(e => e.NotTranslatedTo(languagesCheckList));

            if (needTranslateElements.Count() != 0)
            {
                string s = $"{needTranslateElements.Count()} item(s) does not have a translation in the selected languages:";

                foreach(var language in languagesCheckList)
                {
                    s += $" {language};";
                }

                Debug.LogWarning(s);
            }
#endif
        }
    }
}