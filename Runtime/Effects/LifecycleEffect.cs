using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

///<summary>
/// Effect applied to the lifecycle parameter
///</summary>
[Serializable]
public struct LifecycleEffect : IEquatable<LifecycleEffect>
{
    public bool isInfinite;

    /// <summary>
    /// Value, added / subtracted from the parameter value per second
    /// </summary>
    [Tooltip("Value, added / subtracted from the parameter value per second")]
    public float speed;
    
    /// <summary>
    /// Effect duration in seconds
    /// </summary>
    [Tooltip("Effect duration in seconds")]
    public float duration;
    
    /// <summary>
    /// Every effect is applied by parameter id
    /// </summary>
    public uint targetParameterId;

    public double StartTime { get; set; }

    public bool IsPassed => StartTime + duration <= NetworkTime.time;

    public static LifecycleEffect CreateForDelta(
        uint targetParameterId,
        float delta,
        float duration) {
        return new() {
            targetParameterId = targetParameterId,
            duration = duration,
            speed = delta / duration,
            isInfinite = false
        };
    }

    public override bool Equals(object obj)
    {
        return obj is LifecycleEffect effect && Equals(effect);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(isInfinite, speed, duration, targetParameterId, StartTime);
    }

    public bool Equals(LifecycleEffect other) {
        return isInfinite == other.isInfinite
            && duration == other.duration
            && speed == other.speed
            && targetParameterId == other.targetParameterId
            && StartTime == other.StartTime;
    }

    public override string ToString()
    {
        return "{" + $" isInfinite: {isInfinite}; speed: {speed}; duration: {duration}; "
            + $"targetParameter: {targetParameterId}; StartTime: {StartTime}" + "}";
    }
}
