#if TOOLS
using Godot;

namespace InteractionSystem;

[Tool]
public partial class InteractionPlugin : EditorPlugin
{
    public override void _EnterTree()
    {
        GD.Print("[InteractionSystem] Plugin enabled.");
    }

    public override void _ExitTree()
    {
        GD.Print("[InteractionSystem] Plugin disabled.");
    }
}
#endif
