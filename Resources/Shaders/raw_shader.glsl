precision mediump float;
uniform float u_radius;
uniform vec3 u_color;

in vec2 SCREEN_UV;

void main() {
    vec2 uv = SCREEN_UV;

    
    st = st - 0.5;
    st.x = st.x + 0.1 * sin(u_time); 
    float mask = length(st) < u_radius ? 1.0 : 0.;
    gl_FragColor = vec4(u_color, mask);
}