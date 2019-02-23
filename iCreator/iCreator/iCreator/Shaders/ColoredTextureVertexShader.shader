#version 430 core

layout(location = 0) in vec2 pos;
layout(location = 1) in vec4 color;
layout(location = 2) in vec2 tex;

layout(location = 0) out vec4 outColor;
layout(location = 1) out vec2 outTex;

void main() {
	gl_Position = vec4(pos.xy, 0.0, 1.0);

	outColor = color;
	outTex = tex;
}





