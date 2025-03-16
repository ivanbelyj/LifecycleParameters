using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// Class that allows Mirror to serialize lifecycle effects
/// and transport they by network
/// </summary>
public static class LifecycleEffectReadWrite
{
    public static void WriteLifecycleEffect(
        this NetworkWriter writer,
        LifecycleEffect effect) {

        writer.WriteFloat(effect.duration);
        writer.WriteBool(effect.isInfinite);
        writer.WriteFloat(effect.speed);

        writer.WriteString(effect.targetParameterId);
        writer.WriteDouble(effect.StartTime);
    }

    public static LifecycleEffect ReadLifecycleEffect(this NetworkReader reader) {
        
        float duration = reader.ReadFloat();
        bool isInfinite = reader.ReadBool();
        float speed = reader.ReadFloat();

        string targetParameterId = reader.ReadString();
        double startTime = reader.ReadDouble();

        LifecycleEffect effect = new LifecycleEffect() {
            speed = speed,
            isInfinite = isInfinite,
            targetParameterId = targetParameterId,
            duration = duration,
            StartTime = startTime,
        };
        return effect;
    }
}
