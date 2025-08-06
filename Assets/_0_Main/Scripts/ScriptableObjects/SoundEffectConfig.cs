// SoundEffectConfig.cs
using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundEffectConfig", menuName = "CardMatch/SoundEffectConfig")]
public class SoundEffectConfig : ScriptableObject
{
    [Serializable]
    public struct SoundEffect
    {
        public SoundType type;
        public AudioClip clip;
    }

    [Tooltip("Assign one AudioClip per SoundType here")]
    public List<SoundEffect> effects = new List<SoundEffect>();
}