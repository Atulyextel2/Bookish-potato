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
