# 🎰 Slot Machine Game — Unity Assignment

A fully playable slot machine game built in **Unity 2022.3.18f1** as part of the Underpin Technology Unity Developer Intern assignment.

---

## 🎮 Game Overview

A classic 3-reel slot machine where the player spins to match symbols. Win when all 3 reels show the same symbol. Each spin costs a bet amount deducted from the player's balance.

**Symbols & Payouts:**
| Symbol | Payout |
|--------|--------|
| Cherry | 30     |
| Bell   | 50     |
| Star   | 80     |
| Seven  | 150    |

---

## 🚀 How to Run the WebGL Build

1. Go to the `/Build/WebGL` folder in this repository
2. Open `index.html` in a modern browser (Chrome recommended)
3. If it doesn't load locally, use a local server:
   - Install [Live Server](https://marketplace.visualstudio.com/items?itemName=ritwickdey.LiveServer) in VS Code
   - Right-click `index.html` → **Open with Live Server**

---

## 🕹️ How to Play

- Press **SPIN** to spin all 3 reels
- Each spin costs **10 coins** from your balance
- Match all 3 symbols to win the payout
- Game starts with **100 coins**

---

## ✨ Bonus Features

- **Staggered Reel Start** — Reels start with a slight delay between each other for a realistic cascading feel
- **Bounce Animation** — Each reel plays a satisfying snap/bounce when it stops
- **Win Popup Panel** — A pop-scale animated panel appears on a winning spin
- **Spin Button Lock** — Spin button is disabled during spinning to prevent double-clicks
- **Event-Driven UI** — UIManager listens to GameManager events (UnityEvent) for clean decoupling

---

## 🧠 Thought Process & Approach

1. **Separation of Concerns** — GameManager handles logic, UIManager handles display, Reel handles animation. No cross-dependencies.
2. **RNGManager** — All randomization is routed through a single RNGManager for fairness and easy future seeding/testing.
3. **UnityEvents** — Used UnityEvent callbacks instead of direct references so UI can be swapped without touching game logic.
4. **Staggered Coroutines** — Each reel receives a delay offset so they start one after another, mimicking real slot machines.
5. **Bounce on Stop** — ReelSpinAnimator gives tactile feedback when a reel lands, improving game feel.

---

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── Core/          # GameManager, Reel, RNGManager, SlotSymbol
│   └── UI/            # UIManager, WinAnimator, ReelSpinAnimator
├── Prefabs/           # Reel prefab, WinPanel prefab
├── Animations/        # Animation clips (if any)
├── Sprites/           # All game sprites and UI assets
├── Sounds/            # Audio clips (if applicable)
└── Scenes/
    └── SlotGame.unity
```

---

## 🛠️ Built With

- Unity 2022.3.18f1
- TextMeshPro (UI text)
- Unity UI (Canvas, Image, Button)
