[![Roy Theunissen](Documentation~/Github%20Header.jpg)](http://roytheunissen.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](LICENSE.md)
![GitHub Follow](https://img.shields.io/github/followers/RoyTheunissen?label=RoyTheunissen&style=social)
<a href="https://roytheunissen.com" target="blank"><picture>
    <source media="(prefers-color-scheme: dark)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/globe_dark.png">
    <source media="(prefers-color-scheme: light)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/globe_light.png">
    <img alt="globe" src="globe_dark.png" width="20" height="20" />
</picture></a>
<a href="https://bsky.app/profile/roytheunissen.com" target="blank"><picture>
    <source media="(prefers-color-scheme: dark)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/bluesky_dark.png">
    <source media="(prefers-color-scheme: light)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/bluesky_light.png">
    <img alt="bluesky" src="bluesky_dark.png" width="20" height="20" />
</picture></a>
<a href="https://www.youtube.com/c/r_m_theunissen" target="blank"><picture>
    <source media="(prefers-color-scheme: dark)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/youtube_dark.png">
    <source media="(prefers-color-scheme: light)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/youtube_light.png">
    <img alt="youtube" src="youtube_dark.png" width="20" height="20" />
</picture></a> 
<a href="https://www.tiktok.com/@roy_theunissen" target="blank"><picture>
    <source media="(prefers-color-scheme: dark)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/tiktok_dark.png">
    <source media="(prefers-color-scheme: light)" srcset="https://github.com/RoyTheunissen/RoyTheunissen/raw/master/tiktok_light.png">
    <img alt="tiktok" src="tiktok_dark.png" width="20" height="20" />
</picture></a>

_Allows you to emit pre-defined controller vibrations/rumbles in a spatialized way, with attenuation over distance._

## About the Project

There's a lot of different libraries that help you send rumble values to a controller in an abstract, controller-agnostic way. That's all well and good, but how do you decide what these rumble values should be at any given moment?

I found myself making the same solution at different companies: a system where you define one-off or looping rumbles, play them back from a specific place or globally, and having a listener at a specific position, very much like Unity's audio system.

Then based on the distance between the rumble's origin and the rumble listener, an attenuation is applied. The further away the rumble is from the camera or the player (wherever your Listener is), the softer the rumble gets.


https://github.com/user-attachments/assets/b8ebbf51-7781-4c4b-9e8e-92db9f423a83


## Getting Started

- You need to have some kind of centralized `Rumble Service` in your project. There are several ways to go about this:
    - The easiest is placing a `RumbleServiceComponent` in your game somewhere. It registers itself as the active `Rumble Service` by default, but you can disable this so you have more control over execution order. Consider whether or not you want this to persist between scenes or not, by default it doesn't do this.
    - Alternatively, you could instantiate a `RumbleService` pure C# object yourself and register that as the main `Rumble Service` whenever you are initializing your other systems. You can look at `RumbleServiceComponent` for an example. It's basically just a `MonoBehaviour` wrapper for a `RumbleService` object.
    - If you want more control and don't mind re-implementing some functionality you could also implement the `IRumbleService` interface on a custom class of yours and register that whenever you are initializing your other systems. That would, for instance, allow the main Rumble Service instance to inherit from some other class, such as `Scriptable Object`.
- Add a `Rumble Listener` somewhere in your scene, similar to how an `Audio Listener` works. You could put it on your player character or on your camera, whatever makes the most sense for your game.
- Create a `Rumble Config` for a _One-Off_ or _Looping_ rumble (for example `Create/ScriptableObject/Spatial Rumble/Rumble Config (One-Off)`)
  
![image](https://github.com/user-attachments/assets/df85d82b-6d50-4b54-b966-29dbd617972d)
- There's two workflows for dispatching rumbles:
    - Have a serialized reference to a `Rumble Source` component. Similar to `Audio Source` components, you can assign a `Rumble Config` to them and tell them to `Play` or `Stop` via code. For an example, see the `Spatial Rumble Sample - Attenuation` scene in the include sample (accessible via the Package Manager).
    - Have a serialized reference to a `Rumble Config` and then call `Play` on that. For more information see the section _Dispatching rumbles from code_.

## Dispatching rumbles from code
### One-offs
For emitting a One-Off rumble you can call `Play` directly on a `RumbleOneOffConfig`, optionally specifying a transform if you want it to be spatialized. You then get back a `RumbleLoopingPlayback` instance that you can cache if you want to manipulate the playback, such as stopping it prematurely. Users of [FMOD-Syntax](https://github.com/RoyTheunissen/FMOD-Syntax) will be familiar with this approach.
```cs
[SerializeField] private RumbleOneOffConfig rumbleOneOffConfig;

private void Awake()
{
    // Play the specified rumble, spatialized as originating from our transform.
    rumbleOneOffConfig.Play(transform);
}
```
See the `Rumble Emitter One-Off` object in the `Spatial Rumble Sample - From Code` scene in the Sample included in the package (accessible via the Package Manager).

### Loops
For emitting a One-Off rumble you can call `Play` directly on a `RumbleLoopingConfig`, optionally specifying a transform if you want it to be spatialized. You then get back a `RumbleLoopingPlayback` instance that you should cache. You can then update its volume over time, and you can call `Stop` when you're done (or `Cleanup` if you are fully done with it, such as in `OnDestroy`). Users of [FMOD-Syntax](https://github.com/RoyTheunissen/FMOD-Syntax) will be familiar with this approach.
```cs
[SerializeField] private RumbleLoopingConfig rumbleLoopingConfig;

private RumbleLoopingPlayback rumbleLoopingPlayback;

private void OnEnable()
{
    // Start the looping playback and cache the instance so we can manipulate / stop it.
    rumbleLoopingPlayback = rumbleLoopingConfig.Play(transform);
}

private void OnDisable()
{
    // Always call Cleanup on a rumble playback when you're done with it.
    rumbleLoopingPlayback?.Cleanup();
}

private void Update()
{
    // Pulsate the 'opacity' of the effect back and forth over time.
    float scale = 0.5f + 0.5f * Mathf.Sin(Mathf.PI * Time.time * 2.0f / 2.0f);
    rumbleLoopingPlayback.OpacityMultiplier = scale;
}
```
See the `Rumble Emitter Continuous` object in the `Spatial Rumble Sample - From Code` scene in the Sample included in the package (accessible via the Package Manager).

## Wishlist
- Maybe add a setting to `RumbleServiceComponent` to make it call `DontDestroyOnLoad` on itself? Let me know if you want this feature. I personally want to manage this stuff myself, I never do this on a per-service basis.
- Maybe support multiple `Rumble Listener`s? One per player, for local multiplayer games? Let me know if you want this feature. I'm not working on any local multiplayer games so I've not bothered with this.
- Add native support for [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676?srsltid=AfmBOoqIs1fy2g3eiVEAKkPKVms4wBOrg4iM2VP4wKOEOn80atgmO5qi) for tweening the `OpacityMultiplier` of a `RumbleLoopingPlayback`? You can already control it with a `DOTween` but maybe it would be nice if that was built-in, and it automatically cleaned up the tween when you call `Cleanup` on the playback...
- Add native support for [Rewired](https://assetstore.unity.com/packages/tools/utilities/rewired-21676). The Unity Input System package is already optional, so you can use Spatial-Rumble to just determine the desired rumble values and then poll those every frame and send them to the hardware any way that you wish. Given how popular `Rewired` is it could be nice to support this natively though.

## Compatibility

### Unity Version

This system was developed for Unity 2022, it's recommended that you use it in Unity 2022 or upwards.

If you use an older or a newer version of Unity and are running into trouble, feel free to reach out and I'll see what I can do to help.

### Passing the rumble values to the hardware
The Unity Input System package is currently optional. If it's added to your project then `ENABLE_INPUT_SYSTEM` will be defined, and then it will automaticaly send the rumble values to the hardware via the Unity Input System. See `RumbleService.PassRumbleOnToHardware` to see how that works. You can also choose not to use the Unity Input System and use some other solution such as [Rewired](https://assetstore.unity.com/packages/tools/utilities/rewired-21676) instead. In that case you can use Spatial-Rumble to just determine the desired rumble values and then poll those every frame and send them to the hardware any way that you wish. Supporting Rewired natively is part of the [wishlist](#wishlist).

## Installation

### Package Manager

Go to `Edit > Project Settings > Package Manager`. Under 'Scoped Registries' make sure there is an OpenUPM entry.

If you don't have one: click the `+` button and enter the following values:

- Name: `OpenUPM` <br />
- URL: `https://package.openupm.com` <br />

Then under 'Scope(s)' press the `+` button and add `com.roytheunissen`.

It should look something like this: <br />
![image](https://user-images.githubusercontent.com/3997055/185363839-37b3bb3d-f70c-4dbd-b30d-cc8a93b592bb.png)

<br />
All of my packages will now be available to you in the Package Manager in the 'My Registries' section and can be installed from there.
<br />


### Git Submodule

You can check out this repository as a submodule into your project's Assets folder. This is recommended if you intend to contribute to the repository yourself.

### OpenUPM
The package is available on the [openupm registry](https://openupm.com). It's recommended to install it via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add com.roytheunissen.spatial-rumble
```

### Manifest
You can also install via git URL by adding this entry in your **manifest.json**

```
"com.roytheunissen.spatial-rumble": "https://github.com/RoyTheunissen/Spatial-Rumble.git"
```

### Unity Package Manager
```
from Window->Package Manager, click on the + sign and Add from git: https://github.com/RoyTheunissen/Spatial-Rumble.git
```


## Contact
[Roy Theunissen](https://roytheunissen.com)

[roy.theunissen@live.nl](mailto:roy.theunissen@live.nl)
