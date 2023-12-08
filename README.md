# LC-Boombox
Allows players to add to or replace boombox tracks in Lethal Company.

## Installing Songs when Installed via r2modman

When mods are installed with r2modman, BepInEx gets configured to place mod files in a different location

You can find the correct folder by going to `Settings`, clicking `Browse profile folder`, then navigating to `BepInEx\Custom Songs\Boombox Music`.

![image](https://github.com/DeadlyKitten/LC-Boombox/assets/9684760/ef378cdc-c2af-4ba4-82ef-d2aa29a9af31)

## Manual Installation
Place the [latest release](https://github.com/DeadlyKitten/LC-Boombox/releases/latest) into the `BepInEx/plugins` folder. Run the game once to generate content folders.

Place boombox tracks into `BepInEx/Custom Songs/Boombox Music`.

-----
 
### Valid file types are as follows:
- WAV
- OGG
- MP3


## 🔧 Developing

Clone the project, then create a file in the root of the project directory named:

`CustomBoomboxTracks.csproj.user`

Here you need to set the `GameDir` property to match your install directory.

Example:
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <PropertyGroup>
    <!-- Set "YOUR OWN" game folder here to resolve most of the dependency paths! -->
    <GameDir>C:\Program Files (x86)\Steam\steamapps\common\Lethal Company</GameDir>
  </PropertyGroup>
</Project>
```

Now when you build the mod, it should resolve your references automatically, and the build event will copy the plugin into your `BepInEx\plugins` folder!
