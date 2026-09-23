using HarmonyLib;
using UnityEngine;

namespace XPortalNetworks.Patches
{
    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.OnPlaced))]
    static class WearNTear_OnPlaced
    {
        internal static void Postfix(WearNTear __instance)
        {
            if (__instance == null)
                return;

            Piece piece = __instance.GetComponent<Piece>();
            ZNetView nview = __instance.GetComponent<ZNetView>();
            if (piece != null && !string.IsNullOrEmpty(piece.m_name) && piece.m_name.Contains("$piece_portal") && nview != null)
            {
                ZDO portalZDO = nview.GetZDO();
                if (portalZDO == null)
                {
                    Log.Error("A portal was placed but the ZDO is not available");
                    return;
                }

                ZDOID portalId = portalZDO.m_uid;
                Vector3 location = portalZDO.GetPosition();
                XPortalNetworks.OnPortalPlaced(portalId, location);
            }
        }
    }

    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.Destroy))]
    static class WearNTear_Destroy
    {
        static void Prefix(WearNTear __instance)
        {
            if (__instance == null)
                return;

            Piece piece = __instance.m_piece;
            if (piece == null)
                return;

            ZNetView nview = piece.m_nview;
            if (nview == null)
                return;

            if (!string.IsNullOrEmpty(piece.m_name) && piece.m_name.Contains("$piece_portal") && piece.CanBeRemoved())
            {
                ZDO portalZDO = nview.GetZDO();
                if (portalZDO == null)
                {
                    Log.Error("A portal was destroyed but the ZDO is not available");
                    return;
                }

                ZDOID portalId = portalZDO.m_uid;
                XPortalNetworks.OnPortalDestroyed(portalId);
            }
        }
    }
}
