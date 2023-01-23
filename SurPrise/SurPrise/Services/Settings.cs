// Stockage persistant des informations par l'application avec le plugin Settings

using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurPrise.Services
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        // Clé et valeur par défaut de la liste de prises, stockée en format JSON
        private const string PlugListContentKey = "pluglistContent";
        private static readonly string PlugListContentDefault = "";

        #endregion

        // Getter et setter de la liste de prises
        public static List<(string name, string desc, bool on, Guid id)> PlugListContent
        {
            get
            {
                string value = AppSettings.GetValueOrDefault(PlugListContentKey, PlugListContentDefault);
                List<(string name, string desc, bool on, Guid id)> map;
                if (string.IsNullOrEmpty(value))
                    map = new List<(string name, string desc, bool on, Guid id)>();
                else
                    map = JsonConvert.DeserializeObject<List<(string name, string desc, bool on, Guid id)>>(value);
                return map;
            }
            set
            {
                string listValue = JsonConvert.SerializeObject(value);
                AppSettings.AddOrUpdateValue(PlugListContentKey, listValue);
            }
        }
    }
}
