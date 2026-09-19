# 2.0.8 - Valheim 1.0.15 Alignment & Internalized Dependency Updates
* **Valheim 1.0.15 Alignment**:
  * Aligned publicized game assembly and UnityEngine references to Valheim 1.0.15.
  * Updated internalized `Vapok.Valheim.Common` dependency to 3.13.1015.
* **Stability & Localization**:
  * Synchronized all 35 game localizations for splash screen and configuration registry.
  * Audited network RPCs, ZDO portal mappings, and headless UI isolation against game version 1.0.15.

# 2.0.7 - Scene Transition & Portal Target Exception Hardening
* **Scene Transition Exception Resolution**:
  * Fixed `ArgumentException: The scene is invalid` thrown by `Environment.IsHeadless` when queried during active scene loading and logout transitions.
  * Cached headless state in `Environment.IsHeadless` and implemented a protected fallback to `SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null`.
  * Updated `PortalConfigurationPanel.InitialiseUI()` to reference the cached `Environment.IsHeadless` property rather than querying `GUIManager.IsHeadless()` directly.
* **Portal Lookup Null-Safety & Dictionary Resilience**:
  * Replaced unsafe dictionary indexer (`knownPortals[id]`) in `KnownPortalsManager.GetKnownPortalById(ZDOID id)` with `knownPortals.TryGetValue(id, out var portal) ? portal : null` to avoid `KeyNotFoundException`.
  * Added null guards across all callers (`KnownPortal.GetFriendlyTargetName()`, `XPortalNetworks.OnPrePortalHover()`, `XPortalNetworks.OnPortalRequestText()`, `XPortalNetworks.OnPortalDestroyed()`, `ServerEvents.RPC_AddOrUpdateRequest()`, and `PortalConfigurationPanel.ResolveInitialDestinationNetworkOwnerId()`).
* **Map Ping Hardening**:
  * Guarded `SendToClient.PingMap()` against null `ZRoutedRpc.instance` and null `UserInfo.GetLocalUser()` instances.
  * Added exception handling and fallback name string assignment to prevent UI cancellation during map ping requests.

# 2.0.6 - Splash Window Updates & Valheim 1.0.14 Alignment
* **Splash Window Updates**:
  * Updated telemetry default to unchecked on first launch (Opt-In).
  * Added Send Error Logs toggle (Opt-Out) to capture anonymous crash diagnostics and error reports.
  * Added in-game scrollable Privacy Policy overlay with responsive mouse wheel support.
  * Added interactive tooltip data disclaimers on checkbox hover.
* **Valheim 1.0.14 Alignment**:
  * Aligned publicized game assembly and UnityEngine references to Valheim 1.0.14.
  * Updated internalized Vapok.Valheim.Common dependency to 3.12.1014.

# 2.0.5 - Jewelcrafting Font Compatibility
* **Compatibility Fix**: Fixed issue where Jewelcrafting packages its own font which was overriding part of a vanilla font, causing the Splash screen to appear blank.
* **Vapok.Common Dependency Bump**: Updated internalized dependency to `Vapok.Valheim.Common` 3.11.1012.

# 2.0.4 - Updated README with Telemetry Information
* **Documentation Update**: Updated the README.md with Anonymous Telemetry and Privacy section per request of mod stores.
* **Vapok.Common Dependency Bump**: Updated internalized dependency to `Vapok.Valheim.Common` 3.9.1012.

# 2.0.3 - Unified Splash Screen & Telemetry Controls
* **Unified Startup Splash Screen & Telemetry**:
  * Updated `Vapok.Valheim.Common` dependency reference to `v3.5.1012`.
  * Registered mod metadata with centralized `ModSplashManager`.
  * Added `ShowSplashOnStartup` and `Enable Anonymous Telemetry` configuration bindings to `ConfigRegistry`.

# 2.0.1 - Dependency & Compatibility Maintenance
* **Runtime & Dependency Updates**:
  * Synchronized package manifest and project references with Jotunn `2.30.0` and BepInEx `5.4.2350`.
  * Verified build pipeline and ILRepack bundling with `Vapok.Valheim.Common` `3.2.1012`.
* **Compatibility & Documentation**:
  * Validated portal destination selection UI and network configuration hot-reloading against current Valheim 1.0 builds.
  * Standardized mod documentation, changelog tiers, and release staging.

# 2.0.0 - Portal Networks & Valheim 1.0+ Overhaul
* **Portal Networks Architecture**:
  * Overhauled portal mechanics to introduce an expansive multi-tier **Portal Networks** system:
    * **Global / Public Network**: Accessible to all players on the server without restriction.
    * **Player Networks & Private Portals**: Dedicated per-player network channels with private portal protection to restrict unauthorized access.
    * **Custom Named Networks**: Dynamic support for up to 15 server-defined custom networks configured in `xportal_networks.json` with live hot-reloading support.
* **Server Administration & Permission Controls**:
  * Implemented permission checks restricting portal deconstruction and destruction to the original creator or authenticated server admins.
  * Synchronized portal network configurations and permission sets strictly across dedicated servers via Jotunn ServerSync.
* **Valheim 1.0 Compatibility & Core Updates**:
  * Updated assembly references for Valheim 1.0 (`1.0.12`), BepInEx 5.4.2350, and Jotunn 2.30.0.
  * Rebuilt on .NET Framework 4.8.
  * Bundled `Vapok.Valheim.Common` 3.2.1012 via ILRepack.
* **UI, Gamepad & Networking Fixes**:
  * Resolved controller legend rendering artifacts and gamepad input focus issues in portal configuration dialogs.
  * Fixed dedicated server admin portal destruction permission validation.
  * Improved ZDO network key synchronization and portal pairing resolution to eliminate connection dropouts under high network load.

# 1.0.0 - Initial Portal Management Release
* Initial release of portal grouping and tag management mechanics.
