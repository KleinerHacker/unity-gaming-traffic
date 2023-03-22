using UnityEngine;

namespace UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight.Handler.Emissive
{
    public abstract class TrafficLightLampEmissiveBaseHandler : TrafficLightLampBaseHandler, ITrafficLightLampEmissiveHandler
    {
        #region Inspector Data

        [Header("Lights")]
        [SerializeField]
        [ColorUsage(false, true)]
        protected Color emissiveColor = Color.white * 10_000f;

        #endregion
    }
}