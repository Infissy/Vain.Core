using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
namespace Vain.SpellSystem.Elemental
{

    /// <summary>
    /// Resource that contains all the particles needed for Rendering and physics.
    /// Adding is only possible with the given functions.
    /// ! DO NOT ADD OR REMOVE FROM CurrentBuffer/PastBuffer, IT CAN CAUSE UNWANTED PROBLEMS, design flaw soon to be fixed 
    /// </summary>
    partial class ElementalParticleBuffer : Resource
    {



        struct ParticleData
        {
            public ElementalParticleResource Resource;
            public uint ClusterID;
        }
        //FIXME: At the moment indexing is incomplete, it doesn't account for particle deletion nor for reaching maximum list size

        Dictionary<int,ParticleData> _particleResources = new Dictionary<int, ParticleData>();
        List<ElementalMeshResource> _particles;
        int _bufferIndex;
        Memory<ElementalParticle>[] _buffers = {new ElementalParticle[200000], new ElementalParticle[200000]};
        

        List<List<uint>> Clusters = new List<List<uint>>();

        int _lastParticleIndex = -1;


        //TODO: Avoid having exposed the internal buffer to every class, at the moment both of them are exposed for physics system to work peorperly
        // maybe look into list copy performance so it can copy it every step and there is way more data safety

        public int ParticleCount{get;private set;}
        public Memory<ElementalParticle> CurrentBuffer
        {
                
            get
            {
                return _buffers[_bufferIndex];
            }
            private set
            {
                _buffers[_bufferIndex] = value;
            }
        }


        public  Memory<ElementalParticle>  PastBuffer
        {
            get
            {
                return _buffers[1-_bufferIndex];
            }
             private set
            {
                _buffers[1-_bufferIndex] = value;
            }
        }


        public void Update()
        {
            _bufferIndex = 1 - _bufferIndex;
        }
        
      
        public void AddParticle (ElementalParticleResource particleResource, Vector3 position)
        {

            var particle = particleResource.GetParticle();

            particle.Position = position;
            particle.ID = getIndex();
            CurrentBuffer.Span[particle.ID] = particle;
            PastBuffer.Span[particle.ID]  = particle;

            var data = new ParticleData{ClusterID = 0,Resource = particleResource};

            ParticleCount++;

            _particleResources.Add(particle.ID,data);
            
        }
        //TODO: implement links between particles
        public void AddCluster(ElementalParticleResource particleResource, Vector3[] positions, Dictionary<int,int> _links = null)
        {
            List<ElementalParticle> particles = new List<ElementalParticle>();


            foreach (var position in positions)
            {
                var particle = particleResource.GetParticle();
                AddParticle(particleResource,position);
            }

        }
        
        public ElementalParticleResource GetParticleResource(ElementalParticle particle)
        {
           return _particleResources[particle.ID].Resource;
        }
        
        public ElementalParticleResource GetParticleResource(int ID)
        {
           return _particleResources[ID].Resource;
        }
      
        //FIXME: Add smart indexing so it doesn't overflow or 
        int getIndex()
        {
            _lastParticleIndex++;
            return (int) _lastParticleIndex;
        }
    }
    
}