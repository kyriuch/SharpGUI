#version 430 core

layout(location = 0) out vec4 outColor;

layout(location = 0) in vec4 color;
layout(location = 1) in vec2 tex;

layout(binding = 0) uniform sampler2D uTex;

void main() {
	outColor = color * texture(uTex, tex);
}




