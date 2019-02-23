#version 430 core

layout(location = 0) out vec4 outColor;

layout(location = 0) in vec2 tex;

layout(binding = 0) uniform sampler2D uTex;

void main() {
	outColor = texture(uTex, tex);
}