using System.Collections.Generic;
using System.IO;
using KalkuzSystems.Attributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KalkuzSystems.Analysis.Debugging
{
    /// <summary>
    /// Settings for how the <see cref="KalkuzLogger"/> going to behave.
    /// </summary>
    public class LoggerSettings : ScriptableObject
    {
        private const string SETTINGS_PATH = "Assets/LoggerSettings.asset";

        #region Fields

        [SerializeField, Tooltip("Logs messages only in the Editor if true.")]
        private bool editorOnly = true;

        [SerializeField, Tooltip("Whether or not the Logs should be printed.")]
        private bool logsEnabled = true;

        [SerializeField, Tooltip("Whether or not the Warnings should be printed.")]
        private bool warningsEnabled = true;

        [SerializeField, Tooltip("Whether or not the Errors should be printed.")]
        private bool errorsEnabled = true;

        [LineSeparator(1, 20)] [SerializeField, Tooltip("Logs' Text Color")]
        private Color logColor = Color.white;

        [SerializeField, Tooltip("Logs' Text Color")]
        private Color warningColor = Color.yellow;

        [SerializeField, Tooltip("Logs' Text Color")]
        private Color errorColor = Color.red;

        /// <summary>
        /// Logs messages only in the Editor if true.
        /// </summary>
        public bool EditorOnly => editorOnly;

        /// <summary>
        /// Whether or not the Logs should be printed.
        /// </summary>
        public bool LogsEnabled => logsEnabled;

        /// <summary>
        /// Whether or not the Warnings should be printed.
        /// </summary>
        public bool WarningsEnabled => warningsEnabled;

        /// <summary>
        /// Whether or not the Errors should be printed.
        /// </summary>
        public bool ErrorsEnabled => errorsEnabled;

        /// <summary>
        /// The color of texts in log messages.
        /// </summary>
        public Color LogColor => logColor;

        /// <summary>
        /// The color of texts in warning messages.
        /// </summary>
        public Color WarningColor => warningColor;

        /// <summary>
        /// the color of texts in error messages.
        /// </summary>
        public Color ErrorColor => errorColor;

        #endregion

        #region SettingsProvider

        internal static LoggerSettings GetSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<LoggerSettings>(SETTINGS_PATH);
            if (settings == null)
            {
                settings = CreateInstance<LoggerSettings>();
                AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
            }

            return settings;
        }

        #endregion
    }

    #region SettingsProvider

    static class LoggerSettingsProviderRegister
    {
        [SettingsProvider]
        public static SettingsProvider CreateFromSettingsObject()
        {
            // Create an AssetSettingsProvider from a settings object (UnityEngine.Object):
            var settingsObj = LoggerSettings.GetSettings();
            var provider = AssetSettingsProvider.CreateProviderFromObject("Project/Kalkuz Systems/Logger Settings", settingsObj);

            // Register keywords from the properties of LoggerSettings
            provider.keywords = SettingsProvider.GetSearchKeywordsFromSerializedObject(new SerializedObject(settingsObj));
            return provider;
        }
    }    

    #endregion
}