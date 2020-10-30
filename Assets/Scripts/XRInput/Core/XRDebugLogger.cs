using System;
using UnityEngine;
using System.Linq;
using TMPro;
using Crengine.XRInput.Core.Singleton;

namespace Crengine.XRInput.Core
{
    public class XRDebugLogger : XRInputSingleton<XRDebugLogger>
    {
        [SerializeField] private TextMeshProUGUI logTextArea;
        [SerializeField] private bool enableDebug = false;
        [SerializeField] private int maxLines = 15;

        private void OnEnable()
        {
            logTextArea.enabled = enableDebug;
            enabled = enableDebug;
        }

        public void LogInfo(string message)
        {
            ClearLines();
            logTextArea.text += $"{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")} <color=\"white\">{message}</color>\n";
        }

        public void LogError(string message)
        {
            ClearLines();
            logTextArea.text += $"{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")} <color=\"red\">{message}</color>\n";
        }

        public void LogWarning(string message)
        {
            ClearLines();
            logTextArea.text += $"{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")} <color=\"yellow\">{message}</color>\n";
        }

        private void ClearLines()
        {
            if (logTextArea.text.Split('\n').Count() >= maxLines)
            {
                logTextArea.text = string.Empty;
            }
        }
    }
}