# Unity ColourLovers Importer

A simple editor tool to search and load colours and palettes directly from [COLOURlovers](http://www.colourlovers.com). 
Hooks into the COLOURlovers API documented [here](http://www.colourlovers.com/api).

There are options to edit and save the palettes returned. Saving a colour palette will create an asset in the Assets/Editor folder. Unfortunately Unity doesn't expose the ColorPresetLibrary so the preset assets are generated based on a text asset (ITS SO BAD BUT HEY IT WORKS). 

Hopefully this is a handy little tool for prototyping/creating placeholder assets or UI to give you easy access to nice colour combinations instead of using default/random colours.

Open importer under Window > ColourLovers Importer

Tested on Unity 5.6, 2017 and 2018 and seems to work correctly.

#### Link to download unity package available [here](http://shelleylowe.com/unity-colourlovers-importer/).

### Demo

![demo](http://shelleylowe.com/unity-colourlovers-importer/img/Example.gif)

![presets](http://shelleylowe.com/unity-colourlovers-importer/img/SavedPresets.png)

Options for loading the latest/top/random palettes and colours

![results](http://shelleylowe.com/unity-colourlovers-importer/img/Results.png)
