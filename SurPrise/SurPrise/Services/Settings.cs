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

        private const string PlugListContentKey = "playlistContent";
        private static readonly string PlugListContentDefault = "";

        #endregion

        public static List<(string name, string desc, bool on)> PlugListContent
        {
            get
            {
                string value = AppSettings.GetValueOrDefault(PlugListContentKey, PlugListContentDefault);
                List<(string name, string desc, bool on)> map;
                if (string.IsNullOrEmpty(value))
                    map = new List<(string name, string desc, bool on)>();
                else
                    map = JsonConvert.DeserializeObject<List<(string name, string desc, bool on)>>(value);
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
