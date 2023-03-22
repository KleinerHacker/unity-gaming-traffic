using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Assets.Gaming.Traffic.TrafficLight;

namespace UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight
{
    [AddComponentMenu(UnityGamingTrafficConstants.Menu.TrafficLightMenu + "/Traffic Light Cross Controller")]
    [DisallowMultipleComponent]
    public sealed class TrafficLightCrossController : MonoBehaviour
    {
        #region Inspector Data

        [SerializeField]
        private float cleanupDelay = 1f;

        [SerializeField]
        private TrafficLightCrossPreset preset;

        [SerializeField]
        private TrafficLightAssignment[] assignments = Array.Empty<TrafficLightAssignment>();

        [SerializeField]
        private TrafficLightCrossState startupState = TrafficLightCrossState.Off;

        #endregion

        #region Properties

        public TrafficLightCrossState State { get; private set; } = TrafficLightCrossState.Off;

#if UNITY_EDITOR
        public TrafficLightAssignment[] Assignments
        {
            get => assignments;
            set => assignments = value;
        }
#endif

        #endregion

        private bool _isCleanup = false;
        private int _phaseIndex = 0;
        private float _timeCounter = 0f;

        #region Builtin Methods

        private void Awake()
        {
            switch (startupState)
            {
                case TrafficLightCrossState.Regular:
                    SwitchRegular();
                    break;
                case TrafficLightCrossState.OutOfOrder:
                    SwitchOutOfOrder();
                    break;
                case TrafficLightCrossState.Off:
                    SwitchOff();
                    break;
                default:
                    throw new NotImplementedException(State.ToString());
            }
        }

        private void FixedUpdate()
        {
            if (State != TrafficLightCrossState.Regular)
                return;

            _timeCounter += Time.fixedDeltaTime;
            if (_isCleanup && _timeCounter >= cleanupDelay)
            {
                _timeCounter = 0f;
                _isCleanup = false;

#if PCSOFT_TRAFFIC_LIGHT_LOGGING
                Debug.Log("[TRAFFIC LIGHT CROSS] Finish cleanup phase", this);
                Debug.Log("[TRAFFIC LIGHT CROSS] Switch to green for " + string.Join(',',
                    preset.Phases[_phaseIndex].Behaviors
                        .Where(x => x.TargetState == TrafficLightState.Green)
                        .SelectMany(x => FindTrafficLights(x.TrafficLightGroupReference))
                        .Distinct()
                        .Select(x => x.gameObject.name)
                ), this);
#endif

                SwitchTrafficLightByBehaviors(
                    preset.Phases[_phaseIndex].Behaviors
                        .Where(x => x.TargetState == TrafficLightState.Green)
                );
            }
            else if (!_isCleanup && _timeCounter >= preset.Phases[_phaseIndex].ShowTime)
            {
                _timeCounter = 0f;
                _isCleanup = true;

                _phaseIndex++;
                if (_phaseIndex >= preset.Phases.Length)
                {
                    _phaseIndex = 0;
                }

#if PCSOFT_TRAFFIC_LIGHT_LOGGING
                Debug.Log("[TRAFFIC LIGHT CROSS] Switch to red for " + string.Join(',',
                    preset.Phases[_phaseIndex].Behaviors
                        .Where(x => x.TargetState == TrafficLightState.Red)
                        .SelectMany(x => FindTrafficLights(x.TrafficLightGroupReference))
                        .Distinct()
                        .Select(x => x.gameObject.name)
                ), this);
                Debug.Log("[TRAFFIC LIGHT CROSS] Start cleanup phase", this);
#endif

                SwitchTrafficLightByBehaviors(
                    preset.Phases[_phaseIndex].Behaviors
                        .Where(x => x.TargetState == TrafficLightState.Red)
                );
            }
        }

        #endregion

        public void SwitchRegular(bool force = false)
        {
            if (!force && State == TrafficLightCrossState.Regular)
                return;

#if PCSOFT_TRAFFIC_LIGHT_LOGGING
            Debug.Log("[TRAFFIC LIGHT CROSS] Switch state to regular", this);
#endif

            foreach (var controller in preset.Phases.SelectMany(x => x.Behaviors).SelectMany(x => FindTrafficLights(x.TrafficLightGroupReference)).Distinct())
            {
                controller.SwitchOff(force: force);
            }

            State = TrafficLightCrossState.Regular;
        }

        public void SwitchOutOfOrder(bool force = false)
        {
            if (!force && State == TrafficLightCrossState.OutOfOrder)
                return;

#if PCSOFT_TRAFFIC_LIGHT_LOGGING
            Debug.Log("[TRAFFIC LIGHT CROSS] Switch state to Out Of Order", this);
#endif

            foreach (var controller in preset.Phases.SelectMany(x => x.Behaviors).SelectMany(x => FindTrafficLights(x.TrafficLightGroupReference)).Distinct())
            {
                controller.SwitchOutOfOrder(force: force);
            }

            State = TrafficLightCrossState.OutOfOrder;
        }

        public void SwitchOff(bool force = false)
        {
            if (!force && State == TrafficLightCrossState.Off)
                return;

#if PCSOFT_TRAFFIC_LIGHT_LOGGING
            Debug.Log("[TRAFFIC LIGHT CROSS] Switch state Off", this);
#endif

            foreach (var controller in preset.Phases.SelectMany(x => x.Behaviors).SelectMany(x => FindTrafficLights(x.TrafficLightGroupReference)).Distinct())
            {
                controller.SwitchOff(force: force);
            }

            State = TrafficLightCrossState.Off;
        }

        private void SwitchTrafficLightByBehaviors(IEnumerable<TrafficLightCrossBehavior> behaviors)
        {
            foreach (var behavior in behaviors)
            {
                foreach (var trafficLight in FindTrafficLights(behavior.TrafficLightGroupReference))
                {
                    switch (behavior.TargetState)
                    {
                        case TrafficLightState.Red:
                            trafficLight.SwitchRed();
                            break;
                        case TrafficLightState.Green:
                            trafficLight.SwitchGreen();
                            break;
                        case TrafficLightState.OutOfOrder:
                            trafficLight.SwitchOutOfOrder();
                            break;
                        case TrafficLightState.Off:
                            trafficLight.SwitchOff();
                            break;
                        default:
                            throw new NotImplementedException(behavior.TargetState.ToString());
                    }
                }
            }
        }

        private TrafficLightController[] FindTrafficLights(string uuid) =>
            assignments.First(x => string.Equals(x.Uuid, uuid)).TrafficLights;
    }

    [Serializable]
    public sealed class TrafficLightAssignment
    {
        #region Inspector Data

        [SerializeField]
        [HideInInspector]
        private string uuid;

        [SerializeField]
        private TrafficLightController[] trafficLights = Array.Empty<TrafficLightController>();

        #endregion

        #region Properties

        public string Uuid
        {
            get => uuid;
#if UNITY_EDITOR
            set => uuid = value;
#endif
        }

        public TrafficLightController[] TrafficLights => trafficLights;

        #endregion
    }

    public enum TrafficLightCrossState
    {
        Regular,
        OutOfOrder,
        Off
    }
}