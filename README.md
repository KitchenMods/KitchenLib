# KitchenLib

# Major KitchenLib Announcement

Unfortunately we have noticed a problem in the way we're generating IDs for GameDataObjects.
At the moment we're using the NAME of the mod to generate IDs for GameDataObjects.
Although this poses a problem if a developer wants to change the name of their mod, all the IDs generated will also change, this causes any save files / old runs ( with the old IDs ) to completely break.

In saying this, we've decided we are going to have to change how IDs are generated.  This is going to pose one major problem. All modded saves / runs will no longer work correctly on the latest version of KitchenLib.

## This is your 3 week grace period

This save breaking change will occur in 3 weeks from today. in 21 days.
I will also be uploading a version of KitchenLib called KitchenLib Legacy which will keep the old ID generation method. ( This will only stay up temporarily for people who miss this grace period and want to finish their runs. )
( Once the Legacy version is uploaded, I will have a link to it here. )

## I have a modded run! What do I do?

You will have until the above posted time to complete your run before it is broken, or you will be able to use KitchenLib Legacy

## I have a modded Franchise! What do I do?

Your franchise may still work, although may also be buggy.
When your franchise is selected, you will notice that any modded cards that were previously in that franchise, are nolonger there.
You will have the ability to select a new main dish ( assuming it was originally modded ), and it will be treated as a vanilla franchise.

## What are GameDataObjects
GameDataObjects are what build the large majority of PlateUp! This includes things such as Appliances, Items, etc.

We apologies for any issues caused by this change.


## Description

KitchenLib is a ilbrary designed to assist in modding the Unity game PlateUp!

## [Documentation](https://github.com/KitchenMods/KitchenLib/wiki)

## Installation

https://steamcommunity.com/sharedfiles/filedetails/?id=2898069883

## BepInEx and MelonLoader support

As of v0.5.2, support for BepInEx and MelonLoader has been completely dropped.

## Contributors

`R4wizard#5960`

`StarFluxGames#7646`

`blarglebottoms#6086`

`Rolo#7645`

`Yariazen#8367`

`ZekNikZ#0878`


Semver is licensed under the MIT License, see [LICENSE.txt](https://github.com/maxhauser/semver/blob/master/License.txt) for more information.
