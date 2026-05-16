# Turn Forge Tracker

A Dungeons & Dragons Turn Forge Tracker desktop application for Windows. Track combat encounters with initiative, effects, rounds, and notes during gameplay.

## Features

### Core Functionality
- **Initiative System**: Add unlimited creatures/NPCs with initiative values and effect counts
- **Turn Tracking**: Visually highlight active creature, advance turns with "Next Turn" button
- **Round Counter**: Automatically increments when cycling through all creatures
- **Effects Management**: Apply effects with durations, effects decrement each round
- **Notes Panel**: Full-width text editor for DM notes
- **Dice Roller**: Support for d3, d4, d6, d8, d10, d12, d20, d100 with customizable modifiers
- **Save/Load**: Persist encounters to JSON format

## Build Instructions

### Prerequisites
- Visual Studio 2026 Community Edition
- Windows SDK

### Compilation
```
cd C:\workspace\myProjects\vsStudio\Turn_Forge_Tracker\src
cl /D_CRT_SECURE_NO_WARNINGS /D_WINDOWS /I..\include /W3 /EHsc /Fo..\build\ ^
  main.c models.c ui.c effects.c dice.c persistence.c ^
  /link user32.lib kernel32.lib /OUT:..\build\Turn_Forge_Tracker.exe
```

Or use the included Visual Studio project file (Turn_Forge_Tracker.vcxproj)

## Project Structure

```
Turn_Forge_Tracker/
├── src/
│   ├── main.c              - Application entry point
│   ├── models.c            - Core data structures and logic
│   ├── ui.c                - WinAPI UI implementation
│   ├── effects.c           - Effects database management
│   ├── dice.c              - Dice rolling mechanics
│   └── persistence.c       - JSON save/load functionality
├── include/
│   ├── models.h            - Creature, Effect, Encounter definitions
│   ├── ui.h                - UI function declarations
│   ├── effects.h           - Effects database interface
│   ├── dice.h              - Dice roller interface
│   └── persistence.h       - Save/load interface
├── resources/
│   └── effects.json        - Predefined D&D effects database
├── build/                  - Output directory for compiled executable
├── Turn_Forge_Tracker.vcxproj
├── Turn_Forge_Tracker.sln
└── README.md
```

## Data Structures

### Creature
- Name (string)
- Initiative (integer)
- Effects array
- Effect count

### Effect
- Name (string)
- Remaining duration (integer, in rounds)

### Encounter
- Creatures array
- Current turn index
- Round number
- Notes (text)

## User Interface Layout

```
+------------------+----------------------------------+
| Round N        [Next Turn]                   |
+------------------+----------------------------------+
| Initiative       | Effects    [Add] [Remove]   |
|---|---|---|------|---|---|---|---|---|---|---|
| Name  Init Eff   | Effect Name       Duration  |
| Avery  15   2    | Poisoned          2         |
| Goblin 12   1 <- | Bless             3         |
| Zephyra 7   0 <- |                             |
|---|---|---|------|---|---|---|---|---|---|---|
| [Sort] [Add]     |                             |
+------------------+----------------------------------+
| Notes                                        |
|------|------|------|------|------|------|---|
| Full-width editable text area for DM notes  |
|------|------|------|------|------|------|---|
+----------------------------------------------+
| Dice Roller                                  |
|------|------|------|------|------|------|---|
| Die: [d20 ▼] # Dice: [2] Modifier: [+1] [Roll]|
| [ 17 (+1) = 18 ]                            |
| [ 11 (+1) = 12 ]                            |
+----------------------------------------------+
```

## Usage

1. **Start the Application**: Run Turn_Forge_Tracker.exe
2. **Add Creatures**: Click "Add Creature" button to add NPCs/players
3. **Sort Initiative**: Click "Sort" to organize by initiative
4. **Track Turns**: Click "Next Turn" to advance to next creature
5. **Manage Effects**: Select creature, click "Add" to add effects, "Remove" to delete
6. **Roll Dice**: Select die type, number, modifier, then click "Roll"
7. **Notes**: Edit the notes area as needed
8. **Save**: Use File menu to save encounter state

## Keyboard Shortcuts

Currently supports keyboard input for text fields and buttons via standard Windows behavior.

## Effects Database

The application loads effects from `resources/effects.json` which includes:
- Conditions: Poisoned, Stunned, Blinded, Charmed, etc.
- Spells: Bless, Haste, Slow, etc.
- Exhaustion levels
- Custom effects can be added to the JSON file

## Technical Details

- **Language**: C (ANSI C standard compatible)
- **GUI Framework**: Windows API (WinAPI)
- **Data Persistence**: JSON (manual parsing, no external library)
- **Architecture**: Modular design with clear separation of concerns

## Limitations & Known Issues

- Add Creature button creates default creatures (future: dialog for custom name/initiative)
- Add Effect button adds first effect from database (future: dialog for effect selection)
- JSON parsing is manual (future: consider cJSON library for robustness)
- Keyboard shortcuts not yet implemented
- No dark mode support yet

## Future Enhancements

- [ ] Dialog windows for creature/effect creation
- [ ] File menu with Open/Save dialogs
- [ ] Edit creature dialog
- [ ] Effect selection dialog
- [ ] Color-coded rows by status
- [ ] Drag-and-drop reordering
- [ ] Keyboard shortcuts (spacebar for Next Turn, etc.)
- [ ] Dark mode
- [ ] Status bar with battle statistics

## License

Internal use only.

## Support

For issues or feature requests, please contact the development team.
