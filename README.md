# Slot Machine Game

A fully playable 3-reel slot machine built in Unity 2022.3.18f1, submitted as part of the Underpin Technology Unity Developer Intern assignment.

---

## Table of Contents

- [Game Overview](#game-overview)
- [How to Run WebGL Build](#how-to-run-webgl-build)
- [How to Play](#how-to-play)
- [Symbols and Payouts](#symbols-and-payouts)
- [Game Rules](#game-rules)
- [Bonus Features](#bonus-features)
- [Project Structure](#project-structure)
- [Scripts Overview](#scripts-overview)
- [Thought Process and Approach](#thought-process-and-approach)
- [Built With](#built-with)

---

## Game Overview

This is a classic 3-reel slot machine game where the player pulls the handle to spin the reels. The goal is simple — match all 3 symbols on the reels to win coins. The game keeps track of your balance, rewards you on wins, and ends the round when you either run out of coins or cross the winner threshold. After either outcome, the game resets automatically so you can play again.

---

## How to Run WebGL Build

1. Open the `/Build/WebGL` folder from this repository
2. Run `index.html` in Google Chrome
3. If the file does not load directly, use a local server to avoid browser security issues:
   - Install the Live Server extension in VS Code
   - Right-click `index.html` and select Open with Live Server

Note: Do not open `index.html` by double-clicking it. It will not load correctly due to CORS restrictions. Always use a local server.

---

## How to Play

1. The game starts with 100 coins in your balance
2. Click the Spin button or pull the handle on the right side of the machine
3. Each spin deducts 10 coins from your balance
4. Wait for all 3 reels to stop spinning
5. If all 3 reels land on the same symbol, you win the payout for that symbol
6. Keep spinning to build up your balance
7. Cross 200 coins and you win the round
8. If your balance drops to 0, the game shows a loss screen and resets

---

## Symbols and Payouts

| Symbol | Payout |
|--------|--------|
| Cherry | 30 coins |
| Bell | 50 coins |
| Star | 80 coins |
| Seven | 150 coins |

The payout is added directly to your balance when all 3 reels match.

---

## Game Rules

| Situation | What Happens |
|-----------|--------------|
| All 3 reels show the same symbol | You win and the payout is added to your balance |
| Reels do not match | No payout, the bet is already deducted |
| Balance reaches 0 | A YOU LOSE popup appears and the game resets to 100 coins after 3 seconds |
| Balance crosses 200 | A WINNER popup appears and the game resets to 100 coins after 3 seconds |
| Clicking spin while reels are moving | Nothing happens, the button is locked until all reels stop |

---

## Bonus Features

**Handle Pull Animation**
The handle on the side of the machine rotates 90 degrees on the Y axis when you click spin, holds for a moment, then returns to its original position. It gives the feel of actually pulling a real slot machine handle.

**Staggered Reel Start**
Each reel starts spinning with a small delay after the previous one. This creates a cascading effect that looks and feels much more natural than all three starting at the same time.

**Vertical Symbol Strip**
Each reel is built as a vertical strip of 4 symbol images. During a spin, all 4 images shuffle rapidly. When the reel stops, the strip settles into a clean final position showing the landed symbol in context with the others around it.

**Bounce on Stop**
When a reel lands, it plays a small vertical bounce animation. It is a subtle effect but it makes the stop feel satisfying and physical rather than just abrupt.

**Win Popup Panel**
A popup panel appears with a scale animation whenever you win a spin. It shows the payout amount and disappears automatically after 2.5 seconds.

**Game Over and Winner Screens**
The same popup panel is reused for end-game states. It shows YOU LOSE when balance hits 0 and WINNER when balance crosses 200. In both cases the spin button is locked and the game resets after 3 seconds.

**Spin Button Lock**
The spin button is disabled the moment a spin starts and only re-enables once all reels have fully stopped. This prevents any accidental double spins.

**Event-Driven UI**
The UIManager does not directly talk to the GameManager. Instead it listens to UnityEvents fired by the GameManager. This means the entire UI layer can be changed or replaced without touching any game logic.

---

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs
│   │   ├── Reel.cs
│   │   ├── RNGManager.cs
│   │   └── SlotSymbol.cs
│   └── UI/
│       ├── UIManager.cs
│       ├── WinAnimator.cs
│       ├── ReelSpinAnimator.cs
│       └── HandleAnimator.cs
├── Sprites/
│   └── Slot Machine/
├── Prefabs/
├── Animations/
├── Sounds/
└── Scenes/
    └── SlotGame.unity
```

---

## Scripts Overview

**GameManager.cs**
This is the main controller of the game. It handles deducting the bet, launching all 3 reels with staggered delays, checking whether the spin was a win or a loss, updating the balance, detecting the game-over and winner conditions, and firing events to notify the UI.

**Reel.cs**
Controls one reel. During a spin it rapidly randomizes the 4 symbol images in the strip to simulate movement. When the spin ends it uses RNGManager to pick the final symbol and settles the strip around it. It also triggers the bounce animation on landing.

**RNGManager.cs**
A single centralized class that handles all random number generation. Every random call in the game goes through here. This makes it easy to add weighted odds or a seed system later without changing anything else.

**SlotSymbol.cs**
A simple data class that holds a symbol's name, sprite, and payout value. It is serializable so it can be filled in directly from the Unity Inspector.

**UIManager.cs**
Handles everything visual. It listens to events from GameManager and updates the balance text, result text, win popup, and spin button state. It also runs the coroutine that auto-hides the win panel after 2.5 seconds.

**WinAnimator.cs**
Attached to the WinPanel. Every time the panel is activated it plays a scale-pop animation that grows the panel in and then settles it to normal size.

**ReelSpinAnimator.cs**
Attached to each reel. When called after a reel stops, it moves the reel down slightly and snaps it back up to create a bounce effect.

**HandleAnimator.cs**
Attached to the handle image. When TriggerPull is called it rotates the handle 90 degrees on the Y axis over 0.2 seconds, holds it there for 0.6 seconds, then returns it to the original rotation over 0.2 seconds.

---

## Thought Process and Approach

**Keeping things separate**
From the start I wanted each script to own only one thing. GameManager handles logic, UIManager handles display, Reel handles its own animation. None of them reach into each other directly. This made it much easier to change one part without breaking another.

**Using events instead of direct references**
I used UnityEvents to connect the GameManager to the UIManager. The GameManager fires events like onWin, onLose, onGameOver, and onWinner. The UIManager just listens and reacts. This means I could completely redo the UI without touching a single line of game logic.

**Centralizing randomization**
All random calls go through RNGManager. This is a small thing but it matters. If I ever want to add weighted symbol probabilities or a seeded RNG for testing, I only need to change one place.

**Staggered reel starts**
Each reel gets a delay of i times 0.2 seconds before it starts spinning. This one small change makes the whole machine feel much more like a real slot machine instead of three images changing at the same time.

**Strip-based reels**
Instead of swapping one image per reel, I built each reel as a vertical strip of 4 images. During the spin all 4 shuffle fast. When it stops the strip fills in naturally around the landed symbol. This looks far more authentic and was worth the extra setup.

**End-game flow**
I did not want the game to just freeze when someone wins or loses. Both end states show a clear popup, lock the input so nothing can be clicked, and then auto-reset after 3 seconds. The game loop stays smooth and the player can immediately play again.

**The handle**
This was a small detail but I think it matters. Clicking a button is fine but pulling a handle feels like a slot machine. The Y-axis rotation gives it a physical depth that a Z rotation would not.

---

## Built With

- Unity 2022.3.18f1
- C# for all game logic and UI scripts
- TextMeshPro for UI text
- Unity UI system including Canvas, Image, Button, and VerticalLayoutGroup
- UnityEvents for decoupled communication between game and UI layers

---

Made for the Underpin Technology Unity Developer Intern Assignment
