#version 410 core

layout(location=0) in vec3 position;
layout(location=1) in vec2 textureCoords;
layout(location=2) in float ui;

uniform mat4 MODEL_MATRIX;
uniform mat4 VIEW_MATRIX;
uniform mat4 PROJECTION;
uniform mat4 ORTHOGRAPHIC;

out vec2 v_textureCoords;

void main() {
    v_textureCoords = textureCoords;
    if (ui == 1.0f) gl_Position = ORTHOGRAPHIC * MODEL_MATRIX * vec4(position, 1.0f);
    else gl_Position = PROJECTION * VIEW_MATRIX * MODEL_MATRIX * vec4(position,1.0f);
}