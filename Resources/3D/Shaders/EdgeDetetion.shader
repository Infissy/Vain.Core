shader_type spatial;
render_mode cull_front, unshaded;

uniform vec4 outline_color : hint_color;
uniform float outline_width = 1.0;

void vertex() {

	POSITION = vec4(VERTEX, 1.0);
}

void fragment() {
	COLOR = ALBEDO;
}