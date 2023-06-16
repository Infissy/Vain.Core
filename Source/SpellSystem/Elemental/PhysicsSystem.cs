using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using System.Collections.Generic;
using Vain.Log;
namespace Vain.SpellSystem.Elemental
{
    internal partial class PhysicsSystem : Resource
    {

        [Export]
        public ElementalParticleBuffer Buffer{get;set;}
        

        //TODO: Implement it using box mesh of any sort so it can easily modified
        [Export]
        public Vector3 BoundsPosition {get;set;}
        [Export] 
        public Vector3 BoundsSize {get;set;}
        
        Vector3I _cellCount;

        [Export]
        public float CellSize {get;set;} = 1;
        

        ILookup<int, int> _grid;
        LinearOctree _octTree;
        
        bool _running = true;

        Task _mainTask;


        public double Delta{get;set;}
        public bool Running
        {
            get 
            {   
           
                return _running;
            }

            set
            {
                
                _running = value;

                if(value)
                {
                    if(_mainTask == null || _mainTask.Status != TaskStatus.Running)
                    {
                        _mainTask = new Task(runningLoop);
                        _mainTask.Start();

                    }
                    
                }
                else
                {
                    if(_mainTask == null || _mainTask.Status != TaskStatus.RanToCompletion)
                        _mainTask.Wait();

                }
            }
        }

        Stopwatch _st = new Stopwatch();


        

       
        void runningLoop()
        {
            while(_running)
            {
                Tick(Delta);
            }
        }
      
        public void Tick(double delta)
        {
            _st.Restart();
            if(Buffer == null)
                throw new Exception("PhysicsSystem is missing the buffer resource, it needs a ElementalParticleBuffer resource in order to work properly");
            var temp = 0f;
            generateOctree();
            temp = _st.ElapsedMilliseconds;
            Logger.Runtime("    Grid Generation: ", temp + "ms");
            checkOctatreeInteractions(delta);
            
            Logger.Runtime("    Interaction step: ", _st.ElapsedMilliseconds - temp + "ms");
            temp = _st.ElapsedMilliseconds;
            applyVelocity(delta);
            Logger.Runtime("    Velocity: ", _st.ElapsedMilliseconds - temp + "ms");

            temp = _st.ElapsedMilliseconds;
            Buffer.Update();

            Logger.Runtime("Physics Time: ", _st.ElapsedMilliseconds + "ms");
            
        }
    
        void generateOctree()
        {
            float size = Mathf.Max(Mathf.Max(BoundsSize.X,BoundsSize.Y),BoundsSize.Z);
            _octTree = new LinearOctree(BoundsPosition,size,150,255);
      
            var buffer = Buffer.CurrentBuffer.Slice(0,(int)Buffer.ParticleCount).Span;
            for (int i = 0; i < Buffer.ParticleCount; i++)
            {
                _octTree.Insert(buffer[i]);
            }
              
            _octTree.Build();
            
        }

        void generateGrid()
        {
            //KD-Tree / BSP Tree
            

            var upperLeft = BoundsPosition - BoundsSize/2;
            _cellCount = (Vector3I) (BoundsSize / CellSize);
            _grid = Buffer.CurrentBuffer.Slice(0,(int)Buffer.ParticleCount).ToArray().AsParallel()
                    .Where((p) => isInsideBounds(p.Position))
                    .ToLookup(
                        (p) => 
                        {
                            var deltaPos = p.Position - upperLeft ;
                            var cellCoordinate = (Vector3I) (deltaPos / CellSize);

                            var linearCoordinate = vec3ToLinearCoordinate(cellCoordinate,_cellCount);   
                            return linearCoordinate;
                        },
                        (p) => p.ID
                    
                    );
         
        }
        void checkOctatreeInteractions(double delta)
        {      
            
            Span<int> queryParticles = stackalloc int[500];

            var current = Buffer.CurrentBuffer.Span;
            var past = Buffer.PastBuffer.Span;
            
            var dummy = new Span<int>();

            for( int i = 0;  i < Buffer.ParticleCount; i++)
            {
                
          
    
            
             
                int particleCount = _octTree.Query(current[i].Position,current[i].Radius * 1.2f, ref queryParticles);
                
                if(particleCount > queryParticles.Length)
                {
                    queryParticles = stackalloc int [particleCount];
                    _octTree.Query(current[i].Position,current[i].Radius * 1.2f, ref queryParticles);
                }
                
                 for (int o = 0; o < particleCount; o++)
                {
                    
                    
                    current[i] = ElementalParticle.ParticleToParticleInteraction(
                        past[i],
                        past[queryParticles[o]],
                    
                        delta
                    );
                }
                
            
                
            }
        }
        void checkGridInteractions(double delta)
        {
            var cellCount = (Vector3I) (BoundsSize / CellSize);
            Parallel.ForEach(_grid, (cell) => 
            {
                
                for (int x = -1; x < 1; x++)
                {
                      for (int y = -1; y < 1; y++)
                    {
                        for (int z = -1; z < 1; z++)
                        {

                            var otherCell = linearToVec3Coordinate(cell.Key,_cellCount);
                            otherCell.X += x;
                            otherCell.Y += y;
                            otherCell.Z += z; 
                            var cellCoordinate = vec3ToLinearCoordinate(otherCell,_cellCount);
                            if(_grid[cellCoordinate].Count() > 0 )
                            {
                                CellToCellInteractions(cell.Key,cellCoordinate,delta);
                            }
                        }   
                    }
                }
            });
        }

        void CellToCellInteractions(int cell,int other, double delta)
        {

            var past = Buffer.PastBuffer.Span;
            var current = Buffer.CurrentBuffer.Span;
            foreach (var particleID in _grid[cell])
            {
                foreach (var otherID in _grid[other])
                {

                    //FIXME: Indexing, connected to the overhaul buffer access needs 
                    current[(int)particleID] = ElementalParticle.ParticleToParticleInteraction(
                            past[(int)particleID],
                            past[(int)otherID],
                       
                            delta
                        );
                }
            }

        
        }

       
        
       



      
    
        void applyVelocity(double delta)
        {
        
            Parallel.For(0,Buffer.ParticleCount, (i) =>
                {
                    var current = Buffer.CurrentBuffer.Span;
                    ElementalParticle particle =  current[i];
                    particle.Position = current[i].Position + current[i].Velocity * (float)delta;
                    current[i] = particle;
                }
            
            );
         
        }

        void applyCostrains()
        {
            
        }



        bool isInsideBounds(Vector3 position)
        {
            if(position.X >= BoundsPosition.X + BoundsSize.X || position.X <= BoundsPosition.X - BoundsSize.X)
                return false;
            if(position.Y >= BoundsPosition.Y + BoundsSize.Y || position.Y <= BoundsPosition.Y - BoundsSize.Y)
                return false;
            if(position.Z >= BoundsPosition.Z + BoundsSize.Z || position.Z <= BoundsPosition.Z - BoundsSize.Z)
                return false;
            return true;
        }


        static Vector3I linearToVec3Coordinate(int linear, Vector3I cellCount)
        {
            Vector3I res;
            res.X =  linear % cellCount.X;
            res.Y =  (linear / cellCount.X)  % cellCount.Y;
            res.Z =  linear / (cellCount.X * cellCount.Y);
            return res;
        }
        int vec3ToLinearCoordinate(Vector3I coordinate,  Vector3I cellCount)
        {
            return coordinate.X +  cellCount.X  * (coordinate.Y + coordinate.Z * cellCount.Y);

           
        }
    }
}