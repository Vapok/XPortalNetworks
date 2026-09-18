using UnityEngine;
using UnityEngine.Rendering;
using Jotunn.Managers;

namespace XPortalNetworks
{
    internal static class Environment
    {
        private static bool? _isHeadless;

        /// <summary>
        /// Are we the Server?
        /// </summary>
        /// <returns>True if ZNet says we are a server</returns>
        internal static bool IsServer
        {
            get
            {
                return ZNet.instance != null && ZNet.instance.IsServer();
            }
        }

        /// <summary>
        /// Are we Headless? (dedicated server)
        /// </summary>
        /// <returns>True if SystemInfo.graphicsDeviceType is not set</returns>
        internal static bool IsHeadless
        {
            get
            {
                if (_isHeadless.HasValue)
                    return _isHeadless.Value;

                try
                {
                    if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null)
                    {
                        _isHeadless = true;
                        return true;
                    }

                    _isHeadless = GUIManager.IsHeadless();
                }
                catch
                {
                    _isHeadless = SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;
                }

                return _isHeadless.Value;
            }
        }

        /// <summary>
        /// Has the Game started? Set via a patch on Game.Start
        /// </summary>
        internal static bool GameStarted { get; set; } = false;

        /// <summary>
        /// Is the Game shutting down? This happens on logout and on quit.
        /// </summary>
        internal static bool ShuttingDown
        {
            get
            {
                return Game.instance.m_shuttingDown;
            }
        }

        /// <summary>
        /// The PeerID of the server
        /// </summary>
        internal static long ServerPeerId
        {
            get
            {
                return ZRoutedRpc.instance.GetServerPeerID();
            }
        }
    }
}
