# 🎰 Slot Machine Game

> A fully playable 3-reel slot machine built in **Unity 2022.3.18f1**
> Submitted as part of the **Underpin Technology — Unity Developer Intern** assignment.

---

## 📋 Table of Contents

- [Game Overview](#-game-overview)
- [How to Run WebGL Build](#-how-to-run-webgl-build)
- [How to Play](#-how-to-play)
- [Symbols & Payouts](#-symbols--payouts)
- [Game Rules](#-game-rules)
- [Bonus Features](#-bonus-features)
- [Project Structure](#-project-structure)
- [Scripts Overview](#-scripts-overview)
- [Thought Process & Approach](#-thought-process--approach)
- [Built With](#-built-with)

---

## 🎮 Game Overview

A classic 3-reel slot machine game where the player pulls the handle to spin the reels. Match all 3 symbols across the reels to win a payout. The game tracks your balance, rewards big wins, and ends when you either run out of coins or hit the winner threshold — then automatically resets for another round.

---

## 🚀 How to Run WebGL Build

1. Go to the `/Build/WebGL` folder in this repository
2. Open `index.html` in **Google Chrome** (recommended)
3. If it doesn't load locally due to browser security, use a local server:
   - Install [Live Server](https://marketplace.visualstudio.com/items?itemName=ritwickdey.LiveServer) in VS Code
   - Right-click `index.html` → **Open with Live Server**

> ⚠️ Do NOT open `index.html` by double-clicking — it won't load due to CORS restrictions. Always use a local server.

---

## 🕹️ How to Play

1. The game starts with **100 coins**
2. Click the **SPIN button** or pull the **handle** to spin all 3 reels
3. Each spin costs **10 coins**
4. Wait for all 3 reels to stop
5. If all 3 reels show the **same symbol** — you win the payout!
6. Keep spinning to grow your balance
7. Reach **200+ coins** to be crowned **WINNER** 🏆
8. If your balance hits **0** — Game Over, the game resets automatically

---

## 🃏 Symbols & Payouts

| Symbol | Payout |
|:------:|:------:|
| 🍒 Cherry | 30 coins |
| 🔔 Bell   | 50 coins |
| ⭐ Star   | 80 coins |
| 7️⃣ Seven  | 150 coins |

> Payout is added to your balance on a winning spin.

---

## 📏 Game Rules

| Condition | Result |
|-----------|--------|
| All 3 reels match | Win — payout added to balance |
| Reels don't match | Lose — no payout, bet already deducted |
| Balance reaches **0** | **YOU LOSE** popup → game resets to 100 coins after 3 seconds |
| Balance reaches **200+** | **WINNER!** popup → game resets to 100 coins after 3 seconds |
| Spin during spinning | Ignored — button locked until reels stop |

---

## ✨ Bonus Features

| Feature | Description |
|---------|-------------|
| 🎰 **Handle Pull Animation** | The slot machine handle rotates 90° on Y axis when clicked, then returns — mimicking a real pull |
| 🌊 **Staggered Reel Start** | Each reel starts with a 0.2s delay after the previous, creating a cascading spin effect |
| 🔁 **Vertical Symbol Strip** | Each reel has a 4-image vertical strip that shuffles rapidly during spin, then lands cleanly |
| 💥 **Bounce on Stop** | Each reel plays a snap/bounce animation when it lands for satisfying tactile feedback |
| 🏆 **Win Popup Panel** | An animated scale-pop panel appears showing the payout amount on a winning spin |
| ⏱️ **Auto-hide Win Panel** | Win popup automatically disappears after 2.5 seconds |
| 💀 **Game Over Screen** | "YOU LOSE" shown in the popup when balance hits 0, game auto-resets |
| 🥇 **Winner Screen** | "WINNER!" shown in the popup when balance exceeds 200, game auto-resets |
| 🔒 **Spin Button Lock** | Button is disabled during spinning and end-game popups to prevent double input |
| 📡 **Event-Driven Architecture** | UIManager listens to GameManager via UnityEvents — zero direct coupling |

---

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs       — Spin flow, balance, win/lose, game-end logic
│   │   ├── Reel.cs              — Reel strip animation and symbol landing
│   │   ├── RNGManager.cs        — Centralized random number generation
│   │   └── SlotSymbol.cs        — Symbol data model (name, sprite, payout)
│   └── UI/
│       ├── UIManager.cs         — All UI updates, listens to GameManager events
│       ├── WinAnimator.cs       — Scale-pop animation on the WinPanel
│       ├── ReelSpinAnimator.cs  — Bounce effect when a reel stops
│       └── HandleAnimator.cs    — Handle pull/return animation on spin click
├── Sprites/
│   └── Slot Machine/            — All game sprites and UI assets
├── Prefabs/                     — Reusable GameObjects
├── Animations/                  — Animation clips
├── Sounds/                      — Audio clips (if applicable)
└── Scenes/
    └── SlotGame.unity           — Main game scene
```

---

## 📜 Scripts Overview

### `GameManager.cs` — Core
The brain of the game. Handles:
- Deducting bet on each spin
- Launching all 3 reels with staggered delays
- Checking win condition (all 3 symbols match)
- Awarding payouts and updating balance
- Detecting game-over (balance = 0) and winner (balance ≥ 200)
- Auto-resetting the game after 3 seconds on game-end
- Firing UnityEvents so UIManager stays decoupled

### `Reel.cs` — Core
Controls a single reel's vertical symbol strip:
- Rapidly shuffles all 4 symbol images during spin
- Uses RNGManager to pick the final landed symbol
- Fills the strip naturally around the result
- Triggers bounce animation via ReelSpinAnimator

### `RNGManager.cs` — Core
Single point of randomization. All `Random.Range` calls go through here for fairness and easy future seeding or testing.

### `SlotSymbol.cs` — Core
Simple serializable data class holding a symbol's name, sprite, and payout value. Assigned in the Inspector.

### `UIManager.cs` — UI
Listens to all GameManager events and updates the UI:
- Balance text, result text, bet text
- Win popup with payout amount (auto-hides after 2.5s)
- Game Over and Winner popups
- Spin button enable/disable state

### `WinAnimator.cs` — UI
Plays a scale-pop animation on the WinPanel every time it becomes active using `OnEnable`.

### `ReelSpinAnimator.cs` — UI
Adds a vertical bounce to a reel's RectTransform when it stops, giving a satisfying snap feel.

### `HandleAnimator.cs` — UI
Animates the slot machine handle on spin click:
- Rotates 90° on Y axis over 0.2s
- Holds for 0.6s
- Returns to original rotation over 0.2s

---

## 🧠 Thought Process & Approach

1. **Separation of Concerns** — GameManager owns all game logic. UIManager owns all display logic. Reel owns its own animation. No script reaches into another's responsibility.

2. **Event-Driven UI** — UnityEvents (`onWin`, `onLose`, `onGameOver`, `onWinner`, etc.) decouple the UI completely from game logic. The UI can be redesigned without touching a single line of game code.

3. **Centralized RNG** — All randomization goes through `RNGManager.GetRandomIndex()`. This makes it trivial to add seeding, weighted odds, or testing hooks in the future.

4. **Staggered Coroutines** — Each reel receives an increasing delay offset (`i * 0.2s`) so they cascade naturally, just like a real slot machine.

5. **Strip-based Reel Design** — Instead of swapping a single image, each reel has a vertical strip of 4 images that shuffle rapidly. This looks more authentic and fills the reel window naturally on landing.

6. **Game-End States** — Rather than just stopping the game, both game-over and winner states show a clear popup, lock input, then auto-reset — keeping the game loop smooth and replayable.

7. **Handle Animation** — A small but impactful detail. The Y-axis rotation gives the handle a realistic pull feel that makes the interaction more satisfying.

---

## 🛠️ Built With

- **Unity 2022.3.18f1**
- **C#** — All game logic and UI scripts
- **TextMeshPro** — High-quality UI text rendering
- **Unity UI** — Canvas, Image, Button, VerticalLayoutGroup
- **UnityEvents** — Decoupled event system between game and UI layers

---

<div align="center">

Made with ❤️ for the Underpin Technology Unity Developer Intern Assignment

</div>
