# Youtube Boombox

[![GitHub Build Status](https://img.shields.io/github/actions/workflow/status/archiew1/youtubeboombox/build.yml?style=for-the-badge&logo=github)](https://github.com/archiew1/youtubeboombox/actions/workflows/build.yml)
[![Thunderstore Version](https://img.shields.io/thunderstore/v/Archie/YoutubeBoomBox?style=for-the-badge&logo=thunderstore&logoColor=white)](https://thunderstore.io/c/lethal-company/p/Archie/YoutubeBoomBox/)
[![Thunderstore Downloads](https://img.shields.io/thunderstore/dt/Archie/YoutubeBoomBox?style=for-the-badge&logo=thunderstore&logoColor=white)](https://thunderstore.io/c/lethal-company/p/Archie/YoutubeBoomBox/)

A simple mod that allows players to load audio from youtube audio to the in game boombox.

## Features

- /add `search query` (Loads first result of the query. Note if size of the video is large loading will take significant time)

- /clear (Clears all audio tracks)

- If there are no custom tracks default tracks resume
- Audio tracks are no longer random instead they operate in a circular queue

## Installation

Install like any other BepInEx mod. Install to the following directory:

```
  \GAME_LOCATION\Lethal Company\BepInEx\plugins
```

## Change Log

- 1.0.0 Released
- 1.0.1 Fixed folder issue causing an error
- 1.0.2 Fixed 403 error caused by maxing daily queries