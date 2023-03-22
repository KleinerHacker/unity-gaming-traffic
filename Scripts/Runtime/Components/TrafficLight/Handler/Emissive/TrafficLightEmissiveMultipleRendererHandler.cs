using UnityEngine;

namespace UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight.Handler.Emissive
{
    [AddComponentMenu(UnityGamingTrafficConstants.Menu.TrafficLightMenu + "/Traffic Light Handler (Lamp Emissive Multi Renderer)")]
    [DisallowMultipleComponent]
    public sealed class TrafficLightEmissiveMultipleRendererHandler : TrafficLightLampEmissiveBaseHandler
    {
#if HDRP && !FORCE_URP
        private const string MaterialEmissiveColorKey = "_EmissiveColor";
#else
        private const string MaterialEmissiveColorKey = "_EmissionColor";
#endif
        private static readonly int MaterialEmissiveColor = Shader.PropertyToID(MaterialEmissiveColorKey);

        #region Inspector Data

        [SerializeField]
        private Renderer redRenderer;

        [SerializeField]
        private Renderer yellowRenderer;

        [SerializeField]
        private Renderer greenRenderer;

        #endregion

        protected override void SwitchRedState(float v)
        {
            redRenderer.material.SetColor(MaterialEmissiveColor, Color.Lerp(Color.black, emissiveColor, v));
        }

        protected override void SwitchYellowState(float v)
        {
            yellowRenderer.material.SetColor(MaterialEmissiveColor, Color.Lerp(Color.black, emissiveColor, v));
        }

        protected override void SwitchGreenState(float v)
        {
            greenRenderer.material.SetColor(MaterialEmissiveColor, Color.Lerp(Color.black, emissiveColor, v));
        }

        protected override void SwitchRedStateEditor(float v)
        {
            redRenderer.sharedMaterial.SetColor(MaterialEmissiveColor, Color.Lerp(Color.black, emissiveColor, v));
        }

        protected override void SwitchYellowStateEditor(float v)
        {
            yellowRenderer.sharedMaterial.SetColor(MaterialEmissiveColor, Color.Lerp(Color.black, emissiveColor, v));
        }

        protected override void SwitchGreenStateEditor(float v)
        {
            greenRenderer.sharedMaterial.SetColor(MaterialEmissiveColor, Color.Lerp(Color.black, emissiveColor, v));
        }
    }
}