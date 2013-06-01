using System;
using Windows.Storage;

namespace MyBookLibrary.Core
{
    public class ItemPersistedSettings
    {
        public int CurrentPage { get; set; }

        public int CurrentPageInSection { get; set; }

        public int CurrentSection { get; set; }

        public int NumberOfPagesInSection { get; set; }

        public int TotalNumberOfPages { get; set; }

        public static void DeleteItemPersistedSettings(string key)
        {
            ApplicationData.Current.RoamingSettings.Values.Remove(key);
        }

        public static ItemPersistedSettings DeserializeItemPersistedSettings(string key)
        {
            var itemSettings = new ItemPersistedSettings();

            var data = ApplicationData.Current.RoamingSettings.Values[key] as string;

            if (data != null)
            {
                string[] items = data.Split(',');
                itemSettings.CurrentPage = Convert.ToInt32(items[0]);
                itemSettings.CurrentPageInSection = Convert.ToInt32(items[1]);
                itemSettings.CurrentSection = Convert.ToInt32(items[2]);
                itemSettings.NumberOfPagesInSection = Convert.ToInt32(items[3]);
                itemSettings.TotalNumberOfPages = Convert.ToInt32(items[4]);
            }

            return itemSettings;
        }

        public static void SerializeItemPersistedSettings(ItemPersistedSettings itemSettings, string key)
        {
            string data = string.Format("{0},{1},{2},{3},{4}", itemSettings.CurrentPage,
                                        itemSettings.CurrentPageInSection, itemSettings.CurrentSection,
                                        itemSettings.NumberOfPagesInSection, itemSettings.TotalNumberOfPages);

            ApplicationData.Current.RoamingSettings.Values[key] = data;
        }
    }
}