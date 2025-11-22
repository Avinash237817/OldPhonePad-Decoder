# 📱 OldPhonePad — Production-Ready C# Decoder

A robust, extensible, and fully validated implementation of the classic old-mobile keypad text decoder.  
This project is engineered to meet the production standards and expectations outlined by Iron Software.

---

## 🚀 Project Goals

This project focuses on delivering:

### ✅ Production-ready code
Clean, safe, modular, readable, maintainable.

### ✅ Clear, well-organized structure
Core logic in `src/`, all test logic in `tests/`.

### ✅ Stability & robustness
Handles all real-world and edge case inputs:
- noise characters  
- repeated digits  
- very long sequences  
- backspaces  
- early termination  
- multiple separators  
- invalid patterns  
- runtime keymaps  

### ✅ Professional engineering standards
- Proper separation of concerns  
- Pure core logic (no I/O)  
- API wrapper for extensibility  
- Full test harness  
- Fuzz validation  
- CSV batch processing  
- Deterministic behavior  

---

# 📂 Project Structure

OldPhonePad_Submission/
│
├── src/
│ └── OldPhonePad/
│ ├── OldPhonePad.csproj
│ ├── OldPhonePadDecoder.cs ← Core decoding logic
│ ├── OldPhonePadApi.cs ← Public interface layer
│ └── Program.cs ← CLI runner (single & batch modes)
│
├── tests/
│ ├── TestHarness.csproj
│ ├── Generator.cs ← Random test input generator
│ ├── Validator.cs ← Expected vs actual comparison
│ ├── Program.cs ← Test harness CLI (generate/validate)
│ ├── testcases.csv ← Auto-generated input dataset
│ └── results.csv ← Validation output
│
├── run_all.ps1 ← One-click generate+validate pipeline
├── AI_PROMPT.md
├── README.md
└── OldPhonePad.sln


---

# 🔧 Decoder Rules (Functionality Overview)

The OldPhonePad decoder converts numeric button presses into characters using classic keypad mapping:

| Key | Letters |
|-----|---------|
| 2   | ABC     |
| 3   | DEF     |
| 4   | GHI     |
| 5   | JKL     |
| 6   | MNO     |
| 7   | PQRS    |
| 8   | TUV     |
| 9   | WXYZ    |
| 0   | Space   |

### Special characters  
| Input | Meaning |
|-------|---------|
| `space` | Flush group (separator) |
| `*` | Backspace (delete previous output char) |
| `#` | End input immediately |

### Behavior details
- Pressing a digit multiple times cycles letters using modulo wrap  
  - e.g., `77777` → `'P'`  
- Unknown characters are treated as separators  
- Input is processed left-to-right in a deterministic single pass  
- `OldPhonePadDecoder` is pure: no I/O, no static state  

---

# ▶ How to Run

## 1️⃣ **Decode a single input**


dotnet run --project src/OldPhonePad/OldPhonePad.csproj -- "4433555 555666#"


Output:


HELLO


---

## 2️⃣ **Decode many inputs via CSV (Batch Mode)**



dotnet run --project src/OldPhonePad/OldPhonePad.csproj -- --file testcases.csv --out results.csv


CSV format:


input,expected
"33#","E"
"4433555 555666#","HELLO"


---

# 🧪 Testing & Validation

The project includes a complete test system.

## Generate random test inputs


dotnet run --project tests/TestHarness.csproj -- --gen 5000


Generates:


tests/testcases.csv


## Validate decoder against expected outputs


dotnet run --project tests/TestHarness.csproj -- --validate


Outputs:


tests/results.csv
tests/validation_log.txt


Each row contains:


input,expected,actual,match


---

# 🔄 Full Pipeline (One Command)

Run everything (build → generate → validate → results):



.\run_all.ps1 1000


Or use alternative uploaded decoder:



.\run_all.ps1 1000 -UseUploadedDecoder


---

# 🔌 Runtime Custom Keymaps

Provide your own keypad mapping at runtime:



--map " , ,ABC,DEF,GHI,JKL,MNO,PQRS,TUV,WXYZ"


This makes the decoder fully dynamic and future-proof.

---

# 🔍 Error Handling

The decoder gracefully handles:

- Empty input  
- Unknown characters  
- Multiple `*`  
- Multiple `#`  
- Leading/trailing spaces  
- Massive inputs  
- Empty groups  
- Custom keymaps missing characters  

No unexpected exceptions or side-effects occur.

---

# 🤖 AI Prompt Transparency

The prompt used for AI assistance is stored in:


AI_PROMPT.md


This clearly documents what AI was used for (formatting/cleanup)  
and what was written by the developer (core logic, design choices, testing).

---

# 🏁 Final Notes

This project provides:

- Clean, modular production-ready code  
- A fully independent test validation system  
- Support for runtime mapping  
- CLI batch processing  
- Noise resistance  
- Deterministic output  
- Automated large-scale fuzzing  

If you'd like enhancements (CI pipeline, NuGet packaging, benchmarking suite, GUI runner), just ask!
