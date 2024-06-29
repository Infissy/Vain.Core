shader_type canvas_item;
uniform float RADIUS;
uniform vec3 SPELL_COLOR;
uniform float SWAY;


void fragment() {
    vec2 uv = UV;

    
    uv = uv - 0.5;
    uv.x = uv.x + 0.2 * SWAY * sin(TIME); 
    float mask = length(uv) < (RADIUS * 0.45) ? 1.0 : 0.;
    COLOR = vec4(SPELL_COLOR, mask);
}