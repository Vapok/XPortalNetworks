using HarmonyLib;
using System.Collections.Generic;

namespace XPortalNetworks.Patches
{
    [HarmonyPatch(typeof(ZDOMan), nameof(ZDOMan.ConnectPortals))]
    static class ZDOMan_ConnectPortals
    {
        static ZDOID FindNewId(List<ZDOID> allPortals, ZDOID oldId)
        {
            foreach (ZDOID newId in allPortals)
            {
                ZDO newZdo = ZDOMan.instance.GetZDO(newId);

                if (newZdo == null) continue;

                ZDOID previousId = newZdo.GetZDOID(XPortalNetworks.Key_PreviousId);

                if (oldId == previousId)
                {
                    string portalName = ZdoTools.GetName(newZdo);
                    Log.Debug($"Old ZDOID `{oldId}` is now `{newId}` (`{portalName}`)");
                    return newId;
                }
            }

            return oldId;
        }

        static bool Prefix()
        {
            Log.Debug("Restoring Portal connections..");

            List<ZDOID> connectionIds1 = ZDOExtraData.GetAllConnectionZDOIDs(ZDOExtraData.ConnectionType.Portal);
            List<ZDOID> connectionIds2 = ZDOExtraData.GetAllConnectionZDOIDs(ZDOExtraData.ConnectionType.Portal | ZDOExtraData.ConnectionType.Target);

            List<ZDOID> allPortalIds = new List<ZDOID>();
            allPortalIds.AddRange(connectionIds1);
            allPortalIds.AddRange(connectionIds2);

            Log.Debug($"Found {allPortalIds.Count} portal(s).");

            if (allPortalIds.Count == 0) return false;

            foreach (ZDOID portalId in allPortalIds)
            {
                if (portalId == ZDOID.None) continue;

                ZDO portalZdo = ZDOMan.instance.GetZDO(portalId);

                if (portalZdo == null) continue;

                string portalName = portalZdo.GetString("tag");
                Log.Debug($"Checking connection for `{portalId}` (`{portalName}`)");

                ZDOID targetId = portalZdo.GetZDOID(XPortalNetworks.Key_TargetId);

                if (targetId == ZDOID.None) continue;

                ZDO targetZdo = ZDOMan.instance.GetZDO(targetId);

                if (targetZdo == null || targetZdo.GetZDOID(XPortalNetworks.Key_PreviousId) == ZDOID.None)
                {
                    Log.Debug($"Target `{targetId}` does not exist, finding new ZDOID..");
                    targetId = FindNewId(allPortalIds, targetId);
                    targetZdo = ZDOMan.instance.GetZDO(targetId);
                }

                if (targetZdo == null)
                {
                    Log.Debug($"Target `{targetId}` could not be found by its PreviousId either. Skipping..");
                    continue;
                }

                string targetPortalName = targetZdo.GetString("tag");
                Log.Info($"Connecting: `{portalId}` (`{portalName}`)  ==>  `{targetId}` (`{targetPortalName}`)");

                portalZdo.SetOwner(ZDOMan.GetSessionID());
                portalZdo.SetConnection(ZDOExtraData.ConnectionType.Portal, targetId);
                portalZdo.Set(XPortalNetworks.Key_TargetId, targetId);
            }

            Log.Debug("Updating PreviousId for all portals..");
            foreach (ZDOID portalId in allPortalIds)
            {
                ZDO portalZdo = ZDOMan.instance.GetZDO(portalId);
                if (portalZdo != null)
                {
                    portalZdo.Set(XPortalNetworks.Key_PreviousId, portalId);
                }
            }

            return false;
        }
    }
}
