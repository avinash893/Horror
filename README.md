# ☣️ Survival Horror Prototype

A Resident Evil-inspired 3D survival horror game built in Unity (C#). Players must survive as many days as possible against escalating zombie outbreaks while balancing hunger, infection rates, and limited ammunition.

---

## 🎮 Core Gameplay Mechanics

### 1. Day-Cycle Survival & Dynamic Zombie Evolution
- **Multi-Day Outbreak**: Each passing day increases horde density and triggers new zombie mutations.
- **Adaptive Zombie Archetypes**: Evolving enemy variants with specialized abilities (sprinters, tanks, spitters, and ambushers).

### 2. Survival & Vitals Systems
- **Infection Rate Management**: Slayed bites and environmental exposure raise infection; administer antivirals and meds to prevent turning.
- **Hunger & Stamina**: Scavenge food rations and clean water to maintain sprint stamina and physical health.

### 3. Combat & Ballistics
- **Firearms & Recoil**: Handguns and shotguns with realistic ammo counts, magazine reload cycles, and recoil impulse.
- **Molotov & Explosive Trajectory**: Custom `LineRenderer` parabolic arc calculating physics trajectory and impact combustion zones.
- **Animation Pipelines**: Blend tree animation states for ADS (aim-down-sights), weapon sway, firing kickback, and enemy hit-reactions.

### 4. Grid Inventory Management
- Classic *Resident Evil*-style spatial inventory with item rotation, slot constraints, combining recipes (herbs/chemicals), and quick-slots.

---

## 🛠️ Tech Stack & Architecture
- **Engine**: Unity 3D (Universal Render Pipeline)
- **Language**: C#
- **Architecture**: Decoupled Event-driven gameplay systems, State Machine AI controllers, ScriptableObject-driven item data.

---

## 🚀 Controls
| Action | Input |
|---|---|
| Move | `W / A / S / D` |
| Aim / Fire | `Right Click` / `Left Click` |
| Molotov Arc | `Hold G` (aim with LineRenderer), Release to throw |
| Inventory | `Tab` / `I` |
| Reload | `R` |
| Sprint | `Left Shift` |
| Interact / Scavenge | `E` |
