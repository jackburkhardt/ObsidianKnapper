## ObsidianKnapper

This tool allows you to unpack narrative content from your favorite Obsidian Entertainment games. Discover cut content, translate games to your language, or explore all of the niche possibilities.

In particular, OEIKnapper can read quests, conversations, global variables, and string tables. To read other assets, please use your unpacking tool of choice depending on the game engine.

Currently supported games include:
- Pentiment
- The Outer Worlds
- Pillars of Eternity II
- South Park: The Stick of Truth

Check the [Wiki page](https://github.com/jackburkhardt/ObsidianKnapper/wiki/Supported-Games) to see detailed game support.

## How to Use
1. Download the latest release of ObsidianKnapper
2. Depending on your chosen game's engine, use an asset unpacking tool to extract the game's assets into another folder in the game's install directory.

Pentiment and POEII are Unity games and they appear to store their narrative data in plain text, so this step may be unnecessary.
Games in other engines, especially Unreal, will likely need this step.

When you're unpacking assets, look for assets with the following file extensions:
- `.conversationasset`
- `.conversationbundle`
- `.stringtablebundle`
- `.questbundle`
- `.globalvariablebundle`
- `.oaf`
- `.conversationbundle`

5. Run ObsidianKnapper and select the game's data folder. If the game's install location isn't automatically detected, manually locate it.
6. Start poking around!

## State of the Project
This is still a work in progress. Depending on the game, its engine, or the version of OEI it was developed with, the structure of these files may vary. The reference game for development is Pentiment, since its narrative files are in cleartext.

Writing changes back to the original game files is not intentionally supported but some users have had varying success. This is an area where help is needed.

Check out the wiki for documentation, feature timeline, and more.

## Project Structure
- `OEIKnapper` - The main project, intended to be a 1:1 representation of the datatypes used in OEI. OEI is the tool used by Obsidian for creating dialogue.
- `OEIKnapperGUI` - A WPF application that runs a GUI for the project.
- `OEIKnapperTests` - Tests for various functions of OEIKnapper.

## Licensing & redistribution

This is a fan-made project and is not affiliated with Obsidian Entertainment, Microsoft, or any other entity.

**You must source your own copies of the games to use this tool.** Please do not distribute this tool with content from the games. They're good games! Buy and play them yourself.

Also note the license of the project, which is GPL-3.0. 
