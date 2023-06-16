using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace Vain.SpellSystem.Elemental.GPU

{


    internal class ComputeShader
    {

        System.Collections.Generic.Dictionary<int, ShaderUniform> _uniforms = new System.Collections.Generic.Dictionary<int, ShaderUniform>();
        RenderingDevice _renderingDevice;
        Rid _shader;


        Rid _uniformSet;
        Rid _pipeline;

        Vector3I _size;


        bool _compiled;

       

        public ComputeShader(RDShaderFile shader,RenderingDevice renderingDevice)
        {

            var shaderBytecode = shader.GetSpirV();
            _shader = renderingDevice.ShaderCreateFromSpirV(shaderBytecode);
   
            _renderingDevice = renderingDevice;


        }

        public void AddImageUniform(int binding, ImageTexture texture)
        {
            
            
            var uTexture = new RDUniform{
                UniformType = RenderingDevice.UniformType.Image,
                Binding = binding,
                
            };
        

            var format = new RDTextureFormat{
                Format = RenderingDevice.DataFormat.R32G32B32A32Sfloat,
                Width = (uint)texture.GetWidth(),
                Height = (uint)texture.GetHeight(),
                UsageBits = RenderingDevice.TextureUsageBits.CanUpdateBit | RenderingDevice.TextureUsageBits.StorageBit | RenderingDevice.TextureUsageBits.CanCopyFromBit,
                
            };
            
            
            var gpuTexture = _renderingDevice.TextureCreate(format,new RDTextureView());
          
            uTexture.AddId(gpuTexture);

            Debug.Assert(!_uniforms.ContainsKey(binding));
            _uniforms.Add(binding,new ShaderUniform(){Type = UniformType.Texture,Buffer = gpuTexture , BufferUniform = uTexture});
            
          
            

        }
        public void AddBufferUniform(int binding, byte[] data)
        {
            var buffer = _renderingDevice.StorageBufferCreate((uint)data.Length, data);
            var uBuffer = new RDUniform()
            {
                UniformType = RenderingDevice.UniformType.StorageBuffer,
                Binding = binding
            };
            uBuffer.AddId(buffer);
            
            Debug.Assert(!_uniforms.ContainsKey(binding));


            _uniforms.Add(binding,new ShaderUniform(){Type = UniformType.Buffer,Buffer = buffer, BufferUniform = uBuffer});
            
        }
        public void UpdateUniform(int binding,byte[] data)
        {   
            Debug.Assert(_uniforms.ContainsKey(binding));
            
            var uniform = _uniforms[binding];
            if(uniform.Type == UniformType.Texture)
                _renderingDevice.TextureUpdate(uniform.Buffer,0,data);
            else
                _renderingDevice.BufferUpdate(uniform.Buffer,0,(uint)data.Length,data);
            

        }

        public byte[] GetData(int binding)
        {
            
            Debug.Assert(_uniforms.ContainsKey(binding));

            var uniform = _uniforms[binding];
            if(uniform.Type == UniformType.Texture)
                return _renderingDevice.TextureGetData(uniform.Buffer,0);
            else
                return _renderingDevice.BufferGetData(uniform.Buffer);

        }
        public void CompileShader(Vector3I workgroups)
        {
                
            
            _uniformSet = _renderingDevice.UniformSetCreate(new Array<RDUniform>(_uniforms.Values.Select(u => u.BufferUniform)), _shader, 0);
            _pipeline = _renderingDevice.ComputePipelineCreate(_shader);
            
            _size = workgroups;
            _compiled = true;
        }

        public void Dispatch()
        
        {

            if(!_compiled)
            {
                //TODO: Add better error logging
                throw new System.Exception("Compute shader not compiled, please compile it before dispatching");
            }
            var computeList = _renderingDevice.ComputeListBegin();
            _renderingDevice.ComputeListBindComputePipeline(computeList, _pipeline);
            _renderingDevice.ComputeListBindUniformSet(computeList, _uniformSet, 0);
            _renderingDevice.ComputeListDispatch(computeList, (uint)_size.X, (uint)_size.Y, (uint)_size.Z);
        
            _renderingDevice.ComputeListEnd();

            _renderingDevice.Submit();


            _renderingDevice.Sync();

            


            
            
        }


    }
}