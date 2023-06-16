using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace Vain.SpellSystem.Elemental
{
    //TODO: maybe implement various encoding types based on various shaders
    static class GPUShaderDataEncoder
    {
        static ElementalParticle[] _ordered = new ElementalParticle[500000];
        static float[] _distances = new float[500000];
        //FIXME: Sort only the elements changed, and do not copy everytime a new array, this is unoptimal
        /// <summary>
        /// Ordered by distance to point
        /// </summary>

        public static void EncodeBufferOrdered (Span<byte> bytes,ElementalParticleBuffer buffer,Vector3 point)
        {
            if(buffer.ParticleCount != _ordered.Length)
            {

                Array.Resize(ref _ordered,buffer.ParticleCount);
                Array.Resize(ref _distances,buffer.ParticleCount);
            }
            Parallel.For(0,buffer.ParticleCount, (i) =>
            
                {
                    _distances[i] = buffer.CurrentBuffer.Span[i].Position.DistanceTo(point);
                }
            );
          

            
            Array.Copy(buffer.CurrentBuffer.ToArray(),_ordered,buffer.ParticleCount);
            Array.Sort(_distances,_ordered);
            

            int size = 0;
            size += 4 + 12; //Count
            size += 32 * buffer.CurrentBuffer.Length;
            
           
            BitConverter.GetBytes((uint) buffer.ParticleCount).AsSpan().CopyTo(bytes.Slice(0,4));
            
            for (int i = 0; i < buffer.ParticleCount; i++)
            {
                var particle = buffer.CurrentBuffer.Span[i];
             
                EncodeElementalParticle(particle,buffer.GetParticleResource(particle)).AsSpan().CopyTo(bytes.Slice(16 + 32 * i,32));
            }
           

            
        }

        public static byte[] EncodeElementalParticle(ElementalParticle particle,ElementalParticleResource particleResource)
        {
            var bytes = new byte[32];
            BitConverter.GetBytes(particle.Position.X).CopyTo(bytes,0);
            BitConverter.GetBytes(particle.Position.Y).CopyTo(bytes,4);
            BitConverter.GetBytes(particle.Position.Z).CopyTo(bytes,8);
            BitConverter.GetBytes(particle.Radius).CopyTo(bytes,12);
            
            BitConverter.GetBytes(particleResource.Color.R).CopyTo(bytes,16);
            BitConverter.GetBytes(particleResource.Color.G).CopyTo(bytes,20);
            BitConverter.GetBytes(particleResource.Color.B).CopyTo(bytes,24);
            

            return bytes;
            
        }

        public static byte[] EncodeCamera(Camera3D camera)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(EncodeTransform(camera.Transform));
            bytes.AddRange(BitConverter.GetBytes(camera.Fov));
            bytes.AddRange(new byte[12]);

            return bytes.ToArray();
            
        }

        
        public static byte[] EncodeTransform(Transform3D transform)
        {
             
        
            List<byte> bytes = new List<byte>();
        
            bytes.AddRange(BitConverter.GetBytes(transform[0,0]));
            bytes.AddRange(BitConverter.GetBytes(transform[1,0]));
            bytes.AddRange(BitConverter.GetBytes(transform[2,0]));
            bytes.AddRange(BitConverter.GetBytes(transform[3,0]));

            bytes.AddRange(BitConverter.GetBytes(transform[0,1]));
            bytes.AddRange(BitConverter.GetBytes(transform[1,1]));
            bytes.AddRange(BitConverter.GetBytes(transform[2,1]));
            bytes.AddRange(BitConverter.GetBytes(transform[3,1]));

            bytes.AddRange(BitConverter.GetBytes(transform[0,2]));
            bytes.AddRange(BitConverter.GetBytes(transform[1,2]));
            bytes.AddRange(BitConverter.GetBytes(transform[2,2]));
            bytes.AddRange(BitConverter.GetBytes(transform[3,2]));

            bytes.AddRange(BitConverter.GetBytes(0f));
            bytes.AddRange(BitConverter.GetBytes(0f));
            bytes.AddRange(BitConverter.GetBytes(0f));
            bytes.AddRange(BitConverter.GetBytes(1f));
          
       
            return bytes.ToArray();
            

        

        }
    }
    

}
