# Godot Interaction System

一个通用的 Godot 4 交互检测插件（C#）。

## 功能

- Area2D 范围检测可交互对象
- 自动显示/隐藏交互提示
- 可配置的输入动作和检测半径
- 信号通知（进入、离开、交互触发）
- `IInteractable` 接口，任何节点实现即可被交互

## 安装

将 `addons/interaction_system/` 复制到你的 Godot 项目中，然后在 项目设置 → 插件 中启用。

## 使用

1. 将 `InteractionComponent.tscn` 作为子场景添加到玩家节点
2. 让 NPC/物体实现 `IInteractable` 接口：

```csharp
using InteractionSystem;

public partial class MyNPC : Area2D, IInteractable
{
    public string InteractionText => "按 E 对话";

    public void OnInteract()
    {
        GD.Print("开始对话");
    }

    public void OnInteractionEnd()
    {
        GD.Print("结束对话");
    }
}
```

3. 在编辑器中调整 `DetectionRadius` 和 `InputAction`

## 信号

| 信号 | 说明 |
|------|------|
| `Interacted(Node target)` | 交互触发时 |
| `InteractableEntered(Node target)` | 可交互对象进入范围 |
| `InteractableExited(Node target)` | 可交互对象离开范围 |

## License

MIT
