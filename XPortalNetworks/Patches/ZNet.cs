using HarmonyLib;
using XPortalNetworks.RPC;

namespace XPortalNetworks.Patches
{
    [HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
    internal static class ZNet_RPC_PeerInfo_Postfix
    {
        private static void Postfix(ZNet __instance)
        {
            if (__instance == null || __instance.IsServer())
            {
                return;
            }

            XPortalNetworksAdminSync.RequestFromServerIfNeeded();
        }
    }
}
