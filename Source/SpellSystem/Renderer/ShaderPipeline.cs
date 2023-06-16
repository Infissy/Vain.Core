using Godot;


using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vain.SpellSystem.Elemental.GPU
{
    internal class ShaderPipeline
    {
        ComputeShader _physicsShader;
        ComputeShader _renderingShader;

        void Setup()
        {

        }


        async void LaunchPhysicsStage()
        {
            await Task.Run(_physicsShader.Dispatch);
        }
        async void LaunchRenderingStage()
        {
            await Task.Run(_renderingShader.Dispatch);
        }

        internal Texture2D Result {get;set;}
        void AddStage()
        {

        }
    }
}