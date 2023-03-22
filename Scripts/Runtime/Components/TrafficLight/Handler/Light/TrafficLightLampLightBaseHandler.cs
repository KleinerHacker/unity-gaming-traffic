using System;
using UnityEngine;

namespace UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight.Handler.Light
{
    public abstract class TrafficLightLampLightBaseHandler : TrafficLightLampBaseHandler, ITrafficLightLampLightHandler
    {
        #region Inspector Data

        [Header("Lights")]
        [SerializeField]
        protected UnityEngine.Light[] redLights = Array.Empty<UnityEngine.Light>();

        [SerializeField]
        protected UnityEngine.Light[] yellowLights = Array.Empty<UnityEngine.Light>();

        [SerializeField]
        protected UnityEngine.Light[] greenLights = Array.Empty<UnityEngine.Light>();

        #endregion
    }
}