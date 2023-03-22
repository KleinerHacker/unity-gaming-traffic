using System;
using System.Linq;
using UnityEngine;
using UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.Extras;
using UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight;

namespace UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Assets.Gaming.Traffic.TrafficLight
{
    [CreateAssetMenu(menuName = UnityGamingTrafficConstants.Menu.TrafficLightMenu + "/Traffic Light Cross Preset")]
    public sealed class TrafficLightCrossPreset : ScriptableObject
    {
        #region Inspector Data

        [SerializeField]
        private TrafficLightGroupReference[] references;

        [SerializeField]
        private TrafficLightCrossPhase[] phases = Array.Empty<TrafficLightCrossPhase>();

        #endregion

        #region Properties

        public TrafficLightCrossPhase[] Phases => phases;

        public TrafficLightGroupReference[] References => references;

        #endregion

        #region Builtin Methods

#if UNITY_EDITOR
        private void OnValidate()
        {
            var references =
                this.references.Where(x => !string.IsNullOrWhiteSpace(x.Uuid)).Select(x => x.Uuid).GroupBy(x => x).Any(x => x.Count() > 1)
                    ? this.references
                    : this.references
                        .Where(x => string.IsNullOrWhiteSpace(x.Uuid));

            foreach (var reference in references)
            {
                reference.Uuid = Guid.NewGuid().ToString();
            }
        }
#endif

        #endregion
    }

    [Serializable]
    public sealed class TrafficLightCrossPhase
    {
        #region Inspector Data

        [SerializeField]
        private string name;

        [SerializeField]
        private float showTime = 10f;

        [SerializeField]
        private TrafficLightCrossBehavior[] behaviors = Array.Empty<TrafficLightCrossBehavior>();

        #endregion

        #region Properties

        public string Name => name;

        public float ShowTime => showTime;

        public TrafficLightCrossBehavior[] Behaviors => behaviors;

        #endregion
    }

    [Serializable]
    public sealed class TrafficLightCrossBehavior
    {
        #region Inspector Data

        [SerializeField]
        private string name;

        [SerializeField]
        [TrafficLightGroupReference]
        private string trafficLightGroupReference;

        [SerializeField]
        private TrafficLightState targetState = TrafficLightState.Off;

        #endregion

        #region Properties

        public string Name => name;

        public string TrafficLightGroupReference => trafficLightGroupReference;

        public TrafficLightState TargetState => targetState;

        #endregion
    }

    [Serializable]
    public sealed class TrafficLightGroupReference
    {
        #region Inspector Data

        [SerializeField]
        private string name;

        [SerializeField]
        [HideInInspector]
        private string uuid;

        #endregion

        #region Properrties

        public string Uuid
        {
            get => uuid;
#if UNITY_EDITOR
            set => uuid = value;
#endif
        }

        public string Name => name;

        #endregion
    }
}