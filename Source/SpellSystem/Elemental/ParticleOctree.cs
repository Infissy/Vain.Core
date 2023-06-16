using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;


namespace Vain.SpellSystem.Elemental
{
    /*
    class ParticleOctree
    {



        OctreeNode _root;
        public ParticleOctree(Vector3 position, float size, int nodeCapacity)
        {
            _root = new OctreeNode(new Box(position,  size),nodeCapacity);
        }

        public ICollection<int> Query(Vector3 center, float size)
        {
            ConcurrentDictionary<int,byte> ids = new ConcurrentDictionary<int,byte>();
            
            _root.QueryRange(ref ids, new Box(center,size));

            return ids.Keys;
        }
        public void Insert(ElementalParticle particle)
        {
            _root.Insert(new OctreeNode.Particle(){id = particle.ID,aabb = new Box(particle.Position, particle.Radius )});
        }

        internal struct OctreeNode
        {
            internal struct Octans 
            {
                OctreeNode? _unw;
            }
            internal struct Particle
            {
                internal int id;
                internal Box aabb;
            }
         
            readonly int _capacity;

            Box _box;
            ConcurrentBag<Particle> _particles;

            int _count = 0;
       

            Octans _octans; 


            bool _divided = false;    

            internal OctreeNode(Box aabb, int capacity)
            {
                
                _box = aabb;
               

                _particles = new ConcurrentBag<Particle>();
             
                _capacity = capacity;
            }


            internal void Insert(Particle particle)
            {
                if(!Box.Intersects(_box,particle.aabb))
                    return;



                if(_count >= _capacity)
                {

                    if(!_divided)
                        divide();
                    
                    var octans = _octans;
                    
                    Parallel.For(0,8,(i)=>
                        
                        {
                            
                                octans[i].Insert(particle);
                        }
                    );
                    
                    
                    
                    return;
                }



            

                _particles.Add(particle);
                _count++;


                if(_count >= _capacity)
                {
                   
                    foreach (var p in _particles)
                    {
                        
                        Insert(p);
                    }
                    

                    return;
                }


            }   

          


            void divide()
            {
                // UP/DOWN Y  NORTH/SOUTH Z  EAST/WEST X
                var unw = new Vector3(1,1,1);
                var une = new Vector3(-1,1,1);
                var usw = new Vector3(1,1,-1);
                var use = new Vector3(-1,1,-1);
                var dnw = new Vector3(1,-1,1);
                var dne = new Vector3(-1,-1,1);
                var dsw = new Vector3(1,-1,-1);
                var dse = new Vector3(-1,-1,-1);
                _octans = new OctreeNode[8];


                var unwPos = _box.position + Vector3.One * _box.size * unw / 2;
                _octans[0] = new OctreeNode(new Box(unwPos, _box.size/2), _capacity);
                var unePos = _box.position + Vector3.One * _box.size * une / 2;
                _octans[1] = new OctreeNode(new Box(unePos, _box.size/2), _capacity);
                var uswPos = _box.position + Vector3.One *_box.size * usw / 2;
                _octans[2] = new OctreeNode(new Box(uswPos, _box.size/2), _capacity);
                var usePos = _box.position +Vector3.One * _box.size * use / 2;
                _octans[3] = new OctreeNode(new Box(usePos, _box.size/2), _capacity);
                var dnwPos = _box.position +Vector3.One * _box.size * dnw / 2;
                _octans[4] = new OctreeNode(new Box(dnwPos, _box.size/2), _capacity);
                var dnePos = _box.position + Vector3.One *_box.size * dne / 2;
                _octans[5] =  new OctreeNode(new Box(dnePos, _box.size/2), _capacity);
                var dswPos = _box.position +Vector3.One * _box.size * dsw / 2;
                _octans[6] =  new OctreeNode(new Box(dswPos, _box.size/2), _capacity);
                var dsePos = _box.position +Vector3.One * _box.size * dse / 2;
                _octans[7] =  new OctreeNode(new Box(dsePos, _box.size/2), _capacity);
                
                
                
                _divided = true;
                
            }
            internal void QueryRange(ref ConcurrentDictionary<int,byte> ids,Box box)
            {
                 
                if(!Box.Intersects(_box,box))
                    return;



                if(!_divided)
                {
                    foreach (var p in _particles)
                    {
                   
                        ids.TryAdd(p.id,0x0);
                    }

                    return;
                }



                foreach (var octan in _octans)
                {
                    octan.QueryRange(ref ids,box);
                }
                
                    
                

                
            }


            

            
        }

        internal struct Box
        {
            internal Vector3 position;
            internal float size;

            internal Box(Vector3 position , float size )
            {
                this.size = size;
                this.position = position;
            }
            internal static bool Intersects (Box a, Box b)
            {
                return !(a.position.X - a.size > b.position.X + b.size ||
                a.position.X + a.size < b.position.X - b.size ||
                a.position.Y - a.size > b.position.Y + b.size ||
                a.position.Y + a.size < b.position.Y - b.size ||
                a.position.Z - a.size > b.position.Z + b.size ||
                a.position.Z + a.size < b.position.Z - b.size);
            }
        }

       


        

    }
    */
}