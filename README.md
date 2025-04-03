
# PokerMatic5000 – Poker Hand Evaluation Console App

**PokerMatic5000** is a C# console application created as part of a **technical interview test for Switch Studios**, a company specialising in gambling machine development. The goal was to implement a poker hand evaluator that accepts user input (a hand of five cards) and determines the hand ranking (e.g., Pair, Straight, Flush).

The code was written under time-limited conditions, and I intentionally submitted it **as-is**, without extending development past the allotted time. My aim was to reflect my **true ability under test conditions**. Despite not securing the position, I was told by the lead developer that the approach and structure of the solution was *exactly how they would have done it*.

---

## 🧠 Features Implemented

- 🃏 **Enums for Rank & Suit**  
  Custom `enum` types for clean poker logic, including dual-purpose `AceLow` and `AceHigh`.

- ⌨️ **Console Input Handler**  
  Accepts 5-card poker hands as comma-separated strings, e.g.:
  ```
  AH, KH, QH, JH, 10H
  ```

- 🔍 **Card Parser**  
  Robust parsing logic that:
  - Validates card format
  - Converts text input to `(Rank, Suit)` tuples
  - Rejects invalid or duplicate cards

- 📈 **Hand Evaluation Scaffolding**  
  A framework for detecting hand rankings such as:
  - Pair
  - Two Pair
  - Three of a Kind
  - Straight
  - Flush
  - Full House
  - Four of a Kind
  - Straight Flush

  (Full evaluation logic was not implemented within the time limit.)

- 🔄 **Looped Gameplay**  
  User can enter multiple hands or exit via `"EXIT"` keyword or ESC key.

---

## 📁 Project Structure

```
PM5000/
├── Program.cs          # Main logic, enums, input parsing, evaluation logic
├── App.config          # Default C# console config
├── PM5000.csproj       # Project definition
├── bin/                # Compiled output
└── obj/                # Build artifacts
```

---

## ⏱️ Time Constraints & Professional Integrity

This solution was created during a technical assessment with a **fixed time limit**. I chose not to continue development beyond the deadline, even though I was tempted to do so afterward.

This submission reflects:
- My ability to reason under pressure
- A clean and maintainable architecture
- Honest representation of my development process

Despite being unsuccessful in the application, I received positive feedback from the lead developer, who noted that my structure and decisions **aligned with their own approach**.

---

## 🧪 Future Plans (If Continued)

- Implement full hand ranking determination
- Add hand comparison for multiplayer logic
- Build a test suite for card evaluation
- Add GUI wrapper using WinForms or WPF

---

## 🙌 Final Thoughts

PokerMatic5000 is a great example of how I approach:
- **Clean system design**
- **Readable C# code**
- **Reasoned time management**
- **Honest professional practice**

It's incomplete by design — and yet, it's a solid foundation for further development and an honest reflection of my skillset under pressure.
