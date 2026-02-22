# Godot Interaction System

A reusable interaction detection plugin for Godot 4 (C#).

## Features

- Area2D-based proximity detection for interactable objects
- Auto show/hide interaction hints with customizable text
- Configurable input action and detection radius (exported properties)
- Signals: Interacted, InteractableEntered, InteractableExited
- `IInteractable` interface — implement on any Area2D node

## Installation

Copy `addons/interaction_system/` into your Godot project, then enable it in Project Settings → Plugins.

## Usage

1. Add `InteractionComponent.tscn` as a child scene to your player node
2. Implement `IInteractable` on your NPC/object:

```csharp
using InteractionSystem;

public partial class MyNPC : Area2D, IInteractable
{
    public string InteractionText => "[Space] Talk";

    public void OnInteract()
    {
        GD.Print("Start conversation");
    }

    public void OnInteractionEnd()
    {
        GD.Print("End conversation");
    }
}
```

3. Adjust `DetectionRadius` and `InputAction` in the editor inspector

## Signals

| Signal | Description |
|--------|-------------|
| `Interacted(Node target)` | Emitted when interaction is triggered |
| `InteractableEntered(Node target)` | Emitted when an interactable enters range |
| `InteractableExited(Node target)` | Emitted when an interactable exits range |

## Requirements

- Godot 4.6+ (Mono/C#)
- .NET 8 SDK

## License

MIT
