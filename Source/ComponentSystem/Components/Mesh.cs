using Godot;

namespace Vain
{

    public class Mesh : Component, IInitalizable
    {
        [EditableField]
        PackedScene _meshPrefab;


        MeshInstance _mesh;

        Movable _movable;

        public void Initialize()
        {
            _movable = GetComponent<Movable>();

            _mesh = _meshPrefab.Instance() as MeshInstance;

        }
    }   
}