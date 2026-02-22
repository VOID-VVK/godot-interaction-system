namespace InteractionSystem;

/// <summary>
/// 可交互对象接口。
/// 所有可交互的 NPC、物体等需要实现此接口。
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// 交互提示文本（进入范围时显示）。
    /// </summary>
    string InteractionText { get; }

    /// <summary>
    /// 开始交互（玩家按下交互键时调用）。
    /// </summary>
    void OnInteract();

    /// <summary>
    /// 结束交互（交互完成或取消时调用，用于清理资源、关闭 UI 等）。
    /// </summary>
    void OnInteractionEnd();
}
