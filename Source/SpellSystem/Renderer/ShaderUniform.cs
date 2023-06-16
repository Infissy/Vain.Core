
using Godot;
namespace Vain.SpellSystem.Elemental.GPU
{

    enum UniformType
    {
        Texture,
        Buffer
    }
    class ShaderUniform
    {
        public UniformType Type{get;set;}   
        public Rid Buffer {get;set;}
        public RDUniform BufferUniform{get;set;}
    }
}