# OldPhonePad — C# solution

Solution for the Old Phone Keypad coding challenge (Iron Software).

## What this implements
- Converts a sequence of keypad presses to text based on classic mobile mapping:
  - 2→ABC, 3→DEF, 4→GHI, 5→JKL, 6→MNO, 7→PQRS, 8→TUV, 9→WXYZ, 0→space
- Special symbols:
  - ` ` (space) — separator: flushes the current group (does not insert visible space unless the key is `0`)
  - `*` — backspace: flush pending group, then delete last output char
  - `#` — end: flush pending group and stop processing
- Press wrapping: index = (presses - 1) % keyLength

## How to run
- Build with `dotnet build`
- Run tests: `dotnet test`
- Run CLI demo (optional):
  ```bash
  cd src/OldPhonePad
  dotnet run -- "8 88777444666*664#"
  ```
## Design choices / assumptions
- `#` stops processing and ignores subsequent characters.
- `*` flushes any pending digit-group then deletes the last produced character (if present).
- Space `' '` is a separator and does not insert a visible space unless the group was `0`.
- Unknown characters are treated as separators and cause a flush (documented choice to make behavior deterministic).
- `0` maps to a real space character in output.
- Null/empty input returns an empty string.
- Implementation is single-pass, O(n) time and O(n) memory for output.

## Project layout
- `src/OldPhonePad/` — main implementation and optional CLI
- `tests/OldPhonePad.Tests/` — xUnit tests covering regular and edge cases
- `.github/workflows/ci.yml` — CI to run tests on push/PR
- `AI_PROMPT.md` — the AI prompt used (if any)

## Notes on AI usage
I used AI tools only to help with formatting and wording of comments and README. The core problem-solving and code are my own work. The exact prompt used is recorded in `AI_PROMPT.md`.
