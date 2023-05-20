# About
OverlayControl is a WPF app designed to help control and automate stream overlays for fighting game tournaments, for use with OBS or similar software. Currently only for *Melty Blood Actress Again Current Code*.

## Current features
- Lets the user control information relative to the current match or tournament, to be displayed on a stream overlay, all from the same interface:
    - For both players, their...
        - Tags
        - Sponsor tags
        - Pronouns 
        - Country flags
        - Characters (currently using predetermined images of a static size, used for *Snaildom Saturday Melty* streams)
    - Tournament name
    - Current round
- Automatically updates scores and selected characters by hooking to the game
- Creates a timestamp file based on the start of the first match of the stream

# Roadmap
Version labels and exact order of implementation aren't set in stone.

## 1.0 (initial release)
- Allow recalculating the timestamp file based on a user-input time, for use when publishing VODs
- Allow custom images and image sizes for characters
- General touch-ups
- Implement a match queue to automatically update the overlay
- Make flags a consistent size and add them all to the project proper
- Find a better name <img src=https://cdn.discordapp.com/emojis/851572925038985287.png width=19 height=18 alt="MBAACC sprite of Kohaku making a weird face">

## 1.1
- Support for Fightcade games
- File browser implementation, for custom overlay and timestamp file locations

## 1.2
- Support for other PC games (EFZ, others to be determined)
- Dynamic support for Fightcade games based on scoreboard files

## Future possibilities
- Bracket management and display
- Implementation of team / sponsor logos
