using UnityEngine;

namespace UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight.Handler.Emissive
{
    public abstract class TrafficLightLampEmissiveSingleRendererBaseHandler : TrafficLightLampEmissiveBaseHandler
    {
        protected Renderer _renderer;

        protected virtual void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }
    }
}