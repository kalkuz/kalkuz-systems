using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace KalkuzSystems.Localization.UnityLocalization
{
    /// <summary>
    /// Fast Localization methods which are bridge to
    /// <a href="https://docs.unity3d.com/Packages/com.unity.localization@1.0/manual/index.html">Unity Localization</a>
    /// </summary>
    public static class LocalizationUtility
    {
        /// <summary>
        /// Used to obtain the currently active locale object.
        /// </summary>
        /// <returns>Active locale</returns>
        public static Locale GetCurrentLocale()
        {
            return LocalizationSettings.Instance.GetSelectedLocale();
        }

        /// <summary>
        /// Used to get the available locale object. Note that in order to successfully obtain the locale, the locale
        /// must be initialized in project or assets.
        /// </summary>
        /// <param name="localeIdentifier">The identifier of the requested locale. For example, "en" or "fr".</param>
        /// <returns>The requested locale object</returns>
        public static Locale GetLocale(string localeIdentifier)
        {
            var localizationSettings = LocalizationSettings.Instance;
            var locale = localizationSettings.GetAvailableLocales().GetLocale(new LocaleIdentifier(localeIdentifier));
            if (locale == null)
                throw new Exception($"Locale with identifier '{localeIdentifier}' do not exists. Could not set the new locale.");

            return locale;
        }

        /// <summary>
        /// Changes the currently active locale to desired one.
        /// </summary>
        /// <param name="localeIdentifier">Locale identifier of the locale which will be set active.</param>
        public static void SetCurrentLocale(string localeIdentifier)
        {
            var localizationSettings = LocalizationSettings.Instance;
            var locale = GetLocale(localeIdentifier);

            localizationSettings.SetSelectedLocale(locale);
        }

        /// <summary>
        /// Used to get a translation from the translation table.
        /// </summary>
        /// <param name="fromTable">The table to look for</param>
        /// <param name="fromKey">The key to search in table</param>
        /// <param name="localeIdentifier">The translation requested in the language</param>
        /// <returns>The translation</returns>
        public static string GetTranslation(string fromTable, string fromKey)
        {
            var translation = LocalizationSettings.StringDatabase.GetLocalizedString(fromTable, fromKey, GetCurrentLocale());
            return translation;
        }

        /// <summary>
        /// Used to get a translation from the translation table.
        /// </summary>
        /// <param name="fromTable">The table to look for</param>
        /// <param name="fromKey">The key to search in table</param>
        /// <param name="localeIdentifier">The translation requested in the language</param>
        /// <returns>The translation</returns>
        public static string GetTranslation(string fromTable, string fromKey, string localeIdentifier)
        {
            var translation = LocalizationSettings.StringDatabase.GetLocalizedString(fromTable, fromKey, GetLocale(localeIdentifier));
            return translation;
        }

        /// <summary>
        /// Used to get a translation from the translation table.
        /// </summary>
        /// <param name="fromTable">The table to look for</param>
        /// <param name="fromKey">The key to search in table</param>
        /// <param name="locale">The translation requested in the language</param>
        /// <returns>The translation</returns>
        public static string GetTranslation(string fromTable, string fromKey, Locale locale)
        {
            var translation = LocalizationSettings.StringDatabase.GetLocalizedString(fromTable, fromKey, locale);
            return translation;
        }

        /// <summary>
        /// Used to get a translation from the translation table.
        /// </summary>
        /// <param name="fromTable">The table to look for</param>
        /// <param name="fromKey">The key to search in table</param>
        /// <param name="localeIdentifier">The translation requested in the language</param>
        /// <param name="onComplete">Event that will be executed after translation is obtained</param>
        public static void GetTranslation(string fromTable, string fromKey, string localeIdentifier, UnityAction<string> onComplete)
        {
            var translation = LocalizationSettings.StringDatabase.GetLocalizedString(fromTable, fromKey, GetLocale(localeIdentifier));
            onComplete.Invoke(translation);
        }

        /// <summary>
        /// Used to get a translation from the translation table.
        /// </summary>
        /// <param name="fromTable">The table to look for</param>
        /// <param name="fromKey">The key to search in table</param>
        /// <param name="locale">The translation requested in the language</param>
        /// <param name="onComplete">Event that will be executed after translation is obtained</param>
        public static void GetTranslation(string fromTable, string fromKey, Locale locale, UnityAction<string> onComplete)
        {
            var translation = LocalizationSettings.StringDatabase.GetLocalizedString(fromTable, fromKey, locale);
            onComplete.Invoke(translation);
        }

        /// <summary>
        /// Used to get current language translation from the translation table.
        /// </summary>
        /// <param name="fromTable">The table to look for</param>
        /// <param name="fromKey">The key to search in table</param>
        /// <param name="onComplete">Event that will be executed after translation is obtained</param>
        public static void GetTranslation(string fromTable, string fromKey, UnityAction<string> onComplete)
        {
            var translation = LocalizationSettings.StringDatabase.GetLocalizedString(fromTable, fromKey, GetCurrentLocale());
            onComplete.Invoke(translation);
        }

        /// <summary>
        /// Used to get a translation asynchronously from the translation table.
        /// </summary>
        /// <param name="fromTable">The table to look for</param>
        /// <param name="fromKey">The key to search in table</param>
        /// <param name="localeIdentifier">The translation requested in the language</param>
        /// <param name="onComplete">Event that will be executed after translation is obtained</param>
        public static void GetTranslationAsync(string fromTable, string fromKey, string localeIdentifier, UnityAction<string> onComplete)
        {
            var translation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(fromTable, fromKey, GetLocale(localeIdentifier));

            if (translation.IsDone)
            {
                onComplete.Invoke(translation.Result);
            }
            else
            {
                translation.Completed += (operation) => onComplete.Invoke(operation.Result);
            }
        }

        /// <summary>
        /// Used to get a translation asynchronously from the translation table.
        /// </summary>
        /// <param name="fromTable">The table to look for</param>
        /// <param name="fromKey">The key to search in table</param>
        /// <param name="locale">The translation requested in the language</param>
        /// <param name="onComplete">Event that will be executed after translation is obtained</param>
        public static void GetTranslationAsync(string fromTable, string fromKey, Locale locale, UnityAction<string> onComplete)
        {
            var stringDatabase = LocalizationSettings.StringDatabase;
            var translation =stringDatabase.GetLocalizedStringAsync(fromTable, fromKey, locale);

            if (translation.IsDone)
            {
                onComplete.Invoke(translation.Result);
            }
            else
            {
                translation.Completed += (operation) => onComplete.Invoke(operation.Result);
            }
        }

        /// <summary>
        /// Used to get current language translation asynchronously from the translation table.
        /// </summary>
        /// <param name="fromTable">The table to look for</param>
        /// <param name="fromKey">The key to search in table</param>
        /// <param name="onComplete">Event that will be executed after translation is obtained</param>
        public static void GetTranslationAsync(string fromTable, string fromKey, UnityAction<string> onComplete)
        {
            var stringDatabase = LocalizationSettings.StringDatabase;
            var translation = stringDatabase.GetLocalizedStringAsync(fromTable, fromKey, GetCurrentLocale());

            if (translation.IsDone)
            {
                onComplete.Invoke(translation.Result);
            }
            else
            {
                translation.Completed += (operation) => onComplete.Invoke(operation.Result);
            }
        }

        /// <summary>
        /// Subscribes the action to localization settings' onSelectedLocaleChanged Event
        /// </summary>
        /// <param name="subscriber">Action to be subscribed</param>
        public static void SubscribeOnLocaleChanged(Action<Locale> subscriber)
        {
            LocalizationSettings.Instance.OnSelectedLocaleChanged += subscriber;
        }

        /// <summary>
        /// Unsubscribes the action from localization settings' onSelectedLocaleChanged Event
        /// </summary>
        /// <param name="subscriber">Action to be unsubscribed</param>
        public static void UnsubscribeOnLocaleChanged(Action<Locale> subscriber)
        {
            LocalizationSettings.Instance.OnSelectedLocaleChanged -= subscriber;
        }
    }
}