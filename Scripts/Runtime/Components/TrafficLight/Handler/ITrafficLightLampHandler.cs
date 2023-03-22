using System;

namespace UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight.Handler
{
    public interface ITrafficLightLampHandler
    {
        void SwitchRed(bool on, bool editor = false, Action onFinished = null);

        void SwitchYellow(bool on, bool editor = false, Action onFinished = null);

        void SwitchGreen(bool on, bool editor = false, Action onFinished = null);
    }
}