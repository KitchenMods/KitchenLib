# KitchenLib

## Description

KitchenLib is a ilbrary designed to assist in modding the Unity game PlateUp!

## [Documentation](https://github.com/KitchenMods/KitchenLib/wiki)

## Installation

https://steamcommunity.com/sharedfiles/filedetails/?id=2898069883

## BepInEx and MelonLoader support

As of v0.5.2, support for BepInEx and MelonLoader has been completely dropped.

## Signing, Provenance and Verification

Releases as of [v0.7.9b3](https://github.com/KitchenMods/KitchenLib/releases/tag/v0.7.9b3) have a full continuous integration workflow that builds KitchenLib from source and signs the resulting artifact using 
[Cosign](https://docs.sigstore.dev/cosign/overview/). The workflow can be inspected by anyone and is located [here](https://github.com/KitchenMods/KitchenLib/blob/master/.github/workflows/build-and-sign.yml). This workflow should allow an outside party to cryptographically validate that KitchenLib was in fact built from the source code located in this repository. 

**What the signing process does**
The only thing the signing process does, is cryptographically prove that KitchenLib has not been tampered with in between a specific source code reference and your steam workshop install of the mod. This process removes the need to trust any third parties between the signed artifact and delivery to your computer. This of course has a baseline trust of a few entities

- Github.com - immutable git history (no forging of commits) and their hosted runners are not compromising workflows as they run
- Sigstore.dev - This system is responsible for exchanging keys from Github.com OIDC and giving back a certificate for signing. We trust that they are not giving certs to just anyone (similar to a CA) See [Threat Model](https://docs.sigstore.dev/threat-model/) for more details.

That being said, the whole idea behind this process is that you must have a baseline trust somewhere in order to establish an overall chain of custody. At the end of the day, we hope this places your trust in those systems and won't require trust of any specific humans delivering the artifacts to you.

The main thing this process provides is it give you the ability to perform self verification to validate the safety of the artifact (See Verification below)

**What the signing process DOES NOT DO**

Its important to understand what the signing process does and does not do for you. None of this process validates or verifies the safety of the code that is compiled into the resulting artifact. You *must* perform the verification steps below including code verification to assert the safety of the code. The only guarantee you have is that you will be able to find the code you need in order to perform that verification.

### Verification

The high-level overview of verifying artifacts via Cosign is documented [here](https://docs.sigstore.dev/cosign/verify/). In order to validate KitchenLib more specifically, you'll need to perform a few steps:

- Download the latest version of Cosign from the releases page for your platform of choice: https://github.com/sigstore/cosign/releases/latest (most probably want the `cosign-windows-amd64.exe`).
- Locate the KitchenLib artifacts installed by the Steam workshop from your subscription. Most platforms will be `<SteamLibrary>/steamapps/workshop/content/1599600/2999830291`. You'll need to find two files:
`KitchenLib-Workshop.dll` and `cosign.bundle` (these are the same files you can find on the Releases section of this repo in the zip file)
- Once located, you'll want to invoke `cosign` command with these files as follows: `cosign verify-blob --bundle cosign.bundle ./KitchenLib-Workshop.dll --certificate-oidc-issuer "https://token.actions.githubusercontent.com" --certificate-identity "https://github.com/KitchenMods/KitchenLib/.github/workflows/build-and-sign.yml@refs/heads/master"` (note that `refs/heads/development` should be used if verifying beta versions of KitchenLib. Also note that the above command would be slightly different on Windows platforms: `cosign.exe` instead of just `cosign)
- If that command prints out `Verified OK`, then you have confirmed that the Mod has not been tampered with. If you do not get that result, either you may have run the commands incorrectly, or the library has in fact been modified since signing.

If you just want to validate the artifact hasn't been tampered with, you can stop here, but in order to provide further verification on the code, a few more steps need to happen:

- Locate the `cosign.bundle` file and locate the `cert` field within the json payload. Save the contents of the field to a separate file. This is a base64 encoded public certificate with important relevant information.
- If you are on a Unix like platform (OSX or Linux), you can save it to a file and perform the following: `cat <encoded_file> | base64 -d > openssl -in -text -noout`. This will print a bunch of information. The important info is highlighted in the below image:
  ![Cert Information](https://github.com/KitchenMods/KitchenLib/assets/11451714/e180d6de-912c-43b1-a431-5dd273a6f0b2)


  If you are a Windows platform and have `git` installed via [gitforwindows](https://gitforwindows.org/) you likely already have `openssl.exe` available at `C:\Program Files\Git\usr\bin\openssl.exe` and can use that. You'll have to base64 decode the certificate using an additional tool

- You will want to take note of the relevant information so you can find the source code for the artifact. In the example above, you can take the commit SHA to find the exact code that it was generated from: https://github.com/KitchenMods/KitchenLib/tree/fefd811c4da632c2d599a8f5a81253742b606e96
- Once you make it this far, you'll want to verify the code that built the library and signed it. Go inspect the specific workflow file referenced above and assure that nothing malicious is present that could attempt to subvert the building or signing process and the code it references. You will want to verify that the workflow does in fact build from the code referenced in the repository and doesn't pull it from an outside source.
- If the previous step checks out, you can now proceed to verify the actual code content and be relatively certain what you see is what you get in the library artifact.


I highly encourage other experts in the field to validate this flow and verify any security flaws in the design or provide recommendations for tightening things up even more.  



## Contributors

`R4wizard#5960`

`StarFluxGames#7646`

`blarglebottoms#6086`

`Rolo#7645`

`Yariazen#8367`

`ZekNikZ#0878`


Semver is licensed under the MIT License, see [LICENSE.txt](https://github.com/maxhauser/semver/blob/master/License.txt) for more information.
