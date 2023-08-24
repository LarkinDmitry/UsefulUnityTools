using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KAKuBCE.UsefulUnityTools
{
    public static class StringLocalizator
    {
        private static Dictionary<string, Translate> translateData;

        public static string TranslateTo(this string str, Language language)
        {
            if (translateData == null)
            {
                Initialization();
            }

            if (!translateData.ContainsKey(str))
            {
                Debug.LogWarning($"TranslateData does not contain a translation for \"{str}\"");
                translateData.Add(str, Translate.Empty);
                UpdateTranslateData();
            }

            string result = translateData[str].GetTranslate(language);
            return string.IsNullOrEmpty(result) ? str : result;
        }

        private static void Initialization()
        {
            try
            {
                string jsonData = Resources.Load("translateData").ToString();
                translateData = JsonConvert.DeserializeObject<Dictionary<string, Translate>>(jsonData);
            }
            catch
            {
                UpdateTranslateData();
            }

            CheckTranslateData();
        }

        private static void UpdateTranslateData()
        {
#if UNITY_EDITOR
            string dataPath = Path.Combine("Assets", "Resources", "translateData.json");
            string data = JsonConvert.SerializeObject(new(), Formatting.Indented);
            File.WriteAllText(dataPath, data);
#endif
        }

        private static void CheckTranslateData()
        {
#if UNITY_EDITOR
            string result = string.Empty;

            foreach (var v in translateData)
            {
                if (v.Value.IsNotFullTranslate())
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result += $"No full translation for:";
                    }

                    result += $"\n{v.Key}";
                }
            }

            if (!string.IsNullOrEmpty(result))
            {
                Debug.LogWarning(result);
            }
#endif
        }
    }

    public enum Language { Ru, En, Fr }

    [Serializable]
    public class Translate
    {
        public string en;
        public string ru;
        public string fr;

        public static Translate Empty => new()
        {
            en = string.Empty,
            ru = string.Empty,
            fr = string.Empty
        };        

        public bool IsNotFullTranslate()
        {
            return string.IsNullOrEmpty(en) || string.IsNullOrEmpty(ru) || string.IsNullOrEmpty(fr);
        }

        public string GetTranslate(Language language)
        {
            return language switch
            {
                Language.En => en,
                Language.Ru => ru,
                Language.Fr => fr,
                _ => string.Empty,
            };
        }
    }
}
