using Jotunn.Managers;

namespace XPortalNetworks
{
    internal static class Environment
    {
        private static bool? _isHeadless;

        internal static bool IsServer
        {
            get
            {
                return ZNet.instance != null && ZNet.instance.IsServer();
            }
        }

        internal static bool IsHeadless
        {
            get
            {
                if (_isHeadless.HasValue)
                    return _isHeadless.Value;

                _isHeadless = GUIManager.IsHeadless();
                return _isHeadless.Value;
            }
        }

        internal static bool GameStarted { get; set; } = false;

        internal static bool ShuttingDown
        {
            get
            {
                return Game.instance == null || Game.instance.m_shuttingDown;
            }
        }

        internal static long ServerPeerId
        {
            get
            {
                return ZRoutedRpc.instance != null ? ZRoutedRpc.instance.GetServerPeerID() : 0L;
            }
        }
    }
}

