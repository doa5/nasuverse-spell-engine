# Nasuverse Spell Engine

A single-screen, turn-based **Nasuverse magic simulator** built in C# (.NET 10) to explore modular domain design, shared world-state mutation, and cross-system magical interactions. Instead of a scrolling console log, the application behaves like a live, redrawing dashboard: selecting characters and casting spells mutates a shared environment in real time.

## Concept

This sandbox focuses on **systemic interaction**. Three Nasuverse characters share a single room. Their actions alter environmental variables (atmospheric mana density, entropy, reality texture) that in turn change how the *other* characters' spells behave.

### Live Terminal Dashboard

The console is cleared and redrawn every turn, simulating a live monitor:

- **Header / Environmental Panel** — active turn count, room durability, reality texture, entropy %, and mana density.
- **Active Character Panel** — the selected character's current mana and unique subsystem traits.
- **Action & Spell Menu** — cast an available spell, swap the active character, or reset/exit.
- **Buffered Event Feed** — a rolling log (capped at 6 entries) of recent actions, environmental shifts, and flavor commentary (e.g. reactions from Taiga).

### Persistent Shared World State

The `WorldState` exists independently of any single character and persists for the whole session:

- **Turn Counter** — only advances when an action actually modifies the environment or a spell resolves; navigating menus or swapping characters does not consume a turn.
- **Reality Texture** — the dominant ruleset governing the room (e.g. standard Human World, True Ancestor Territory, Frozen Heat Death Void).
- **Entropy & Saturation Counters** — dynamic meters tracking temporary environmental changes caused by heavy spellcasting.

## Character Interaction Synergy

The core showcase is how **Aoko**, **Arcueid**, and **Saber** modify — and react to — the same environment. Their magic systems amplify, suppress, or disrupt one another.

```
┌────────────────────────┐
│     SABER (Dragon)      │
│ Saturates atmosphere    │
└───────────┬─────────────┘
            │
 Saturates air │ Strips ambient mana
 lowers costs  │ forces core reliance
            ▼
┌──────────────────────┐        ┌──────────────────────┐
│ AOKO (5th Magic)      │        │ ARCUEID (True Ancestor)│
│ Accumulates entropy   ├───────►│ Overwrites texture     │
└──────────────────────┘        └──────────────────────┘
 Drives world into                Decaying environment
 Heat Death Void                  weakens Earth link
```

### Aoko Aozaki — The Fifth Magic

- **Mechanic:** Time-borrowing and thermodynamic entropy redirection instead of standard mana efficiency.
- **Signature Spells:** *Earthlight Starbow* (standard heavy strike) and *Fifth Magic: Redshift* (zero mana cost, switches character to adult Aoko, but adds +35% to global Heat Death Entropy).
- **World Impact:** Pushing entropy to 100% transforms the room into a Heat Death Void.
- **Interactions:**
  - With **Saber**: free-casts standard spells off ambient atmospheric mana when the room is saturated.
  - With **Arcueid**: under Millennial Castle, non-Fifth Magic spells cost double mana as human magecraft is suppressed.

### Arcueid Brunestud — The True Ancestor

- **Mechanic:** Direct environmental rewrite (Marble Phantasm) drawing on Earth's backing, bypassing conventional human circuits.
- **Signature Spells:** *Event Storage* (direct physical claw strike) and *Millennial Castle Brunestud* (overwrites room texture).
- **World Impact:** Overwrites the active texture to "Millennial Castle" and clears all ambient atmospheric energy.
- **Interactions:**
  - With **Saber**: instantly strips atmospheric mana from Mana Burst, forcing Saber back onto her internal Dragon Core pool.
  - With **Aoko**: at 100% Heat Death Entropy, the decaying room weakens Arcueid's Earth connection, doubling the mana cost to manifest Millennial Castle.

### Saber / Artoria Pendragon — King of Knights

- **Mechanic:** Internal high-density mana production via her Dragon Core, discharging excess energy into the surrounding air.
- **Signature Spells:** *Mana Burst* (saturates atmosphere for 3 turns) and *Excalibur*.
- **World Impact:** Sets atmospheric mana density to HIGH for a temporary duration.
- **Interactions:**
  - With **Aoko**: atmospheric saturation grants free energy, letting Aoko cast heavy spells without relying on entropy-generating Fifth Magic.
  - With **Arcueid**: high-density dragon mana introduces "noise" into the natural order, making environmental manipulation harder until Arcueid forcibly purges it.

## Project Structure

```
spell-engine/
├── Application/
│   ├── CharacterCatalog.cs       # Static catalog of character/spell definitions
│   ├── CharacterDefinition.cs
│   ├── CharacterFactory.cs       # Builds Character instances from definitions
│   ├── DashboardRenderer.cs      # Clears & redraws the dashboard UI
│   ├── DashboardView.cs
│   └── TrainingSession.cs        # Main event loop: selection, casting, turn advance
├── Domain/
│   ├── Character.cs              # Core character model (mana, state)
│   ├── CastSpellResult.cs
│   ├── Spell.cs
│   ├── SpellEffectResult.cs
│   ├── ResourcePool.cs
│   ├── ICastingRules.cs / DefaultCastingRules.cs
│   ├── ITransformState.cs
│   ├── Characters/                # Character-specific casting rules & transform states
│   │   ├── AokoCastingRules.cs
│   │   ├── AokoTransformState.cs
│   │   ├── ArcueidCastingRules.cs
│   │   └── SaberCastingRules.cs
│   ├── Effects/                   # Composable spell effects
│   │   ├── ISpellEffect.cs
│   │   ├── DamageEffect.cs
│   │   ├── EntropyEffect.cs
│   │   ├── AtmosphericBurstEffect.cs
│   │   ├── ExtendTransformEffect.cs
│   │   ├── RestoreManaEffect.cs
│   │   └── TextureEffect.cs
│   └── World/
│       ├── WorldState.cs          # Shared, persistent environment state
│       ├── RealityTexture.cs
│       └── TaigaCommentary.cs     # Flavor-text reactions
└── Program.cs                     # Entry point
```

### Architecture Notes

- **Effects as composable units** — each spell is defined as a set of `ISpellEffect` implementations (damage, entropy shift, texture overwrite, atmospheric burst, etc.), allowing a single spell to touch both the caster and the shared `WorldState`.
- **Per-character casting rules** — `ICastingRules` implementations (e.g. `AokoCastingRules`, `ArcueidCastingRules`, `SaberCastingRules`) encode how each character's mana costs and eligibility are modified by current world conditions, which is how the cross-character synergy is implemented without hardcoding character-to-character checks.
- **Separation of Application vs Domain** — `Domain` contains pure game/magic logic (characters, spells, effects, world state) with no UI concerns; `Application` wires that logic to the dashboard-style console UI and the turn loop.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run

```powershell
cd spell-engine
dotnet run
```

### Build

```powershell
dotnet build spell-engine.slnx
```