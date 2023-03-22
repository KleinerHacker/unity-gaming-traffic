using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils;

namespace UnityGamingTraffic.Editor.gaming.traffic_light.Scripts.Editor.Provider
{
    public sealed class GamingProvider : SettingsProvider
    {
        private const string TrafficLightLogging = "PCSOFT_TRAFFIC_LIGHT_LOGGING";

        #region Static Area

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new GamingProvider();
        }

        #endregion

        private bool _trafficLightGroup = true;

        public GamingProvider() : base("Project/Gaming", SettingsScope.Project, new[] { "gaming", "tooling" })
        {
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.HelpBox("Project based gaming settings for diverse useful tooling. See areas below.", MessageType.None);

            _trafficLightGroup = EditorGUILayout.BeginFoldoutHeaderGroup(_trafficLightGroup, "Traffic Light System");
            if (_trafficLightGroup)
            {
                EditorGUI.indentLevel = 1;
                {
                    ExtendedEditorGUILayout.SymbolFieldLeft("Verbose Logging", TrafficLightLogging);
                }
                EditorGUI.indentLevel = 0;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}