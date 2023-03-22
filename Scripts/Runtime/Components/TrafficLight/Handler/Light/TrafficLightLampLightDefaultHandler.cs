using UnityEngine;

namespace UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight.Handler.Light
{
    [AddComponentMenu(UnityGamingTrafficConstants.Menu.TrafficLightMenu + "/Traffic Light Handler (Lamp Light Default)")]
    [DisallowMultipleComponent]
    public sealed class TrafficLightLampLightDefaultHandler : TrafficLightLampLightBaseHandler
    {
        #region Inspector Data

        [Header("Intensities")]
        [SerializeField]
        private float redIntensity = 10_000f;

        [SerializeField]
        private float yellowIntensity = 10_000f;

        [SerializeField]
        private float greenIntensity = 10_000f;

        #endregion

        protected override void SwitchRedState(float v)
        {
            foreach (var redLight in redLights)
            {
                redLight.intensity = redIntensity * v;
            }
        }

        protected override void SwitchYellowState(float v)
        {
            foreach (var yellowLight in yellowLights)
            {
                yellowLight.intensity = yellowIntensity * v;
            }
        }

        protected override void SwitchGreenState(float v)
        {
            foreach (var greenLight in greenLights)
            {
                greenLight.intensity = greenIntensity * v;
            }
        }

        protected override void SwitchRedStateEditor(float v) => SwitchRedState(v);

        protected override void SwitchYellowStateEditor(float v) => SwitchYellowState(v);

        protected override void SwitchGreenStateEditor(float v) => SwitchGreenState(v);
    }
}