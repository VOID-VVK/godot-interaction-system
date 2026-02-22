using Godot;

namespace InteractionSystem;

/// <summary>
/// Interaction detection component.
/// Attach to the player node. Uses Area2D to detect interactable objects in range,
/// shows/hides interaction hints, and handles interaction input.
/// </summary>
public partial class InteractionComponent : Area2D
{
    // ==================== Constants ====================
    private const string DEFAULT_INPUT_ACTION = "ui_accept";

    // ==================== Exported Properties ====================
    [ExportGroup("Interaction")]
    [Export] public string InputAction { get; set; } = DEFAULT_INPUT_ACTION;

    [Export] public float DetectionRadius
    {
        get => _detectionRadius;
        set
        {
            _detectionRadius = value;
            UpdateCollisionRadius();
        }
    }

    // ==================== Signals ====================
    [Signal] public delegate void InteractedEventHandler(Node target);
    [Signal] public delegate void InteractableEnteredEventHandler(Node target);
    [Signal] public delegate void InteractableExitedEventHandler(Node target);

    // ==================== Node References ====================
    private CanvasLayer? _hud;
    private Label? _interactLabel;
    private CollisionShape2D? _collisionShape;

    // ==================== State ====================
    private Node? _currentInteractable;
    private float _detectionRadius = 100f;

    /// <summary>
    /// The current interactable object in range (read-only).
    /// </summary>
    public Node? CurrentInteractable => _currentInteractable;

    public override void _Ready()
    {
        _hud = GetNodeOrNull<CanvasLayer>("HUD");
        _interactLabel = _hud?.GetNodeOrNull<Label>("InteractLabel");
        _collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");

        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;

        ShowHint(false);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(InputAction))
        {
            InteractWithCurrent();
        }
    }

    // ==================== Detection Callbacks ====================

    private void OnAreaEntered(Area2D area)
    {
        if (area is IInteractable)
        {
            _currentInteractable = area;
            ShowHint(true);
            EmitSignal(SignalName.InteractableEntered, area);
        }
    }

    private void OnAreaExited(Area2D area)
    {
        if (_currentInteractable != area) return;

        _currentInteractable = null;
        ShowHint(false);
        EmitSignal(SignalName.InteractableExited, area);
    }

    // ==================== Interaction ====================

    /// <summary>
    /// Trigger interaction with the current interactable object.
    /// </summary>
    public void InteractWithCurrent()
    {
        if (_currentInteractable is IInteractable interactable)
        {
            interactable.OnInteract();
            EmitSignal(SignalName.Interacted, _currentInteractable);
        }
    }

    /// <summary>
    /// End the current interaction.
    /// </summary>
    public void EndCurrentInteraction()
    {
        if (_currentInteractable is IInteractable interactable)
        {
            interactable.OnInteractionEnd();
        }
    }

    // ==================== Internal ====================

    private void ShowHint(bool show)
    {
        if (_interactLabel == null) return;

        _interactLabel.Visible = show;

        if (show && _currentInteractable is IInteractable interactable)
        {
            _interactLabel.Text = interactable.InteractionText;
        }
    }

    private void UpdateCollisionRadius()
    {
        if (_collisionShape?.Shape is CircleShape2D circle)
        {
            circle.Radius = _detectionRadius;
        }
    }
}
