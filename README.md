# Memory-Match Card Game

A simple, cross-platform (Desktop / iOS / Android) memory-match card game made in Unity. Flip N cards at a time (default 2) to find matching sets.

---

## 📦 Project Assets

- **Main Scene**  
  `Assets/_0_Main/Scenes/_0_Main.unity`

- **Card Prefab**  
  `Assets/_0_Main/Prefabs/CardViewPrefab.prefab`

- **Sprite Atlas**  
  `Assets/_0_Main/UIs/SpriteAtlases/Cards/CardsAtlas_01.jpg`

- **Card Data (ScriptableObjects)**  
  `Assets/_0_Main/Resources/CardData_Set_1/*.asset`

- **Game Config**  
  `Assets/_0_Main/Resources/GameConfig/GameConfig_01.asset`

- **Sound Effects Config**  
  `Assets/_0_Main/Resources/GameSoundConfig/SoundEffectConfig_01.asset`

- **Audio Files**  
  `Assets/_0_Main/Audios/` (flip, match, mismatch, gameover, click)

---

## 🚀 How It Works

1. **CompositionRoot** reads all assets (GameConfig, SoundEffectConfig, CardData, Prefab, SpriteAtlas, AudioSource, UIController, LayoutSelector) from the Inspector.  
2. **Start Screen** lets you pick a grid preset (rows×cols) and press **Start**.  
3. **BoardManager** instantiates a shuffled deck of cards (N copies per match-group), using `CardViewPrefab` and your sprite atlas.  
4. **GameController** listens for clicks/taps (via `MouseVectorInputProvider` or `TouchVectorInputProvider`), enqueues flip commands, and drives flip animations through `AnimationManager`.  
5. **GameStateMachine** collects flips in groups of **matchGroupSize**, waits the configured flip duration, then checks for a match:  
   - **Match** → play “match” sound, increment Matches.  
   - **Mismatch** → play “mismatch” sound, flip cards back, increment Tries.  
6. When all cards are matched, play “game-over” sound and show **Home** button to reset.

---

## 🔧 Customization (No Code Changes)

1. **CardData_Set_1**  
   - Swap or add `CardData` SOs (face/back sprites + `matchId`).

2. **GameConfig_01.asset**  
   - **presets**: grid names & sizes for the dropdown.  
   - **matchGroupSize**: cards per match (2, 3, etc.).  
   - **flipAnimationDuration**: delay before checking.

3. **SoundEffectConfig_01.asset**  
   - Assign your own AudioClips for each `SoundType`.

4. **CardViewPrefab**  
   - Change art, components, BoxCollider, or UI elements in the prefab.

5. **UI & Layout**  
   - Tweak `UIController` Canvas or `LayoutSelector` dropdown in the Editor.

6. **Audio Files**  
   - Replace `.wav`/`.mp3` under `Assets/_0_Main/Audios/` as desired.

All changes take effect immediately when you press **Play** or build—no C# edits required.

---

## 📁 Folder Structure

Assets/
└─ _0_Main/
├─ Scenes/
│    └─ _0_Main.unity
├─ Prefabs/
│    └─ CardViewPrefab.prefab
├─ UIs/
│    └─ SpriteAtlases/CardsAtlas_01.jpg
├─ Audios/
│    └─ flip.wav, match.wav, …
└─ Resources/
├─ CardData_Set_1/        ← CardData SOs
├─ GameConfig/            ← GameConfig_01.asset
└─ GameSoundConfig/       ← SoundEffectConfig_01.asset

Scripts/
├─ CompositionRoot/
├─ Presentation/
├─ Domain/
└─ Infrastructure/

---

Enjoy customizing and extending your Memory-Match game—**no code changes** needed!  

---

## 🤝 Contributing & Branching Strategy

We follow a **protected-branch workflow** to keep our main codebase stable while encouraging contributions.

### 🔀 Branch Types

| Branch Type      | Purpose                                         | Rules |
|------------------|-------------------------------------------------|-------|
| `main`           | Production-ready code. Always stable and release-ready. | **Protected** — No direct pushes. PRs only, must be approved by Code Owner(s). |
| `develop`        | Integration branch where features are merged before release. | **Protected** — No direct pushes. PRs only, must be approved by Code Owner(s). |
| `release/*`      | Release prep branches (e.g., `release/1.0.0`). | **Protected** — Same rules as `main`. |
| `feature/*`      | New features from `develop` (e.g., `feature/new-game-mode`). | Push allowed, force-push blocked. Merged into `develop` via PR. |
| `hotfix/*`       | Urgent fixes to be merged directly into `main` and `develop`. | Push allowed, force-push blocked. PRs required for merging to protected branches. |

---

### 📌 Branch Naming Conventions

- **Feature**: `feature/<short-description>`  
  _Example_: `feature/add-scoreboard`
- **Hotfix**: `hotfix/<short-description>`  
  _Example_: `hotfix/fix-null-ref`
- **Release**: `release/<version>`  
  _Example_: `release/1.1.0`

---

### 🛠 How to Contribute

1. **Sync with develop**
   ```bash
   git checkout develop
   git pull
   ```
2. **Create your feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Commit & push**
   ```bash
   git commit -m "Add your message here"
   git push -u origin feature/your-feature-name
   ```
4. **Open a Pull Request**
   - **Base branch**: `develop`  
   - **Compare branch**: `feature/your-feature-name`
5. **Wait for Review**
   - All PRs to `develop` and `main` require review from a **Code Owner** (currently: `@your-github-username`).
   - The PR will not merge without approval.
6. **Address Feedback**
   - Any new commits after approval will dismiss the previous approval — you’ll need a fresh review.
7. **Merge**
   - We use **Squash merge** (one clean commit per PR).
   - `develop` merges to `main` only after testing and review.

---

### 🔒 Branch Protection Rules (Summary)

- **`main` & `release/*`**
  - PRs required, no direct pushes
  - Code Owner approval required
  - Force pushes & deletions blocked
- **`develop`**
  - PRs required, no direct pushes
  - Code Owner approval required
  - Force pushes & deletions blocked
- **`feature/*` & `hotfix/*`**
  - Push allowed
  - Force pushes blocked
  - Merges into protected branches require PR & approval

---

### 👑 Code Owners

We use a `.github/CODEOWNERS` file to define who can approve changes to protected branches.  
Current global Code Owner:
```
* @your-github-username
/.github/CODEOWNERS @your-github-username
```
> Add other maintainers or teams on the `*` line if you want them to be able to approve PRs.

---

### 📣 TL;DR Flow

```
(feature/*)  →  PR →  develop  →  PR →  main
(hotfix/*)   →  PR →  main (+ cherry-pick/merge into develop)
```

---

## 🔄 Contribution Workflow Overview (ASCII)

```
┌───────────────────────┐
│     feature/*          │
│  (or hotfix/*)         │
│  Push allowed          │
│  Force-push blocked    │
└──────────┬─────────────┘
           │  PR
           ▼
┌───────────────────────┐
│       develop          │
│  No direct pushes      │
│  PR required           │
│  Code Owner approval   │
│  Force-push blocked    │
│  Linear history (opt)  │
└──────────┬─────────────┘
           │  PR
           ▼
┌───────────────────────┐
│         main           │
│  No direct pushes      │
│  PR required           │
│  Code Owner approval   │
│  Force-push blocked    │
│  Linear history        │
└──────────┬─────────────┘
           │  (release prep)
           ▼
┌───────────────────────┐
│     release/*          │
│  Same as main rules    │
└───────────────────────┘
```
