using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using System.Runtime.InteropServices;

namespace Vain.SpellSystem.Elemental
{
    class LinearOctree
    {
      
        public struct Particle
        {
            internal Box box;
      
            internal int ID;
            internal Particle(Vector3 position, float size, int ID)
            {
                this.box = new Box(position,size);
         
                this.ID = ID;
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



        /// <summary>
        /// An octan position is determined by its axis value
        /// Y is Up / Down
        /// Z is North / South
        /// X is East / West
        /// 
        /// The octree at the moment is cubic
        /// </summary>
        struct Octan
        {
            internal Box box;

            internal int childCount;
            internal int parent;

            internal int particleStartIndex;
            internal int particleCount;
            internal bool subdivided;
            internal int level;



       

            internal static int GetOctanLinearIndex(Vector3I position) 
            {
                return position switch
                {
                    {X: >=0, Y: >=0, Z: >=0} => 0,                //UNW
                    {X: -1, Y: >=0, Z: >=0} => 1,               //UNE
                    {X: >=0, Y: >=0, Z: -1} => 2,               //USW
                    {X: -1, Y: >=0, Z: -1} => 3,              //USE
                    {X: >=0, Y: -1, Z: >=0} => 4,               //DNW
                    {X: -1, Y: -1, Z: >=0} => 5,              //DNE
                    {X: >=0, Y: -1, Z: -1} => 6,              //DSW
                    {X: -1, Y: -1, Z: -1} => 7,             //DSE
                    _ => -1,
                };

            }


            internal static Vector3I GetOctanRelativePosition(int index)
            {
                return index switch
                {
                    0 => new Vector3I(1,1,1),
                    1 => new Vector3I(-1,1,1),
                    2 => new Vector3I(1,1,-1),
                    3 => new Vector3I(-1,1,-1),
                    4 => new Vector3I(1,-1,1),
                    5 => new Vector3I(-1,-1,1),
                    6 => new Vector3I(1,-1,-1),
                    7 => new Vector3I(-1,-1,-1),
                    _ => Vector3I.Zero


                };
                
            }
        }

        Particle[] _main;
        Octan[] _octans;




        List<Octan> _listOctans;
        List<Particle> _listParticles;
       


        Box _box;
        int _nodeCapacity;
        ///Added to avoid infinite loops in case there is an error in the generation or calculations and a number > nodecapacity of particle is in the same point, making it subdivide to infinity
        int _maxLevel;

        public LinearOctree(Vector3 treePosition, float treeSize, int nodeCapacity, int maxLevel)
        {
            
            _box = new Box(treePosition, treeSize);
            _listOctans = new List<Octan>();

            _listParticles = new List<Particle>(100000);
            

            _nodeCapacity = nodeCapacity;
            _maxLevel = maxLevel;
            
            Octan root = new Octan();
            root.parent = -1;
            root.box = new Box(treePosition,treeSize);

            _listOctans.Add(root);

        }
        public void Build()
        {
            _octans = _listOctans.ToArray();




            _main = _listParticles.ToArray();
        }
        public int Query(Vector3 position, float size, ref Span<int> particles)
        {
            
            Box particleBox = new Box(position,size);
  
            bool exploring = true;
            int octanIndex = 0;

            
            int particleQueryCount  = 0;
            while (exploring)
            {
                if(octanIndex >= _octans.Length)
                    return particleQueryCount;



                
                var currentOctan = _octans[ octanIndex];

                if(!Box.Intersects(particleBox,currentOctan.box))
                {
                    octanIndex = octanIndex + currentOctan.childCount + 1; 
                    continue;
                }

                
                if(currentOctan.subdivided)
                {
                    octanIndex += 1;
                    continue;
                }
                
                
                octanIndex += 1;
             

               
            
                for (int i = currentOctan.particleStartIndex; i <  currentOctan.particleStartIndex + currentOctan.particleCount; i++)
                {
                    if(Box.Intersects(_main[i].box,particleBox))
                    {
                        if(particles.Length > particleQueryCount)
                            particles[particleQueryCount] = _main[i].ID;
                        particleQueryCount += 1;
                    }
                }

                

            }

            return particleQueryCount;

          

        
            ///if y + index +
            ///if x + 

        }
     
        public void Insert(ElementalParticle particle)
        {
            var octanIndex = 0;
            var particleBox = new Box(particle.Position,particle.Radius);


            var exploring = true;


            while (exploring)
            {
                //Particle out of bounds
                if(octanIndex >= _listOctans.Count)
                    return;
                var currentOctan = _listOctans[ octanIndex];


                if(!Box.Intersects(particleBox,currentOctan.box))
                {
                    octanIndex = octanIndex + currentOctan.childCount + 1; 
                    continue;
                }
                
                

                
                if(currentOctan.subdivided)
                {
                    octanIndex += 1;
                    continue;
                }
                
                            
                if(currentOctan.particleCount == _nodeCapacity)
                {
                    //Cannot add a particle when it's over level
                    if(currentOctan.level == _maxLevel)
                        return;
                    subdivide(currentOctan,octanIndex);
                    octanIndex += 1;
                    continue;

                }
                
                
                
                
                insertParticle(octanIndex,particle);
                ///When is more we subdivide
                return;


               
            }

        }
        void subdivide(Octan parent, int parentIndex)
        {
            var indices = _listParticles.GetRange(parent.particleStartIndex,parent.particleCount);
            Span<Particle> orderedParticles = stackalloc Particle[indices.Count * 8];
            Span<int> particleCountPerOctan = stackalloc int[8];


            //Reorder per octans
            for (int i = 0; i < indices.Count; i++)
            {
                var relativePos = _listParticles[i].box.position - parent.box.position;
                var normalized = Vector3I.Zero;

                normalized.X = Mathf.Sign(relativePos.X);
                normalized.Y = Mathf.Sign(relativePos.Y);      
                normalized.Z = Mathf.Sign(relativePos.Z);
                                
                var octanChildIndex = Octan.GetOctanLinearIndex(normalized);

                orderedParticles[ parent.particleCount * octanChildIndex + particleCountPerOctan[octanChildIndex]] = _listParticles[i];
                particleCountPerOctan[octanChildIndex] += 1;
            }
            


            ///Generates the actual octans and assings the ordered array to the main list
          
            var currentIndex = parent.particleStartIndex;
            for(int o = 0; o < 8; o++)
            {
                Octan octan = new Octan();
                octan.box = new Box(parent.box.position + Vector3.One * parent.box.size  / 2.0f * Octan.GetOctanRelativePosition(o),parent.box.size / 2f);
                
                octan.level = parent.level + 1;
                octan.childCount = 0;
                octan.parent = parentIndex;
               
                octan.particleStartIndex =  currentIndex;
                octan.particleCount =  particleCountPerOctan[o];
                
                
                var octanIndex = parentIndex + o + 1;
                if(_listOctans.Count <= octanIndex)
                    _listOctans.Add(octan);
                else
                    _listOctans.Insert(octanIndex, octan);


                for (int i = 0; i < particleCountPerOctan[o]; i++)
                {
                    _listParticles[currentIndex + i] = orderedParticles[o * indices.Count + i ];
                    
                }

                currentIndex += particleCountPerOctan[o];
                
            }


            
            var octans = CollectionsMarshal.AsSpan(_listOctans);
            for (int i = parentIndex + parent.childCount; i <  octans.Length; i++)
            {
                
                if(octans[i].parent > parentIndex)
                {
                   var followingOctan = octans[i];
                   followingOctan.parent += 8;
                   octans[i] = followingOctan;
                }
            }

        
            
         


            parent.subdivided = true;
            parent.childCount += 8;
            octans[parentIndex] = parent;

       
           parentIndex = parent.parent;

           

            while (parentIndex != -1)
            {

               
                var grandParent = octans[parentIndex]; 

                grandParent.childCount += 8;

                octans[parentIndex] = grandParent;

            
               
                
                parentIndex = octans[parentIndex].parent;
            }

        
       

        }
        /// <summary>
        /// Since there has been an insertion all the references have to be updated
        /// </summary>
     
        void insertParticle(int octanIndex, ElementalParticle particle)
        {
            var octans = CollectionsMarshal.AsSpan(_listOctans);


            var newParticle = new Particle(particle.Position,particle.Radius,particle.ID);
            var index = octans[octanIndex].particleStartIndex + octans[octanIndex].particleCount + 1;
            if(_listParticles.Count <= index)
                _listParticles.Add(newParticle); 
            else
                _listParticles.Insert(index,newParticle);


            //Translates all octans's range next to the current by one, since we are appending a particle behind all the particles of those octans
            for(int i = octanIndex  + octans[octanIndex].childCount + 1; i < octans.Length; i++)
        
            {
                var followingOctan = octans[i];
                followingOctan.particleStartIndex += 1;
                octans[i] = followingOctan;

            }
           
            
           

            
            var octan = octans[octanIndex];
            octan.particleCount += 1 ;
            octans[octanIndex] = octan;
            



         


            
            var parentIndex = octan.parent;
           

           

            while (parentIndex != -1)
            {
                octan =  octans[parentIndex];
                octan.particleCount += 1;
                
                octans[parentIndex] = octan;

                parentIndex = octans[parentIndex].parent;
            }

        
                
            

            

        

            

            

        }



      
        
       


        

    }
}