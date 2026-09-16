# 2.0.2 - Unified Splash Screen & Telemetry Controls
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
