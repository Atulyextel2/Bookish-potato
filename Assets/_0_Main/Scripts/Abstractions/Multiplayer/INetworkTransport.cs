using System;

public enum NetMsgType : byte
{
    // Lobby / lifecycle
    Heartbeat = 0,
    PlayerJoined = 1,
    PlayerLeft = 2,
    GameConfig = 3,  // host -> all (difficulty, seed, board size)
    StartGame = 4,  // host -> all (optional)

    // Gameplay (aggregated, no per-flip spam)
    MatchFound = 10, // client -> host, host -> all (validated)
    ScoreSync = 11, // host -> all (authoritative full or delta)

    // Chat & meta
    Chat = 20, // text-only
    GameOver = 30, // host -> all (final standings, top 3)
    Ping = 250,
    Pong = 251,

    // Versioning / future
    Custom = 255
}

[Serializable] public struct MsgGameConfig { public string difficulty; public int seed; public int boardRows; public int boardCols; public string hostPeerId; }
[Serializable] public struct MsgMatchFound { public string playerId; public int pairId; public int newScore; }
[Serializable] public struct MsgScoreSync { public string[] playerIds; public int[] scores; public string leaderId; }
[Serializable] public struct MsgChat { public string playerId; public string text; public long tsUtc; }
[Serializable] public struct MsgGameOver { public string[] ranking; /* top->bottom */ }

public interface INetworkTransport : IDisposable
{
    event Action<string /*peerId*/, ArraySegment<byte>> OnData; // raw payload
    string LocalPeerId { get; }
    bool IsHost { get; }

    void CreateRoom(Action<string /*roomId*/> onCreated);
    void JoinRoom(string roomId, Action<bool> onJoined);
    void SendTo(string peerId, ArraySegment<byte> data, bool reliable = true);
    void Broadcast(ArraySegment<byte> data, bool reliable = true);
    void LeaveRoom();
}

public interface IMessageRouter
{
    void Register(NetMsgType type, Action<string /*peerId*/, ArraySegment<byte>> handler);
    void Unregister(NetMsgType type, Action<string, ArraySegment<byte>> handler);
    void HandleIncoming(string fromPeerId, ArraySegment<byte> data);
}