v0.2.1

# Release Notes - API Related

## Base Classes
### These classes are to be sub-classed.

Added base class `BoolPreference`
Added base class `StringPreference`
Added base class `IntPreference`
Added base class `FloatPreference`

# Release Notes - Other

## Bugs

Fixed a bug where version 1.1.1 ( Thanks Giving Update ) would return a null version.

v0.2.2

# Release Notes - Other

## UI

- Changed how the Main Menu is displayed.
- Added a warning message when BepInEx's caching is enabled.

## Other

- DLL names will stay consistant with KitchenLib-BepInEx and KitchenLib-MelonLoader (Inconsistent names have been causing problems with BepInEx)
- Assembly names will match up with DLL names (Inconsistent names have been causing problems with BepInEx)
- Fixed a bug causing debug builds of PlateUp! to throw an exception when loading the mod.

v0.2.3

# Release Notes - API Related

# General

- Added support for Workshop Mods
- Updated from 4.7.1 to 4.7.2

## Base Classes

- Added base class `CustomUnlockPack`
- Added base class `CustomCompositeUnlockPack`
- Added base class `CustomModularUnlockPack`

- Updated variables in `CustomDish`
- Updated variables in `CustomItem`
- Updated variables in `CustomItemGroup`
- Updated variables in `CustomUnlock`
- Updated variables in `CustomUnlockCard`

## Utils

- Updated `ReflectionUtils` to access more variables.

## Events

- Added `PerformInitialSetupEvent`

# Release Notes - Other

## Performance

- Updated most of our reflection use to use `ReflectionUtils` to keep caching consistant. (This should improve performance with a larger amount of mods)

## Preferences

- Preferences are now stored in `%localappdata%low/It's Happening/PlateUp/UserData/KitchenLib/preferences.dat` by default.
- Preferences will now detect, and move old preferences to the new location.

## Bugs

- Fixed a bug causing the mod to crash when generating GameData more than once in a session.
- Fixed a bug causing the Main Menu to crash in some cases.

v0.2.4

# Release Notes - API Related

## References

- Updated `ApplianceReference`
- Updated `ItemReference`
- Updated `ProcessReference`
- Added `CompositeUnlockPackReference`
- Added `CrateSetReference`
- Added `DecorReference`
- Added `DishReference`
- Added `EffectReference`
- Added `EffectRepresentationReference`
- Added `GardenProfileReference`
- Added `ItemReference`
- Added `ItemGroupReference`
- Added `LayourProfileReference`
- Added `LevelUpgradeSetReference`
- Added `ModularUnlockPackReference`
- Added `PlayerCosmeticReference`
- Added `ProcessReference`
- Added `RandomUpgradeSetReference`
- Added `ResearchReference`
- Added `ShopReference`
- Added `ThemeUnlockReference`
- Added `UnlockReference`
- Added `UnlockCardReference`
- Added `UnlockPackReference`
- Added `WorkshopRecipeReference`

v0.2.5

# Release Notes - API Related

# Registration

- Removed Obsolete `RegisterCustomAppliance<T>`
- Removed Obsolete `AddSystem<T>`
- Removed Obsolete `AddAppliance<T>`
- Removed Obsolete `AddProcess<T>`

## Base Classes

Fixed `BaseGameDataObjectID` not working for most base classes.

### BaseMod

- Rewrote `BaseMod` to be more consistant across mod loaders.

### CustomDish

- Removed Obsolete `UnlocksMenuItems`
- Removed Obsolete `UnlocksIngredients`
- Removed Obsolete `PrerequisiteDishes`

### CustomItemGroup

- Removed Obsolete `DerivedSets`

### CustomUnlock

- Removed Obsolete `Requires`
- Removed Obsolete `BlockedBy`

## References

- Updated all reference names

## Utilities

### ResourceUtils

- Added string FindModPath(Assembly assembly, bool isNonWorkshop = false)

### SystemUtils

- Removed Obsolete `AddSystem<T>`
- Removed Obsolete `NewSystem<T1, T2>`

### CustomGDO

- Removed Obsolete `GetCustomAppliance(int)`
- Removed Obsolete `GetCustomAppliance<T>`
- Removed Obsolete `GetCustomItem(int)`
- Removed Obsolete `GetCustomItem<T>`
- Removed Obsolete `GetCustomProcess(int)`
- Removed Obsolete `GetCustomProcess<T>`
- Removed Obsolete `GetGameDataObject(int)`
- Removed Obsolete `GetGameDataObject<T>`

### GDOUtils

- Removed Obsolete `GetExistingAppliance(int)`
- Removed Obsolete `GetExistingItem(int)`
- Removed Obsolete `GetExistingProcess(int)`
- Added `GetCustomGameDataObject(int)`
- Added `GetCustomGameDataObject<T>`

## Other

- Fixed a bug causing logging to not work when using the Workshop loader.
- Fixed a bug causing the console to have an unnessary warning when registering a `CustomGameDataObject`.

v0.2.6

# Release Notes - API Related

## Base Classes

### BaseMod

- Reimplemented obsolete constructors for temporary backwards compatibility.
- Changed `OnFrameUpdate()` and `OnInitialise()` from `abstract` to `virtual` for temporary backwards compatibility.

v0.2.7

# Release Notes - API Related

## Utilities

### MaterialUtils

- Added `ApplyMaterial<T>(GameObject prefab, string path, Material[] materials)` to allow for different renderer types ie. `SkinnedMeshRenderer`

### ResourceUtils

- Added `AssetBundleLocation` argument to `FindModPath`

v0.2.8

# Release Notes - Other

## Bugs

- Fixed a bug causing Harmony to patch twice when trying to load into multiplayer causing a soft-lock

v0.2.9

# Release Notes - API Related

## Utilities

### ResourceUtils

- `FindModPath(Assembly assembly, AssetBundleLocation location)` is now obsolete.
- Added `GetModsFolder()`
- Added `GetWorkshopFolder()`

## Bugs

- Fixed a bug causing `OnFrameUpdate()` to not get called after joining a multiplayer session.

# Release Notes - Other

## UI

- Non-KitchenLib mods will now show up in the mod list when using the Workshop loader.

v0.3.0

# Release Notes - API Related

## Base Classes

### BaseMod

- Added `ModPostActivate(Mod mod)` for Workshop build.
- Added `ModPostInject()` for Workshop build.
- Added `ModPreInject()` for Workshop build.

## Utilities

### ResourceUtils

- `GetWorkshopFolder()` now only returns if using the workshop build.

## Bugs

- Fixed a bug from 0.2.9 causing progress to not load correctly on MelonLoader or BepInEx.

# Release Notes - Other

## UI

- Fixed Mods menu not displaying mods when using the Workshop loader.

v0.3.1

# Release Notes - API Related

## Base Classes

### BaseMod

- Fixed `OnInitialise()` being called too early

## Utilities

### VariousUtils

- Added `GetID(string name)` to generate a random unique ID.

# Release Notes - Other

v0.3.2

# Release Notes - API Related

## Base Classes

### CustomGardenProfile

- Removed `List<Item> Spawns`
- Added `List<GardenProfile.SpawnProbibility> Spawns`

### CustomItem

- Added `Item ExtendedDirtItem`

## References
- Updated references to contain new fields.