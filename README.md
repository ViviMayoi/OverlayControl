# About
OverlayControl is a WPF app designed to help control and automate stream overlays for fighting game tournaments, for use with OBS or similar software. Currently only for *Melty Blood Actress Again Current Code*.

## Current features
All features listed in this section are already implemented and fully functional, but the ease of use of each may vary before the [initial release](#10-initial-release). 

- Allows the user to control all information relative to the current match and event, **from a single interface**, for stream overlay purposes
    - For **each player**, this includes their:
        - **Tag**
        - **Sponsor tag / team name**
        - **Pronouns**
        - **Country flag**
        - **Character** (currently using predetermined images of a static size, used for *Snaildom Saturday Melty* streams)
        - **Grand Finals bracket status** ([L] tag)
    - **Tournament name**
    - **Current round**
    - **Commentators**
- **Automatically updates scores and selected characters** by hooking to the game
- **Creates a timestamp file** based on the start of the first match of the stream
- Allows the user to **synchronize timestamp files with VODs** by entering the start time of the first match

# Roadmap
Version labels and exact order of implementation aren't set in stone.

## 1.0 (initial release)
- Allow **custom images and image sizes** for characters
- Implement a **match queue** to automatically update the overlay
- Make flags a consistent size and add them all to the project proper
- General UI and code touch-ups
- Find a better name <img src=https://cdn.discordapp.com/emojis/851572925038985287.png width=19 height=18 alt="MBAACC sprite of Kohaku making a weird face">

## 1.1
- Support for **Fightcade games**
- **File browser implementation**, for custom overlay and timestamp file locations
- **Customizable grand finals markers**

## 1.2
- Support for **other PC games** (**EFZ**, others to be determined)
- **Dynamic** support for **Fightcade games** based on scoreboard files

## Future possibilities
- **Bracket management and display**
- Implementation of **team / sponsor logos**

