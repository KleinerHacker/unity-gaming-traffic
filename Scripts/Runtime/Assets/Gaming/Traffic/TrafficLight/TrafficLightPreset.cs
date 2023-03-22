using System;
using UnityEngine;

namespace UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Assets.Gaming.Traffic.TrafficLight
{
    [CreateAssetMenu(menuName = UnityGamingTrafficConstants.Menu.TrafficLightMenu + "/Traffic Light Preset")]
    public sealed class TrafficLightPreset : ScriptableObject
    {
        #region Inspector Data

        [SerializeField]
        private TrafficLightHookUp[] greenToRed;

        [SerializeField]
        private TrafficLightHookUp[] redToGreen;

        [SerializeField]
        private TrafficLightHookUp[] outOfOrder;

        #endregion

        #region Properties

        public TrafficLightHookUp[] GreenToRed => greenToRed;

        public TrafficLightHookUp[] RedToGreen => redToGreen;

        public TrafficLightHookUp[] OutOfOrder => outOfOrder;

        #endregion
    }

    [Serializable]
    public sealed class TrafficLightHookUp
    {
        #region Inspector Data

        [SerializeField]
        private TrafficLightLampHookUp[] lamps;

        [SerializeField]
        [Min(0)]
        private float postDelay;

        #endregion

        #region Properties

        public TrafficLightLampHookUp[] Lamps => lamps;

        public float PostDelay => postDelay;

        #endregion
    }

    [Serializable]
    public sealed class TrafficLightLampHookUp
    {
        #region Inspector Data

        [SerializeField]
        private TrafficLightLamp lamp = TrafficLightLamp.Red;

        [SerializeField]
        private TrafficLightLampState state = TrafficLightLampState.On;

        #endregion

        #region Properties

        public TrafficLightLamp Lamp => lamp;

        public TrafficLightLampState State => state;

        #endregion
    }

    public enum TrafficLightLamp
    {
        Red,
        Yellow,
        Green
    }

    public enum TrafficLightLampState
    {
        On,
        Off
    }

    public static class TrafficLightLampStateExtensions
    {
        public static TrafficLightLampState Invert(this TrafficLightLampState state)
        {
            return state switch
            {
                TrafficLightLampState.On => TrafficLightLampState.Off,
                TrafficLightLampState.Off => TrafficLightLampState.On,
                _ => throw new NotImplementedException(state.ToString())
            };
        }
    }
}