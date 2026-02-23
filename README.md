# Zombie FPS (Unity, C#)

A 3D zombie survival FPS built in **Unity** featuring wave-based spawning, enemy AI navigation, and player combat systems.

## ✨ Features
- Wave-based enemy spawning
- Enemy AI navigation (NavMeshAgent)
- Player combat (shooting, health, damage)
- UI for core game stats (health, wave, etc.)
- Power-ups and difficulty scaling (if present)

## 🧰 Tech Stack
- Unity
- C#
- NavMesh (AI pathfinding)

## 📸 Demo
- Gameplay video: https://youtu.be/e1PocefSCZc

## 🕹️ How to Run
### Option 1: Open in Unity (recommended)
1. Install Unity Hub
2. Open this project folder in Unity
3. Load the main scene
4. Press Play

### Option 2: Download a Build
> If you publish a release build:
- Go to Releases
- Download the Windows build zip
- Run the executable

## 🧠 Architecture Overview
- **Enemy AI**: NavMeshAgent + state-based behaviour
- **Game Manager**: tracks wave state and progression
- **UI Manager**: updates HUD elements
- **Spawner**: spawns enemies based on wave settings

## ✅ What I Learned
- Real-time game state management
- Event-driven gameplay logic across multiple systems
- AI navigation and enemy behaviour design
- Debugging and iteration in a large Unity project
