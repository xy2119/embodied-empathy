# Third-party assets

This repository does not redistribute third-party assets whose licences do not allow redistribution. Download them yourself and place them at the paths below before you open `ar-app/` in Unity. If an asset is missing, the project still opens and compiles, but the scenes affected will show missing references (for example, no body animations, or a fallback font).

| Asset | Where to get it | Place it at | Used for |
|---|---|---|---|
| **MetaPerson Loader** (AvatarSDK) | Unity Package Manager pulls it automatically from `https://github.com/avatarsdk/metaperson-loader-unity.git` (see `Packages/manifest.json`) | — | Loading MetaPerson GLB avatars |
| **MetaPerson developer credentials** | [AvatarSDK developer account](https://accounts.avatarsdk.com/developer/) | `ar-app/Assets/Resources/env.txt` (see `ar-app/env.example.txt`) | Generating the child avatar from a photo |
| **UniWebView** | Unity Asset Store (paid) | `ar-app/Assets/AvatarSelector/ChildhoodAvatarGenerate/UniWebView/` | In-app MetaPerson Creator web view |
| **Mixamo animations** | [mixamo.com](https://www.mixamo.com/), free with an Adobe account. Download as *FBX for Unity*, *Without Skin* | `ar-app/Assets/EmotionDisplay/Animations/_Mixamo/` | Emotion body animations |
| **TextMesh Pro Essentials** | Unity menu: *Window → TextMeshPro → Import TMP Essential Resources* | `ar-app/Assets/TextMesh Pro/` | UI text |
| **JazzCreate Bubble** font (optional) | The font author's distribution page | `ar-app/Assets/Fonts/` | Display font for UI headings |
| **Office Chair Pack 1.0** (optional) | Unity Asset Store | `ar-app/Assets/3d Objects/Office Chair Pack 1.0/` | Furniture in the virtual therapy room |

## Mixamo animations used

The animator controllers (`EmotionDisplay/Animations/*.controller`) reference these Mixamo clips. They were retargeted to the MetaPerson rig:

Angry (arms crossed), Angry Gesture, Angry Point, Annoyed Head Shake, Bashful, Being Cocky, Breathing Idle, Cheering, Crazy Gesture, Crying, Defeat, Defeated, Disappointed, Excited, Happy, Hard Head Nod, Idle Transition, Looking Around, Nervously Look Around, Rejected, Sad Idle, Shaking Head No, Silly Dancing, Sitting Disbelief, Sitting Idle, Standing Greeting, Terrified, Thankful, Waving, Yawn.

After you import the clips, open each animator controller and reassign any states that show *Missing (Motion)*.

## Replaced files

`ar-app/Assets/AvatarSelector/ChildhoodAvatarGenerate/Scripts/AccountCredentials.cs` was originally part of AvatarSDK's proprietary mobile integration sample. Here it is replaced by a minimal stand-in with the same class name, fields and Unity GUID, so scene references keep working.
