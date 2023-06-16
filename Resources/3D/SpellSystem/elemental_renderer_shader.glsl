//#[compute]
#version 450

const float M_PI = 3.1415926535897932384626433832795;
// Invocations in the (x, y, z) dimension
//Maybe create kernel so it each invocation can process nearby cells
layout(local_size_x = 64) in;

struct particlePhysics
{
    vec4 position;

};

struct particleRendering
{
    vec4 color;
};


layout (binding = 1,  std430) restrict readonly buffer ParticlePhysicsBuffer
{
    particlePhysics[] particles;
}
particle_physics_buffer;


layout (binding = 2,  std430) restrict readonly buffer ParticleRenderingBuffer
{
    particleRendering[] particles;
}
particle_rendering_buffer;


layout (binding = 3,  std430) buffer Camera
{
    
    
    layout(row_major) mat4x4 transform;
    float fov;

}
camera;





layout (binding = 0,  std430) restrict readonly buffer ParticleBuffer
{
    uint number_of_particles;  
    int[] particles;
}
particle_buffer;




layout( rgba32f , binding = 4) uniform writeonly image2D u_densityTexture;

const int n_iter = 50;
shared uint nearestParticle;


vec3 calculateRayDirection(vec2 uv, mat3x3 cameraTransform, float fov ) {
    // Step 1: Calculate the camera's right vector
    // Convert FOV from degrees to radians
    float fovRad = radians(fov+30);

  


    vec2 size = vec2(imageSize(u_densityTexture));

    float relativeX =  gl_WorkGroupID.x * 2 / size.x - 1; // [0, screenWidth] -> [-1, 1]
    float relativeY = -(gl_WorkGroupID.y * 2 / size.y - 1); // [0, screenHeight] -> [-1, 1], might need to be flipped depending on the coordinate system
    relativeX *= size.x/size.y;
    vec3 viewVector = vec3(relativeX * sin(fovRad / 2),
                       relativeY * sin(fovRad/ 2),
                       -1);

    vec3 rayDirWorld =  viewVector * transpose(cameraTransform);


    return normalize(rayDirWorld);
}

/*

bool rayMarching(vec3 ray)
{
    vec3 deltapos = camera.transform[3].xyz;
    bool found = false;

    for(int step = 0; step < n_iter && !found; step++)
    {
        float dist_nearest = 10000;
        
       
        for(int i = 0; i < max; i++)
        {
        
            float dist = distance(particle_buffer.particles[i].position.xyz, deltapos) - particle_buffer.particles[i].position.w;
            
            if(dist < dist_nearest)
            {
        
                dist_nearest = dist;
               
            }

            
        
        }
        
        if(dist_nearest < 0.01)
        {
            found = true;

        }
        deltapos += ray * dist_nearest ;


    }

    return found;

}
*/

bool intersectSphere(vec3 startPosition,vec3 direction, vec3 spherePosition, float size)
{
    //Weirdly written for performance improvements? not seeing much though
    

    vec3 oc = startPosition - spherePosition;
    float a = dot(direction, direction);
    float b = 2.0 * dot(direction,oc);
    float c = dot(oc, oc) -   size * size;
    float discriminant = b * b - 4.0 * a * c;


    //If discriminant is negative we return a false value anyway, we don't actually need the proper value of T when the discriminant is negative so we can change the value to avoid errors
    float t = (- b - sqrt(max(0,discriminant))) / (2.0 * a);




    return discriminant >= 0.0 && t >= 0.0;

}

vec4 rayTracing(vec3 ray)
{

    


    vec3 cameraPos =  camera.transform[3].xyz;



    

      
      
    uint max = particle_buffer.number_of_particles / 64 + uint(max( 0 , sign(particle_buffer.number_of_particles % 64 - gl_LocalInvocationIndex )));
     

    int i = 0;
  



    for(i = 0; i < max; i++)
    {
       
        uint globalIndex = i * 64 + gl_LocalInvocationIndex;
        vec4 p = particle_physics_buffer.particles[particle_buffer.particles[globalIndex]].position;
        bool res = intersectSphere(
            cameraPos,
            ray,
            p.xyz,
            p.w
        );


        if(nearestParticle < globalIndex)
            break;
            
        
        if(res)
        {
            //memoryBarrierShared();
            atomicMin(nearestParticle,globalIndex);
            break;

        }
        

            
        
    }
    

  

    return vec4( particle_rendering_buffer.particles[nearestParticle].color.rgb, 1 - nearestParticle / particle_buffer.number_of_particles);
   
        
}
   


   
    //int globalIndex = i * 1024 + int(gl_LocalInvocationIndex);
   // return vec3(nearestDistance);


void main() {
  
    
    atomicExchange(nearestParticle, particle_buffer.number_of_particles);
     
    
    barrier();
    
    vec2 size = vec2(imageSize(u_densityTexture));
    vec2 uv = gl_WorkGroupID.xy / size.xy ;
    

    
    uv.y = 1-uv.y;
    uv.x *= size.x/size.y;
    

    
    //vec3 ray = getCameraRayDir(uv,camera.transform,camera.fov);
    vec3 ray = calculateRayDirection(uv,mat3(camera.transform), camera.fov) ;
    
    //vec3 ray = normalize( vec3( (gl_WorkGroupID.xy-size.xy*.5)/size.x, 1.0 ) );
    
    vec4 color = rayTracing(ray);
    

   
   
    imageStore(u_densityTexture,ivec2(gl_WorkGroupID.x,gl_WorkGroupID.y),color);
 
    



  

    //imageStore(u_densityTexture,ivec2(gl_WorkGroupID.xy),particle_buffer.particles[3].position.w == 1  ? vec4(1,1,1,1) : vec4(0,0,0,1));
    
    //imageStore(u_densityTexture, ivec2(gl_WorkGroupID.xy) ,vec4(particle_buffer.particles[1].position.w == 1 ? 1.0f : 0.0f, 0 ,0,1));
    //imageStore(u_densityTexture, ivec2(gl_WorkGroupID.x,gl_WorkGroupID.y) ,vec4( uv,0,1));
 
    //imageStore(u_densityTexture,ivec2(gl_WorkGroupID.x,gl_WorkGroupID.y),vec4( gl_LocalInvocationIndex / 1024.0,0,0,1));
}
