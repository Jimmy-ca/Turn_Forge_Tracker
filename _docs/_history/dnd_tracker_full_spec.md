# D&D Initiative / Effect Tracker – Full Functional Specification

## Goal
Create a simple Windows desktop (offline-only) application that allows a Dungeon Master to manage initiative order, track turns and rounds, monitor effects, roll dice, take notes, and save/load encounters.

---

## 1. Initiative System

### Requirements
- User can add an unlimited number of entries (PCs, NPCs, creatures)
- Each entry contains:
  - Name (string)
  - Initiative (integer)

### Behavior
- Entries are editable at any time
- New creatures can be added mid-combat

---

## 2. Sorting

### Requirements
- Sort all entries from highest to lowest initiative
- Sorting can be triggered at any time via button

---

## 3. Turn Management

### Requirements
- One entry is always marked as the "active turn"
- Active entry is visually highlighted

### Controls
- "Next Turn" button:
  - Advances to next entry
  - Wraps to first entry after last

---

## 4. Round Tracker

### Requirements
- Display current round prominently (e.g., "Round 1")

### Behavior
- Starts at Round 1
- When turn cycles from last entry back to first:
  - Round increments by +1

---

## 5. Effects System

### Adding Effects
- Clicking a creature opens an effect editor popup
- User can input:
  - Effect Name (text)
  - Duration (number of rounds)

### Display
- Fixed panel on the right side
- Shows selected creature's active effects:
  - Effect Name
  - Remaining Duration

### Behavior
- Effects are reduced by 1 at the start of each NEW round
- Effects are removed automatically when duration reaches 0

---

## 6. Notes Panel

### Requirements
- Editable text box below initiative tracker
- Used for freeform DM notes
- Notes are saved with encounter data

---

## 7. Dice Roller

### Supported Dice
- d3, d4, d6, d8, d10, d12, d20, d100

### Inputs
- Number of dice
- Modifier per die

### Output
- Individual rolls (optional display)
- Modified results
- Total sum

### Example
Rolling 4d20 with +2 modifier
Rolls: 12, 5, 18, 9
Modified: 14, 7, 20, 11
Total: 52

---

## 8. Save / Load Encounter System

### Requirements
- Save encounter to file
- Load previously saved encounters
- Support multiple saved encounters

### Saved Data Includes
- Initiative list
- Current turn index
- Round number
- Effects for each creature
- Notes

---

## UI Layout (Concept)

-----------------------------------------
| Initiative List | Effects Panel        |
|-----------------|---------------------|
| Name | Init     | Effect Name (Turns) |
| Goblin 18       | Poisoned (2)        |
| Fighter 15      | Bless (3)           |
| Wizard 12       |                     |
|                 |                     |
| [Next Turn]     |                     |
| [Sort]          |                     |
-----------------------------------------
| Notes Panel                            |
-----------------------------------------
| Dice Roller                            |
-----------------------------------------

---

## Data Model (Concept)

Creature:
- Name
- Initiative
- Effects[]

Effect:
- Name
- RemainingRounds

---

## Core Logic

Advance Turn:
IF current == last:
  current = 0
  round += 1
  decrement all effects
ELSE:
  current += 1

---

## Optional Enhancements
- Color-coded rows based on state
- Drag-and-drop reordering
- Auto-save functionality
- Dark mode
- Keyboard shortcuts (e.g., spacebar = next turn)

---

## Summary
This application provides a lightweight, offline tool for Dungeon Masters to efficiently manage combat encounters with minimal overhead and maximum clarity.
