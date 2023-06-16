using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
namespace Vain.SpellSystem.Elemental
{
    [Tool]
    partial class ElementalMeshResource : Resource
    {
        [Export]
        public ElementalParticleResource Particle {get;set;}

        [Export]
        public Mesh ConvexMesh {get;set;}

        /// Save position into array so we can generate it only once and save it to disk

        [Export(PropertyHint.Range,"0,100,1,or_greater")]
        int _sampleSize = 10;
        List<Vector3> _positions = new List<Vector3>();


        [ExportGroup("Saved Data")]
        [Export]
        Vector3[] __exportPositions;


        
        Array<Vector3> _meshNormals;
        Array<Vector3> _meshPositions;


        public void GenerateMesh()
        {   
            _positions = new List<Vector3>();
            _meshNormals = (Array<Vector3>)ConvexMesh.SurfaceGetArrays(0)[(int)Mesh.ArrayType.Normal];
            _meshPositions = (Array<Vector3>)ConvexMesh.SurfaceGetArrays(0)[(int)Mesh.ArrayType.Vertex];
        

            _sampleSize = Mathf.Min(_meshPositions.Count,_sampleSize);
           
            
            generateParticlePositions(Vector3.Zero);


             __exportPositions = new Vector3[_positions.Count];
            _positions.CopyTo(__exportPositions);



        }

        public Vector3[] GetCluster(Vector3 offset = default)
        {

            Vector3[] res = new Vector3[__exportPositions.Length];
            
            __exportPositions.CopyTo(res,0);
            res = res.Select(pos => pos + offset).ToArray();
            return res;
        }

        
    
        
        void generateParticlePositions(Vector3 position)
        {
          
            

        
            if(!isPointInsideMesh(position))
                return;
            generatePositionsVertically(position,true);
            generatePositionsVertically(position,false);


            
        }


        void generatePositionsVertically(Vector3 tempPos, bool upwards)
        {
            //Offset for spawning a new particle
            var deltaX = Vector3.Right * Particle.Size * 2;
            var deltaY = (upwards ? Vector3.Up : Vector3.Down) * Particle.Size * 2;
            var deltaZ = Vector3.Back * Particle.Size * 2;

            if(!upwards)
                //DeltaY is already inverted
                tempPos += deltaY;

            var tempYpos = tempPos;
            //Explore Y axis
            while(isPointInsideMesh(tempPos))
            {

                _positions.Add(tempPos);
                var posPreXExploration = tempPos;
                tempPos += deltaX;
                //X
                while(isPointInsideMesh(tempPos))
                {
                    _positions.Add(tempPos);


                    var posPreZExploration = tempPos;
                    tempPos += deltaZ;
                    //Z
                    while(isPointInsideMesh(tempPos))
                    {

                        _positions.Add(tempPos);
                        tempPos += deltaZ;




                    }
                    tempPos = posPreZExploration;
                    tempPos -= deltaZ;
                    //-Z
                    while(isPointInsideMesh(tempPos))
                    {
                        
                        _positions.Add(tempPos);
                        tempPos -= deltaZ;
                        
                       
                    }

                    tempPos = posPreZExploration;
                    tempPos += deltaX;
                   

                }

                tempPos = posPreXExploration;
                tempPos -= deltaX;
                //-X
                while(isPointInsideMesh(tempPos))
                {
                    _positions.Add(tempPos);


                    var tempZpos = tempPos;
                    tempPos += deltaZ;
                    //Z
                    while(isPointInsideMesh(tempPos))
                    {

                        _positions.Add(tempPos);
                        tempPos += deltaZ;




                    }
                    tempPos = tempZpos;
                    tempPos -= deltaZ;
                    //-Z
                    while(isPointInsideMesh(tempPos))
                    {
                        
                        _positions.Add(tempPos);
                        tempPos -= deltaZ;
                        
                       
                    }
                    tempPos = tempZpos;
                    tempPos -= deltaX;
                }
                

                tempPos = posPreXExploration;
                tempPos += deltaY;
            }

        }


        //Checks if a point is inside checking using vertexes normals
        //If the vertex relative from the point has the same direction as its normal we are inside, otherwise outside
        //This should be cheap but only works for convex shapes, in the future might have to look for a better solution  
        bool isPointInsideMesh(Vector3 point)
        {
            float ratio =  (float) _meshPositions.Count / _sampleSize;
            
            for (int i = 0; i < _sampleSize ; i++)
            {
                var dir = (_meshPositions[(int) (i * ratio)]-point).Normalized();


                if(dir.Dot(_meshNormals[(int)(i * ratio)]) <=  0)
                    return false;
            }

            return true;
        }

       
    }


    
    
}