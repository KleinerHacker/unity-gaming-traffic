using System.Linq;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Commons;
using UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Assets.Gaming.Traffic.TrafficLight;
using UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight;

namespace UnityGamingTraffic.Editor.gaming.traffic_light.Scripts.Editor.Components.TrafficLight
{
    [CustomEditor(typeof(TrafficLightCrossController))]
    public sealed class TrafficLightCrossControllerEditor : AutoEditor
    {
        [SerializedPropertyReference("cleanupDelay")]
        [SerializedPropertyDefaultRepresentation]
        [SerializedPropertyLabeledGroup("Logic", Order = 10)]
        private SerializedProperty _cleanupProperty;

        [SerializedPropertyReference("preset")]
        [SerializedPropertyDefaultRepresentation]
        [SerializedPropertyLabeledGroup("Logic", Order = 10)]
        private SerializedProperty _presetProperty;

        [SerializedPropertyReference("assignments")]
        private SerializedProperty[] _assignmentProperties;

        [SerializedPropertyReference("startupState")]
        [SerializedPropertyDefaultRepresentation]
        [SerializedPropertyLabeledGroup("Behavior", Order = 0)]
        private SerializedProperty _startupStateProperty;

        protected override void DoInspectorGUI()
        {
            var preset = (TrafficLightCrossPreset)_presetProperty.objectReferenceValue;
            if (preset == null)
            {
                EditorGUILayout.HelpBox("No preset is assigned", MessageType.Warning);
                return;
            }

            var crossController = (TrafficLightCrossController)serializedObject.targetObject;
            if (CleanupList(crossController, preset))
                return;

            IntentArea(() =>
            {
                foreach (var reference in preset.References)
                {
                    LabeledArea(reference.Name, () =>
                    {
                        EditorGUILayout.PropertyField(_assignmentProperties
                            .First(x => string.Equals(x.FindPropertyRelative("uuid").stringValue, reference.Uuid))
                            .FindPropertyRelative("trafficLights"));
                    });
                }
            });
        }

        private bool CleanupList(TrafficLightCrossController controller, TrafficLightCrossPreset preset)
        {
            var removedAssignments = controller.Assignments
                .Where(x => !preset.References.Any(y => string.Equals(x.Uuid, y.Uuid)));
            var addedAssignments = preset.References
                .Where(x => !controller.Assignments.Any(y => string.Equals(x.Uuid, y.Uuid)));

            if (!removedAssignments.Any() && !addedAssignments.Any())
                return false;

            controller.Assignments = controller.Assignments
                .RemoveAll(removedAssignments.ToArray())
                .Concat(addedAssignments.Select(x => new TrafficLightAssignment { Uuid = x.Uuid }))
                .ToArray();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return true;
        }
    }
}