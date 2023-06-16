#[compute]
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
}


layout (binding = 0,  std430) restrict readonly buffer ParticleBuffer
{
    uint number_of_particles;    
    int[] particles;
}
particle_buffer;






void main() {
  
 
}
