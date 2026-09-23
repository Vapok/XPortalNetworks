namespace XPortalNetworks
{
    internal static class NetPeerUtility
    {
        internal static long GetPeerPlayerId(long peerId)
        {
            if (ZNet.instance == null || ZDOMan.instance == null)
            {
                return 0L;
            }

            ZNetPeer peer = ZNet.instance.GetPeer(peerId);
            if (peer == null)
            {
                if (ZNet.instance.IsServer() && peerId == ZNet.GetUID())
                {
                    if (Player.m_localPlayer != null)
                    {
                        return Player.m_localPlayer.GetPlayerID();
                    }

                    return (Game.instance != null && Game.instance.GetPlayerProfile() != null)
                        ? Game.instance.GetPlayerProfile().GetPlayerID()
                        : 0L;
                }

                return 0L;
            }

            if (peer.m_characterID.IsNone())
            {
                return 0L;
            }

            ZDO characterZdo = ZDOMan.instance.GetZDO(peer.m_characterID);
            if (characterZdo == null)
            {
                return 0L;
            }

            return characterZdo.GetLong(ZDOVars.s_playerID);
        }

        internal static bool IsPeerPrivilegedForPortalNetwork(long peerId)
        {
            if (ZNet.instance == null)
            {
                return false;
            }

            ZNetPeer peer = ZNet.instance.GetPeer(peerId);
            if (peer == null)
            {
                if (ZNet.instance.IsServer() && peerId == ZNet.GetUID())
                {
                    return ZNet.instance.LocalPlayerIsAdminOrHost();
                }

                return false;
            }

            return peer.m_socket != null && ZNet.instance.IsAdmin(peer.m_socket.GetHostName());
        }
    }
}

