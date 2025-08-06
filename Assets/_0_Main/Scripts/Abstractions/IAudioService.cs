// SoundType.cs
public enum SoundType
{
    Flip,
    Match,
    Mismatch,
    GameOver,
    Click
}

public interface IAudioService
{
    void Play(SoundType type);
}