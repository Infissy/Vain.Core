using System;
using System.Collections.Generic;
using System.Diagnostics;

using Godot;

using Vain.Log;
using Vain.SpellSystem.Elemental;


namespace Vain.SpellSystem.Elemental.GPU
{
    partial class Renderer : TextureRect
    {


       
        [Export] 
        RDShaderFile _mainShaderFile;

        ShaderMaterial _overlayMaterial;


        ComputeShader _mainShader;
        ElementalSystem _elementalSystem;

        Camera3D _activeCamera;


        Stopwatch _stopWatch = new Stopwatch();
        

        double _logDelta = 0;
        
        byte[] _buffer = new byte[100000000];
        int n = 0;
        public override void _Ready()
        {
            base._Ready();



            _elementalSystem = GetParent<ElementalSystem>();

            var size = GetWindow().Size;
            Texture = ImageTexture.CreateFromImage(Image.Create((int)size.X,(int)size.Y,false,Image.Format.Rgbaf));

            _activeCamera =  GetViewport().GetCamera3D();
            if(_activeCamera == null)
            {
                //TODO: Add better error messages and handle cases in which the developer didn't add a camera
                throw new Exception("There's no camera in the scene, it is required by the Renderer in order to work, please add it to the scene");
            }





            var renderingDevice = RenderingServer.CreateLocalRenderingDevice();
            _mainShader = new ComputeShader(_mainShaderFile,renderingDevice);

      
            _mainShader.AddImageUniform(0,Texture as ImageTexture); 


            GPUShaderDataEncoder.EncodeBufferOrdered(_buffer.AsSpan(),_elementalSystem.Buffer,_activeCamera.Position);
           
            _mainShader.AddBufferUniform(1,_buffer);
            _mainShader.AddBufferUniform(2,GPUShaderDataEncoder.EncodeCamera(_activeCamera));


            _mainShader.CompileShader(new Vector3I((int)size.X,(int)size.Y,1));



            

            //Finisci inizializzazione reenderer, associa la texture e vedi
        }



        public override void _Process(double delta)
        {
           
            runShader();
           
             
              
            
        }

        async void runShader()
        {
            
            var delta = 0L;
            _stopWatch.Restart();
            GPUShaderDataEncoder.EncodeBufferOrdered(_buffer.AsSpan(),_elementalSystem.Buffer,_activeCamera.Position);
            _mainShader.UpdateUniform(1,_buffer);
           
            _mainShader.UpdateUniform(2,GPUShaderDataEncoder.EncodeCamera(_activeCamera));

            delta = _stopWatch.ElapsedMilliseconds;
            Logger.Runtime("    Uniform update: ",  delta + "ms");



            _mainShader.Dispatch();
            Logger.Runtime("    Dispatch: ",  _stopWatch.ElapsedMilliseconds - delta  + "ms");
            delta += _stopWatch.ElapsedMilliseconds;


            var size = Texture.GetSize();
            (Texture as ImageTexture).Update(Image.CreateFromData((int)size.X,(int)size.Y,false,Image.Format.Rgbaf,_mainShader.GetData(0)));
            Logger.Runtime("    Texture update: ", _stopWatch.ElapsedMilliseconds - delta + "ms");
            
            Logger.Runtime("Compute shader time: ",  _stopWatch.ElapsedMilliseconds.ToString() + "ms");
        
        }


       
        

    }
}