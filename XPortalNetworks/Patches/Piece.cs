using HarmonyLib;
using XPortalNetworks.RPC;

namespace XPortalNetworks.Patches
{
    [HarmonyPatch(typeof(Piece), nameof(Piece.SetCreator))]
    static class Piece_SetCreator
    {
        private static WearNTear m_WearNTear;

        static void Postfix(Piece __instance)
        {
            if (__instance == null)
                return;

            m_WearNTear = __instance.GetComponent<WearNTear>();
            CheckWearNTearCreationTime();
        }

        private static void CheckWearNTearCreationTime(bool delayed = true, object state = null)
        {
            if (delayed)
            {
                QueuedAction.Queue(CheckWearNTearCreationTime, delay: 1);
                return;
            }

            if (m_WearNTear != null && m_WearNTear.m_createTime == -1f)
            {
                Log.Debug("Portal detection work-around: manually invoking WearNTear.OnPlace postfix");
                WearNTear_OnPlaced.Postfix(m_WearNTear);
                m_WearNTear = null;
            }
        }
    }

    [HarmonyPatch(typeof(Piece), nameof(Piece.CanBeRemoved))]
    static class Piece_CanBeRemoved
    {
        static void Postfix(Piece __instance, ref bool __result)
        {
            if (__instance == null || string.IsNullOrEmpty(__instance.m_name) || !__instance.m_name.Contains("$piece_portal"))
            {
                return;
            }

            if (!XPortalNetworksConfig.Instance.Server.RestrictPortalRemoval)
            {
                return;
            }

            __result = CanRemovePortal(__instance);
        }

        static bool CanRemovePortal(Piece piece)
        {
            if (piece == null)
                return false;

            if (ZNet.instance != null && (ZNet.instance.LocalPlayerIsAdminOrHost() || XPortalNetworksAdminSync.IsLocalPortalNetworkAdmin()))
                return true;

            return piece.IsCreator();
        }
    }
}
