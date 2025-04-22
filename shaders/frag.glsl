#version 410 core

in vec2 v_textureCoords;

uniform sampler2D TEXTURE;
uniform vec4 TINT;

out vec4 color;

void main() {
    vec4 textureColor = texture(TEXTURE, v_textureCoords);
    color = TINT * textureColor;
}