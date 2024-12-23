## ObsidianKnapper

This tool allows you to unpack narrative content from your favorite Obsidian Entertainment games.

In particular, OEIKnapper can read quests, conversations, global variables, and string tables. To read other assets, please use your unpacking tool of choice depending on the game engine.

Currently supported games include:
- Pentiment
- The Outer Worlds
- South Park: The Stick of Truth

Check the Wiki to see detailed game support.

## How to Use
1. Download the latest release of ObsidianKnapper
2. Depending on your chosen game's engine, use an asset unpacking tool to extract the game's assets. Look for the following file extensions:
    - `.conversationasset`
    - `.stringtablebundle`
    - `.questbundle`
    - `.globalvariablebundle`
    - `.oaf`
    - `.conversationbundle`
3. Run ObsidianKnapper and select the game's data folder. If you dumped the files to a different folder, locate that instead.
4. Start poking around!

## State of the Project
This is still a work in progress. Depending on the game, its engine, or the version of OEI it was developed with, the structure of these files may vary. The reference game for development is Pentiment, since its narrative files are in cleartext.

Writing changes back to the original game files is not intentionally supported but some users have had varying success. This is an area where help is needed.

Check out the wiki for documentation, feature timeline, and more.

## Licensing & redistribution

This is a fan-made project and is not affiliated with Obsidian Entertainment, Microsoft, or any other entity.

**You must source your own copies of the games to use this tool.** Please do not distribute this tool with content from the games. They're good games! Buy and play them yourself.

Also note the license of the project, which is GPL-3.0. 