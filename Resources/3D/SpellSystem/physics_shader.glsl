#[compute]
#version 450

const float M_PI = 3.1415926535897932384626433832795;
// Invocations in the (x, y, z) dimension
//Maybe create kernel so it each invocation can process nearby cells
layout(local_size_x = 64) in;

struct particle 
{
    vec4 position;
    vec4 color;
};

layout (binding = 1,  std430) restrict readonly buffer ParticleBuffer
{
    uint number_of_particles;    
    particle[20000] particles;
}
particle_buffer;




   
    //int globalIndex = i * 1024 + int(gl_LocalInvocationIndex);
   // return vec3(nearestDistance);


void main() {
  
 
}
