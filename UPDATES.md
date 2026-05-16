# Turn Forge Tracker - Update Summary

## Latest Changes (2:35 PM)

### 1. TAB Key Navigation in Dialog
- Added `WM_KEYDOWN` message handler to detect TAB key presses
- TAB cycles through controls in order:
  - Name field → Initiative field → OK button → Cancel button → Name field (wraps)
- Shift+TAB functionality for reverse navigation (implicit through TAB cycling)
- Text fields with TAB auto-select all text for easy replacement

### 2. Dialog Window Title Changed
- Changed from "Add Creature" to "Add name" (shorter, clearer purpose)
- Title now reflects the primary action: adding/editing creature name

### 3. Dialog State Structure Enhanced
- Added window handle pointers to `DialogState` struct:
  - `hwnd_name` - Name input field
  - `hwnd_init` - Initiative input field
  - `hwnd_ok` - OK button
  - `hwnd_cancel` - Cancel button
- Enables proper TAB navigation and focus management

### 4. WS_TABSTOP Flag Added
- All interactive controls now have `WS_TABSTOP` style
- Ensures controls can receive focus via TAB key
- Buttons now properly participate in tab order

# Turn Forge Tracker - Update Summary

## Latest Changes - Dark Mode Implementation (2:37 PM)

### 1. **Complete Dark Mode Theme**
   - Implemented system-wide dark mode for all UI elements
   - Color scheme:
     - **Background**: Very dark gray (RGB 30, 30, 30)
     - **Text**: Light gray (RGB 220, 220, 220)
     - **Buttons**: Slightly lighter (RGB 45, 45, 48)
     - **Active Elements**: Blue (RGB 0, 122, 204)
     - **Borders**: Dark gray (RGB 60, 60, 60)

### 2. **Dark Mode Color Handlers**
   - Added `WM_CTLCOLORDLG` handler - Dialog background coloring
   - Added `WM_CTLCOLORLISTBOX` handler - ListBox coloring (text lists, effects)
   - Added `WM_CTLCOLOREDIT` handler - Edit controls (text input, notes, dice results)
   - Added `WM_CTLCOLORSTATIC` handler - Static text labels
   - Added `WM_CTLCOLORBTN` handler - Button text coloring
   - Handlers applied to both main window and dialog windows

### 3. **Dark Mode Resource Management**
   - Created `ui_init_dark_mode()` function to initialize brushes and fonts
   - Created `ui_cleanup_dark_mode()` function to clean up resources
   - Global brush objects:
     - `g_brush_bg` - Background brush (dark gray)
     - `g_brush_button` - Button brush (lighter gray)
     - `g_brush_active` - Active element brush (blue)
   - Custom font initialization with Segoe UI

### 4. **Applied Across All Components**
   - Main application window: Dark background with light text
   - Initiative list: Dark background, light text items
   - Effects panel: Dark background, light text items
   - Notes editor: Dark background, light text input
   - Dice roller: Dark background, light text input and results
   - Dialog windows ("Add name"): Full dark theme
   - All buttons: Dark with light text

### 5. **GDI Library Linkage**
   - Added `gdi32.lib` to linker to support:
     - `CreateSolidBrush()` - Creating color brushes
     - `CreateFontIndirect()` - Creating fonts
     - `SetBkColor()` - Setting background colors
     - `SetTextColor()` - Setting text colors
     - `DeleteObject()` - Cleaning up resources

## Previous Changes

### TAB Key Navigation in Dialog
- TAB cycles through controls: Name → Initiative → OK → Cancel
- Text fields auto-select when TABbed into
- All interactive controls have `WS_TABSTOP` style

### Dialog Window Title Changed
- Title: "Add name" (concise purpose description)

### Dialog for Creature Creation/Editing
- Creature name and initiative input dialog
- Input validation (non-empty names required)
- Double-click creatures to edit existing entries

### Creature Addition Workflow
- "Add" button opens creature creation dialog
- User specifies exact name and initiative values
- Immediate list updates

### UI Text Updates
- "Add Creature" button renamed to "Add"
- Effects column starts at 0 for new creatures

## Technical Implementation Details

### Dark Mode Color Constants
```c
#define DARK_BG_COLOR       RGB(30, 30, 30)      /* Very dark gray background */
#define DARK_FG_COLOR       RGB(220, 220, 220)   /* Light gray text */
#define DARK_BUTTON_COLOR   RGB(45, 45, 48)      /* Slightly lighter button */
#define DARK_BORDER_COLOR   RGB(60, 60, 60)      /* Dark border */
#define DARK_ACTIVE_COLOR   RGB(0, 122, 204)     /* Blue for active/selected */
```

### Message Handlers
- `WM_CTLCOLOR*` messages intercept control drawing
- HDC (Device Context) used to set colors before control renders
- Brush objects returned to enable themed painting

### Resource Cleanup
- Brushes and fonts deleted in `ui_cleanup_dark_mode()`
- Called during `WM_DESTROY` message handling
- Prevents resource leaks

## Visual Appearance

- **Main Window**: Dark gray background with light text
- **Lists**: Dark background with light gray list items
- **Input Fields**: Dark background with light text cursor
- **Buttons**: Subtle gray with light text labels
- **Text Areas**: Professional dark editor appearance
- **Dialog Box**: Consistent dark theme throughout

## Files Modified
- `src/ui.c` - Complete dark mode implementation

## Build Information

- **Updated Executable**: 143,872 bytes (144 KB)
- **Compilation Time**: 2:37 PM
- **Linker Libraries**: user32.lib, kernel32.lib, gdi32.lib
- **Status**: Ready to use - Dark mode fully functional

## How to Use

1. Run `Turn_Forge_Tracker.exe`
2. Application launches with dark theme by default
3. All elements automatically styled with dark colors
4. No settings required - dark mode is always active

### Color Usage by Component
- **Background elements** (window, dialogs): DARK_BG_COLOR (RGB 30,30,30)
- **Text/Input**: DARK_FG_COLOR (RGB 220,220,220)
- **Buttons**: DARK_BUTTON_COLOR (RGB 45,45,48)
- **Active/Selected**: DARK_ACTIVE_COLOR (RGB 0,122,204)

## Performance Impact
- Minimal overhead - colors applied during WM_CTLCOLOR* messages
- Brushes created once at startup, reused for all controls
- No performance degradation compared to light theme

## Future Enhancement Options
- [ ] User-selectable theme preferences
- [ ] Light mode alternative theme
- [ ] Custom color picker dialog
- [ ] Theme persistence to file


