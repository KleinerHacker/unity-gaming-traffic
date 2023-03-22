using UnityEngine;

namespace UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight.Handler.Emissive
{
    [AddComponentMenu(UnityGamingTrafficConstants.Menu.TrafficLightMenu + "/Traffic Light Handler (Lamp Emissive Indexed Material)")]
    [DisallowMultipleComponent]
    public sealed class TrafficLightLampEmissiveIndexHandler : TrafficLightLampEmissiveSingleRendererBaseHandler
    {
        private const string MaterialEmissiveColorKey = "_EmissiveColor";
        private static readonly int MaterialEmissiveColor = Shader.PropertyToID(MaterialEmissiveColorKey);

        #region Inspector Data

        [Header("Material Indices")]
        [SerializeField]
        private int redMaterialIndex;

        [SerializeField]
        private int yellowMaterialIndex;

        [SerializeField]
        private int greenMaterialIndex;

        #endregion

        protected override void SwitchRedState(float v)
        {
            _renderer.materials[redMaterialIndex].SetColor(MaterialEmissiveColor, Color.Lerp(Color.black, emissiveColor, v));
        }

        protected override void SwitchYellowState(float v)
        {
            _renderer.materials[yellowMaterialIndex].SetColor(MaterialEmissiveColor, Color.Lerp(Color.black, emissiveColor, v));
        }

        protected override void SwitchGreenState(float v)
        {
            _renderer.materials[greenMaterialIndex].SetColor(MaterialEmissiveColor, Color.Lerp(Color.black, emissiveColor, v));
        }

        protected override void SwitchRedStateEditor(float v)
        {
            GetComponent<MeshRenderer>().sharedMaterials[redMaterialIndex].SetColor(MaterialEmissiveColor, Color.Lerp(Color.black, emissiveColor, v));
        }

        protected override void SwitchYellowStateEditor(float v)
        {
            GetComponent<MeshRenderer>().sharedMaterials[yellowMaterialIndex].SetColor(MaterialEmissiveColor, Color.Lerp(Color.black, emissiveColor, v));
        }

        protected override void SwitchGreenStateEditor(float v)
        {
            GetComponent<MeshRenderer>().sharedMaterials[greenMaterialIndex].SetColor(MaterialEmissiveColor, Color.Lerp(Color.black, emissiveColor, v));
        }
    }
}