using Godot;

namespace InteractionSystem;

/// <summary>
/// 交互检测组件。
/// 挂载到玩家节点上，使用 Area2D 检测范围内的可交互对象，
/// 显示/隐藏交互提示，处理交互输入。
/// </summary>
public partial class InteractionComponent : Area2D
{
    // ==================== 常量 ====================
    private const string DEFAULT_INPUT_ACTION = "ui_accept";

    // ==================== 导出变量 ====================
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

    // ==================== 信号 ====================
    [Signal] public delegate void InteractedEventHandler(Node target);
    [Signal] public delegate void InteractableEnteredEventHandler(Node target);
    [Signal] public delegate void InteractableExitedEventHandler(Node target);

    // ==================== 节点引用 ====================
    private CanvasLayer? _hud;
    private Label? _interactLabel;
    private CollisionShape2D? _collisionShape;

    // ==================== 状态 ====================
    private Node? _currentInteractable;
    private float _detectionRadius = 100f;

    /// <summary>
    /// 当前范围内的可交互对象（只读）。
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

    // ==================== 检测回调 ====================

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

    // ==================== 交互操作 ====================

    /// <summary>
    /// 触发当前交互对象的交互。
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
    /// 结束当前交互。
    /// </summary>
    public void EndCurrentInteraction()
    {
        if (_currentInteractable is IInteractable interactable)
        {
            interactable.OnInteractionEnd();
        }
    }

    // ==================== 内部方法 ====================

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
