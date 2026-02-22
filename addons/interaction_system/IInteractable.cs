namespace InteractionSystem;

/// <summary>
/// Interface for interactable objects.
/// Implement this on any Area2D node (NPC, item, door, etc.) to make it interactable.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Hint text displayed when the player enters interaction range.
    /// </summary>
    string InteractionText { get; }

    /// <summary>
    /// Called when the player presses the interaction key.
    /// </summary>
    void OnInteract();

    /// <summary>
    /// Called when the interaction ends (for cleanup, closing UI, etc.).
    /// </summary>
    void OnInteractionEnd();
}
