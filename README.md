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

_Allows you to emit controller vibration/rumble in a spatialized way._

## About the Project

There's a lot of different libraries that help you send rumble values to a controller in an abstract, controller-agnostic way. That's all well and good, but how do you decide what these rumble values should be at any given moment?

I found myself making the same solution at different companies: a system where you define one-off or looping rumbles, play them back from a specific place or globally, and having a listener at a specific position, very much like Unity's audio system.

Then based on the distance between the rumble's origin and the rumble listener, an attenuation is applied. The further away the rumble is from the camera or the player (wherever your Listener is), the softer the rumble gets.


https://github.com/user-attachments/assets/b8ebbf51-7781-4c4b-9e8e-92db9f423a83


## Getting Started

🛑 <b><u>TO DO</u></b>

- You need to have some kind of centralized Rumble Service in your project. There are several ways to go about this:
    - The easiest is placing a `RumbleServiceComponent` in your game somewhere. It registers itself as the active Rumble Service by default, but you can disable this so you have more control over execution order.
    - Alternatively, you could instantiate a `RumbleService` pure C# object yourself and register that as the main Rumble Service whenever you are initializing your other systems. You can look at RumbleServiceComponent for an example. It's basically just a `MonoBehaviour` wrapper for a `RumbleService` object.
    - If you want more control and don't mind re-implementing some functionality you could also implement the `IRumbleService` interface on a custom class of yours and register that whenever you are initializing your other systems. That would, for instance, allow the main Rumble Service instance to inherit from some other class, such as `Scriptable Object`.
- Create a Rumble Config for a One-Off or Looping rumble (for example `Create/ScriptableObject/Spatial Rumble/Rumble Config (One-Off)`)
![image](https://github.com/user-attachments/assets/df85d82b-6d50-4b54-b966-29dbd617972d)

## Compatibility

🛑 <b><u>TO DO</u></b>

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
