using System;
using UnityAnimation.Runtime.animation.Scripts.Runtime.Utils;
using UnityEngine;
using UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Assets.Gaming.Traffic.TrafficLight;
using UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight.Handler;
using UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight.Handler.Emissive;
using UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight.Handler.Light;

namespace UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight
{
    [AddComponentMenu(UnityGamingTrafficConstants.Menu.TrafficLightMenu + "/Traffic Light Controller")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ITrafficLightLampEmissiveHandler), typeof(ITrafficLightLampLightHandler))]
    public sealed class TrafficLightController : MonoBehaviour
    {
        #region Inspector Data

        [Header("Preset")]
        [SerializeField]
        private TrafficLightPreset preset;

        [Header("Behavior")]
        [SerializeField]
        private TrafficLightState startupState = TrafficLightState.Off;

        #endregion

        #region Properties

        public TrafficLightState State { get; private set; } = TrafficLightState.Off;

        #endregion

        private TrafficLightCommonHandler _handler;

        private float _timeCounter = 0f;
        private byte _hookUpCounter = 0;
#if UNITY_EDITOR
        private float _editorTimeCounter = 0f;
        private byte _editorColorCounter = 0;
#endif

        #region Builtin Methods

        private void Awake()
        {
            var emissiveHandler = GetComponent<ITrafficLightLampEmissiveHandler>();
            var lightHandler = GetComponent<ITrafficLightLampLightHandler>();
            _handler = new TrafficLightCommonHandler(emissiveHandler, lightHandler);

            switch (startupState)
            {
                case TrafficLightState.Red:
                    SwitchRed(force: true);
                    break;
                case TrafficLightState.Green:
                    SwitchGreen(force: true);
                    break;
                case TrafficLightState.OutOfOrder:
                    SwitchOutOfOrder(force: true);
                    break;
                case TrafficLightState.Off:
                    SwitchOff(force: true);
                    break;
                default:
                    throw new NotImplementedException(startupState.ToString());
            }
        }

        private void FixedUpdate()
        {
            if (State != TrafficLightState.OutOfOrder)
                return;

            _timeCounter += Time.fixedDeltaTime;
            if (_timeCounter >= preset.OutOfOrder[_hookUpCounter].PostDelay)
            {
                _hookUpCounter++;
                if (_hookUpCounter >= preset.OutOfOrder.Length)
                {
                    _hookUpCounter = 0;
                }

                ChangeHookUpLampState(preset.OutOfOrder[_hookUpCounter].Lamps);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
                return;

            _editorTimeCounter += Time.deltaTime;
            if (_editorTimeCounter >= 1f)
            {
                _editorTimeCounter = 0f;
                _editorColorCounter++;
                if (_editorColorCounter >= 3)
                {
                    _editorColorCounter = 0;
                }
            }

            var emissiveHandler = GetComponent<ITrafficLightLampEmissiveHandler>();
            if (emissiveHandler == null)
                throw new InvalidOperationException("Emissive Handler required");
            var lightHandler = GetComponent<ITrafficLightLampLightHandler>();
            if (lightHandler == null)
                throw new InvalidOperationException("Light Handler required");
            var handler = new TrafficLightCommonHandler(emissiveHandler, lightHandler);
            switch (_editorColorCounter)
            {
                case 0:
                    handler.SwitchRed(true, true);
                    handler.SwitchYellow(false, true);
                    handler.SwitchGreen(false, true);
                    break;
                case 1:
                    handler.SwitchRed(false, true);
                    handler.SwitchYellow(true, true);
                    handler.SwitchGreen(false, true);
                    break;
                case 2:
                    handler.SwitchRed(false, true);
                    handler.SwitchYellow(false, true);
                    handler.SwitchGreen(true, true);
                    break;
            }
        }
#endif

        #endregion

        public void SwitchRed(Action onFinished = null, bool force = false)
        {
            if (!force && State == TrafficLightState.Red)
            {
                onFinished?.Invoke();
                return;
            }

#if PCSOFT_TRAFFIC_LIGHT_LOGGING
            Debug.Log("[TRAFFIC LIGHT] Switch to red", this);
#endif

            if (State == TrafficLightState.OutOfOrder)
            {
                OffOldColor();
            }

            RunPresetHookUp(preset.GreenToRed, onFinished);
            State = TrafficLightState.Red;
        }

        public void SwitchGreen(Action onFinished = null, bool force = false)
        {
            if (!force && State == TrafficLightState.Green)
            {
                onFinished?.Invoke();
                return;
            }

#if PCSOFT_TRAFFIC_LIGHT_LOGGING
            Debug.Log("[TRAFFIC LIGHT] Switch to green", this);
#endif

            if (State == TrafficLightState.OutOfOrder)
            {
                OffOldColor();
            }

            RunPresetHookUp(preset.RedToGreen, onFinished);
            State = TrafficLightState.Green;
        }

        public void SwitchOutOfOrder(Action onFinished = null, bool force = false)
        {
            if (!force && State == TrafficLightState.OutOfOrder)
            {
                onFinished?.Invoke();
                return;
            }

#if PCSOFT_TRAFFIC_LIGHT_LOGGING
            Debug.Log("[TRAFFIC LIGHT] Switch to out of order", this);
#endif

            OffOldColor();
            State = TrafficLightState.OutOfOrder;
            _timeCounter = 0f;
            _hookUpCounter = 0;
        }

        public void SwitchOff(Action onFinished = null, bool force = false)
        {
            if (!force && State == TrafficLightState.Off)
            {
                onFinished?.Invoke();
                return;
            }

#if PCSOFT_TRAFFIC_LIGHT_LOGGING
            Debug.Log("[TRAFFIC LIGHT] Switch off", this);
#endif

            OffOldColor();
            State = TrafficLightState.Off;
        }

        private void RunPresetHookUp(TrafficLightHookUp[] hookUps, Action onFinished)
        {
            var animationBuilder = AnimationBuilder.Create(this);
            foreach (var hookUp in hookUps)
            {
                animationBuilder
                    .Immediately(() => ChangeHookUpLampState(hookUp.Lamps))
                    .Wait(hookUp.PostDelay);
            }

            animationBuilder
                .WithFinisher(onFinished)
                .Start();
        }

        private void ChangeHookUpLampState(TrafficLightLampHookUp[] lamps)
        {
            foreach (var lampHookUp in lamps)
            {
                switch (lampHookUp.Lamp)
                {
                    case TrafficLightLamp.Red:
                        _handler.SwitchRed(lampHookUp.State == TrafficLightLampState.On);
                        break;
                    case TrafficLightLamp.Yellow:
                        _handler.SwitchYellow(lampHookUp.State == TrafficLightLampState.On);
                        break;
                    case TrafficLightLamp.Green:
                        _handler.SwitchGreen(lampHookUp.State == TrafficLightLampState.On);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void OffOldColor()
        {
            switch (State)
            {
                case TrafficLightState.Red:
                    _handler.SwitchRed(false);
                    break;
                case TrafficLightState.Green:
                    _handler.SwitchGreen(false);
                    break;
                case TrafficLightState.OutOfOrder:
                    _handler.SwitchYellow(false);
                    break;
                case TrafficLightState.Off:
                    break;
                default:
                    throw new NotImplementedException(State.ToString());
            }
        }
    }

    public enum TrafficLightState
    {
        Red,
        Green,
        OutOfOrder,
        Off
    }
}