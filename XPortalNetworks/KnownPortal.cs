using System;
using System.IO;
using UnityEngine;
using XPortalNetworks.Extension;

namespace XPortalNetworks
{
    public class KnownPortal : IEquatable<KnownPortal>
    {
        public ZDOID Id { get; set; }
        public string Name { get; set; }
        public ZDOID PreviousId { get; set; }
        public ZDOID Target { get; set; }
        public Vector3 Location { get; set; }
        public string Colour { get; set; }

        public long NetworkOwnerPlayerId { get; set; }

        public string NetworkOwnerDisplayName { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsDefaultPortal
        {
            get
            {
                return Location.Round().Equals(XPortalNetworksConfig.Instance.Local.DefaultPortal.Value.Round());
            }
        }

        public KnownPortal(ZDOID id)
        {
            Id = id;
            Name = string.Empty;
            Location = Vector3.zero;
            PreviousId = ZDOID.None;
            Target = KnownPortalsManager.Instance.FindDefaultPortal();
            Colour = PortalColour.GetPortalColour(id);
            NetworkOwnerPlayerId = 0L;
            NetworkOwnerDisplayName = string.Empty;
            IsPrivate = false;
        }

        public KnownPortal(ZDOID id, Vector3 location) : this(id)
        {
            Location = location;
            IsPrivate = XPortalNetworksConfig.Instance.Local.DefaultPrivatePortal.Value;
        }

        public KnownPortal(ZPackage pkg)
        {
            Id = pkg.ReadZDOID();
            Name = pkg.ReadString();
            Location = pkg.ReadVector3();
            PreviousId = pkg.ReadZDOID();
            Target = pkg.ReadZDOID();
            Colour = pkg.ReadString();
            NetworkOwnerPlayerId = pkg.ReadLong();
            NetworkOwnerDisplayName = ReadOptionalString(pkg);
            IsPrivate = ReadOptionalBool(pkg);
        }

        private static string ReadOptionalString(ZPackage pkg)
        {
            try
            {
                return pkg.ReadString();
            }
            catch (EndOfStreamException)
            {
                return string.Empty;
            }
        }

        private static bool ReadOptionalBool(ZPackage pkg)
        {
            try
            {
                return pkg.ReadBool();
            }
            catch (EndOfStreamException)
            {
                return false;
            }
        }

        public string GetFriendlyName()
        {
            string portalName = Name;
            if (string.IsNullOrEmpty(portalName))
            {
                return Localization.instance.Localize("$piece_portal_tag_none");
            }
            else
            {
                return portalName;
            }
        }

        public string GetFriendlyTargetName()
        {
            if (!HasTarget())
            {
                return Localization.instance.Localize("$piece_portal_target_none");
            }

            KnownPortal targetPortal = KnownPortalsManager.Instance.GetKnownPortalById(Target);
            if (targetPortal == null)
            {
                return $"{Target} (invalid)";
            }

            return targetPortal.GetFriendlyName();
        }

        public bool HasTarget()
        {
            return Target != ZDOID.None && !Target.IsNone();
        }

        public ZPackage Pack()
        {
            ZPackage pkg = new ZPackage();
            pkg.Write(Id);
            pkg.Write(Name);
            pkg.Write(Location);
            pkg.Write(PreviousId);
            pkg.Write(Target);
            pkg.Write(Colour);
            pkg.Write(NetworkOwnerPlayerId);
            pkg.Write(NetworkOwnerDisplayName ?? string.Empty);
            pkg.Write(IsPrivate);
            return pkg;
        }

        public bool Targets(ZDOID target)
        {
            return Target == target;
        }

        public override string ToString()
        {
            return $"{{ Id: `{Id}`, Name: `{GetFriendlyName()}`, Location: `{Location}`, NetworkOwner: `{NetworkOwnerPlayerId}` (`{NetworkOwnerDisplayName}`), Private: `{IsPrivate}`, Target: `{Target}` (`{GetFriendlyTargetName()}`), Colour: `{Colour}` }}";
        }

        public bool IsGlobalNetwork()
        {
            return NetworkOwnerPlayerId == 0L;
        }

        public bool Equals(KnownPortal other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as KnownPortal);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

