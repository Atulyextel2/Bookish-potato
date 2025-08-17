using System;
using System.Collections.Generic;

public sealed class MessageRouter : IMessageRouter
{
    private readonly Dictionary<NetMsgType, Action<string, ArraySegment<byte>>> _handlers = new();

    private static ArraySegment<byte> SlicePayload(ArraySegment<byte> data)
        => new ArraySegment<byte>(data.Array!, data.Offset + 1, data.Count - 1);

    public void Register(NetMsgType type, Action<string, ArraySegment<byte>> handler)
    {
        if (_handlers.TryGetValue(type, out var existing)) _handlers[type] = existing + handler;
        else _handlers[type] = handler;
    }

    public void Unregister(NetMsgType type, Action<string, ArraySegment<byte>> handler)
    {
        if (_handlers.TryGetValue(type, out var existing)) _handlers[type] = existing - handler;
    }

    public void HandleIncoming(string fromPeerId, ArraySegment<byte> data)
    {
        // [0] = NetMsgType
        if (data.Count < 1) return;
        var type = (NetMsgType)data.Array![data.Offset];
        if (_handlers.TryGetValue(type, out var h)) h?.Invoke(fromPeerId, SlicePayload(data));
    }
}