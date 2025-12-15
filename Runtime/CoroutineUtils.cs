using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bnj.utility_toolkit.Runtime.Coroutines
{
    public static class CoroutineUtils
    {
        public static Coroutine DelayInvoke(this MonoBehaviour behavior, Action action, YieldInstruction yieldInstruction)
        {
            if (!behavior.isActiveAndEnabled) return null;
            return behavior.StartCoroutine(InvokeAfterSeconds());
            IEnumerator InvokeAfterSeconds()
            {
                yield return yieldInstruction;
                action?.Invoke();
            }
        }

        public static Coroutine DelayInvoke(this MonoBehaviour behavior, Action action, float delay)
        {
            return behavior.DelayInvoke(action, new WaitForSeconds(delay));
        }

        public static Coroutine DelayInvokeWhile(this MonoBehaviour behavior, Action action, Func<bool> condition)
        {
            if (!behavior.isActiveAndEnabled) return null;
            return behavior.StartCoroutine(InvokeActionWhenConditionIsFalse());
            IEnumerator InvokeActionWhenConditionIsFalse()
            {
                yield return new WaitWhile(condition);
                action?.Invoke();
            }
        }

        public static Coroutine DelayInvokeUntil(this MonoBehaviour behavior, Action action, Func<bool> condition)
        {
            if (!behavior.isActiveAndEnabled) return null;
            return behavior.StartCoroutine(InvokeActionWhenConditionIsTrue());
            IEnumerator InvokeActionWhenConditionIsTrue()
            {
                yield return new WaitUntil(condition);
                action?.Invoke();
            }
        }

        static YieldInstruction GetYieldFrom(float interval) => interval > 0 ? new WaitForSeconds(interval) : null;
        static float GetDeltaTimeFrom(float interval) => interval > 0 ? interval : Time.deltaTime;

        public static Coroutine RepeatInvoke(this MonoBehaviour behavior, Action<float> action, float interval = 0)
        {
            if (!behavior.isActiveAndEnabled) return null;
            return behavior.StartCoroutine(InvokeActionEveryInterval());
            IEnumerator InvokeActionEveryInterval()
            {
                while (behavior.isActiveAndEnabled)
                {
                    yield return GetYieldFrom(interval);
                    action?.Invoke(GetDeltaTimeFrom(interval));
                }
            }
        }

        public static Coroutine RepeatInvoke(this MonoBehaviour behavior, Action action, YieldInstruction yieldInstruction)
        {
            if (!behavior.isActiveAndEnabled) return null;
            return behavior.StartCoroutine(InvokeActionAfterEveryCustomYieldInstruction());
            IEnumerator InvokeActionAfterEveryCustomYieldInstruction()
            {
                while (behavior.isActiveAndEnabled)
                {
                    yield return yieldInstruction;
                    action?.Invoke();
                }
            }
        }

        public static Coroutine RepeatInvokeWhile(this MonoBehaviour behavior, Action<float> action, Func<bool> condition, float interval = 0)
        {
            if (!behavior.isActiveAndEnabled) return null;
            return behavior.StartCoroutine(InvokeActionEveryIntervalUntil());
            IEnumerator InvokeActionEveryIntervalUntil()
            {
                while (behavior.isActiveAndEnabled && condition())
                {
                    yield return GetYieldFrom(interval);
                    action?.Invoke(GetDeltaTimeFrom(interval));
                }
            }
        }
    }

    public class AnimationSequence
    {
        private readonly MonoBehaviour _context;
        private readonly List<(float time, Action action)> _steps = new();
        private float _time = 0f;

        private AnimationSequence(MonoBehaviour context) => _context = context;

        public static AnimationSequence Create(MonoBehaviour context) => new(context);
        public static AnimationSequence CreateDelayedAction(MonoBehaviour context, float delay, Action action)
        {
            return Create(context).Wait(delay).Then(action);
        }

        public AnimationSequence Wait(float seconds)
        {
            _time += seconds;
            return this;
        }

        public AnimationSequence Then(Action action)
        {
            _steps.Add((_time, action));
            return this;
        }

        public void Run()
        {
            foreach (var (time, action) in _steps)
                _context.DelayInvoke(action, time);
        }
    }
}

