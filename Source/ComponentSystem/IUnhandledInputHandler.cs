using Godot;

namespace Vain
{
    interface IUnhandledInputHandler
    {
        void UnhandledInput(InputEvent inputEvent);
    }
}