using System.Linq;
using UnityCommonEx.Runtime.common_ex.Scripts.Runtime.Utils.Extensions;
using UnityEditor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor;
using UnityEditorEx.Editor.editor_ex.Scripts.Editor.Utils.Extensions;
using UnityEngine;
using UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.Extras;

namespace UnityGamingTraffic.Editor.gaming.traffic_light.Scripts.Editor.Extras
{
    [CustomPropertyDrawer(typeof(TrafficLightGroupReferenceAttribute))]
    public sealed class TrafficLightGroupReferenceEditor : ExtendedDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var properties = property.serializedObject.FindProperties("references")
                .Select(x => (uuid: x.FindPropertyRelative("uuid"), name: x.FindPropertyRelative("name")))
                .ToArray();

            var index = properties.IndexOf(x => string.Equals(x.uuid.stringValue, property.stringValue));
            index = EditorGUI.Popup(position, label, index, properties.Select(x => x.name.stringValue).Select(x => new GUIContent(x)).ToArray());
            property.stringValue = index < 0 ? "" : properties[index].uuid.stringValue;
        }
    }
}