using System;
using UnityAnimation.Runtime.animation.Scripts.Runtime.Utils;
using UnityEngine;

namespace UnityGamingTraffic.Runtime.gaming.traffic_light.Scripts.Runtime.Components.TrafficLight.Handler
{
    public abstract class TrafficLightLampBaseHandler : MonoBehaviour, ITrafficLightLampHandler
    {
        #region Inspector Data

        [Header("Animation")]
        [SerializeField]
        protected AnimationCurve fadingCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [SerializeField]
        protected float fadingSpeed = 1f;

        #endregion

        public void SwitchRed(bool on, bool editor = false, Action onFinished = null) => SwitchState(on, editor, onFinished, SwitchRedState, SwitchRedStateEditor);

        public void SwitchYellow(bool on, bool editor = false, Action onFinished = null) => SwitchState(on, editor, onFinished, SwitchYellowState, SwitchYellowStateEditor);

        public void SwitchGreen(bool on, bool editor = false, Action onFinished = null) => SwitchState(on, editor, onFinished, SwitchGreenState, SwitchGreenStateEditor);

        private void SwitchState(bool on, bool editor, Action onFinished, Action<float> switchAction, Action<float> switchEditorAction)
        {
            if (editor)
            {
                switchEditorAction(on ? 1f : 0f);
                onFinished?.Invoke();
            }
            else
            {
                AnimationBuilder.Create(this)
                    .Animate(fadingCurve, fadingSpeed, v =>
                    {
                        if (on) switchAction(v);
                        else switchAction(1f - v);
                    })
                    .WithFinisher(onFinished)
                    .Start();
            }
        }

        protected abstract void SwitchRedState(float v);
        protected abstract void SwitchYellowState(float v);
        protected abstract void SwitchGreenState(float v);

        protected abstract void SwitchRedStateEditor(float v);
        protected abstract void SwitchYellowStateEditor(float v);
        protected abstract void SwitchGreenStateEditor(float v);
    }
}