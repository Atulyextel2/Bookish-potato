using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : IAudioService
{
    readonly Dictionary<SoundType, AudioClip> _clips;
    readonly AudioSource _source;

    public AudioManager(SoundEffectConfig config, AudioSource source)
    {
        _source = source;
        _clips = config.effects.ToDictionary(e => e.type, e => e.clip);
    }

    public void Play(SoundType type)
    {
        if (_clips.TryGetValue(type, out var clip) && clip != null)
            _source.PlayOneShot(clip);
    }
}