# Unity ColourLovers Importer

A simple editor tool to search and load colours and palettes directly from colourlovers.com. 
Hooks into the [ColourLovers](www.colourlovers.com) API documented [here](www.colourlovers.com/api).

There are options to edit and save the palettes returned. Unfortunately Unity doesn't expose the ColorPresetLibrary so the preset assets are generated based on a text asset (ITS SO BAD BUT HEY IT WORKS).

Hopefully this is a handy little tool for prototyping/creating placeholder UI to give you easy access to nice colour combinations instead of using default/random colours.

### Demo

![demo](http://shelleylowe.com/unity-colourlovers-importer/Example.gif)


Options for loading the latest/top/random palettes and colours

![palettes](http://shelleylowe.com/unity-colourlovers-importer/Palettes.png) ![colours](http://shelleylowe.com/unity-colourlovers-importer/Colours.png)


Saving a colour palette will create a ColorPresetLibrary asset in the Assets/Editor folder

![presets](http://shelleylowe.com/unity-colourlovers-importer/SavedPresets.png)
