﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hadouken.Events.BitTorrent
{
    public interface ITorrentEventListener : IEventListener
    {
        void OnAdded(Action<Torrent> callback);
        void OnRemoved(Action<string> callback);
        void OnError(Action<Torrent> callback);
        void OnCompleted(Action<Torrent> callback);
        void OnMoved(Action<Torrent> callback);
    }
}