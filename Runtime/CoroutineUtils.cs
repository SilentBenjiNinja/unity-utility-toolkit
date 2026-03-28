using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bnj.utility_toolkit.Runtime.Coroutines
{
    /// <summary>
    /// Extension methods on <see cref="MonoBehaviour"/> for deferred, repeated, and conditional action invocation via coroutines.
    /// All methods return the started <see cref="Coroutine"/> so it can be stopped if needed.
    /// Methods are no-ops when the behaviour is inactive or disabled.
    /// </summary>
    public static class CoroutineUtils
    {
        /// <summary>Invokes <paramref name="action"/> after the given <paramref name="yieldInstruction"/> completes.</summary>
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

        /// <summary>Invokes <paramref name="action"/> after <paramref name="delay"/> seconds.</summary>
        public static Coroutine DelayInvoke(this MonoBehaviour behavior, Action action, float delay)
        {
            return behavior.DelayInvoke(action, new WaitForSeconds(delay));
        }

        /// <summary>Invokes <paramref name="action"/> once <paramref name="condition"/> becomes <see langword="false"/>.</summary>
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

        /// <summary>Invokes <paramref name="action"/> once <paramref name="condition"/> becomes <see langword="true"/>.</summary>
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

        /// <summary>
        /// Invokes <paramref name="action"/> every <paramref name="interval"/> seconds while the behaviour is active.
        /// Passes the elapsed delta time (or <paramref name="interval"/>) to the action. Pass <c>0</c> to tick every frame.
        /// </summary>
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

        /// <summary>Invokes <paramref name="action"/> after each <paramref name="yieldInstruction"/> while the behaviour is active.</summary>
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

        /// <summary>
        /// Invokes <paramref name="action"/> every <paramref name="interval"/> seconds while the behaviour is active
        /// and <paramref name="condition"/> returns <see langword="true"/>.
        /// </summary>
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

    /// <summary>
    /// Fluent builder for scheduling a sequence of timed actions on a <see cref="MonoBehaviour"/>.
    /// Actions are dispatched via <see cref="CoroutineUtils.DelayInvoke"/> when <see cref="Run"/> is called.
    /// </summary>
    public class AnimationSequence
    {
        private readonly MonoBehaviour _context;
        private readonly List<(float time, Action action)> _steps = new();
        private float _time = 0f;

        private AnimationSequence(MonoBehaviour context) => _context = context;

        /// <summary>Creates a new sequence bound to <paramref name="context"/>.</summary>
        public static AnimationSequence Create(MonoBehaviour context) => new(context);

        /// <summary>Creates a sequence with a single delayed action and calls <see cref="Run"/> immediately.</summary>
        public static AnimationSequence CreateDelayedAction(MonoBehaviour context, float delay, Action action)
        {
            return Create(context).Wait(delay).Then(action);
        }

        /// <summary>Advances the sequence clock by <paramref name="seconds"/>.</summary>
        public AnimationSequence Wait(float seconds)
        {
            _time += seconds;
            return this;
        }

        /// <summary>Schedules <paramref name="action"/> at the current sequence time.</summary>
        public AnimationSequence Then(Action action)
        {
            _steps.Add((_time, action));
            return this;
        }

        /// <summary>Starts all scheduled coroutines on the bound <see cref="MonoBehaviour"/>.</summary>
        public void Run()
        {
            foreach (var (time, action) in _steps)
                _context.DelayInvoke(action, time);
        }
    }
}

