using System;
using UnityEditor;
using UnityEngine;

namespace KalkuzSystems.Analysis.Debugging
{
    /// <summary>
    /// An extended version of the <see cref="Debug"/>.
    /// </summary>
    public static class KalkuzLogger
    {
        /// <summary>
        /// Used to determine how the <see cref="KalkuzLogger"/> going to behave.
        /// </summary>
        private static LoggerSettings m_settings;

        /// <summary>
        /// Logs the message to Unity's console.
        /// </summary>
        /// <param name="message">Message to be written</param>
        public static void Info(string message)
        {
            Log(() =>
            {
                if (!m_settings.LogsEnabled) return;

                var color = ColorUtility.ToHtmlStringRGBA(m_settings.LogColor);
                Debug.Log($"<color=#{color}>{message}</color>");
            });
        }

        /// <summary>
        /// Prints Warning to the Unity console.
        /// </summary>
        /// <param name="message">Message to be written</param>
        public static void Warning(string message)
        {
            Log(() =>
            {
                if (!m_settings.WarningsEnabled) return;

                var color = ColorUtility.ToHtmlStringRGBA(m_settings.WarningColor);
                Debug.LogWarning($"<color=#{color}>{message}</color>");
            });
        }

        /// <summary>
        /// Prints Error to the Unity console.
        /// </summary>
        /// <param name="message">Message to be written</param>
        public static void Error(string message)
        {
            Log(() =>
            {
                if (!m_settings.ErrorsEnabled) return;

                var color = ColorUtility.ToHtmlStringRGBA(m_settings.ErrorColor);
                Debug.LogError($"<color=#{color}>{message}</color>");
            });
        }

        private static void Log(Action logAction)
        {
            if (m_settings == null)
            {
                m_settings = LoggerSettings.GetSettings();
            }

            if (!Application.isEditor && m_settings.EditorOnly) return;
            logAction?.Invoke();
        }
    }
}