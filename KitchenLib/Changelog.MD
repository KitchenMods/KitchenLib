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

Added support for Workshop Mods
Updated from 4.7.1 to 4.7.2

## Base Classes

Added base class `CustomUnlockPack`
Added base class `CustomCompositeUnlockPack`
Added base class `CustomModularUnlockPack`

Updated variables in `CustomDish`
Updated variables in `CustomItem`
Updated variables in `CustomItemGroup`
Updated variables in `CustomUnlock`
Updated variables in `CustomUnlockCard`

## Utils

Updated `ReflectionUtils` to access more variables.

## Events

Added `PerformInitialSetupEvent`

# Release Notes - Other

## Performance

Updated most of our reflection use to use `ReflectionUtils` to keep caching consistant. (This should improve performance with a larger amount of mods)

## Preferences

Preferences are now stored in `%localappdata%low/It's Happening/PlateUp/UserData/KitchenLib/preferences.dat` by default.
Preferences will now detect, and move old preferences to the new location.

## Bugs

Fixed a bug causing the mod to crash when generating GameData more than once in a session.
Fixed a bug causing the Main Menu to crash in some cases.