uniform sampler2D texture;
uniform sampler2D glow;
uniform vec2 texelSize;

void main()
{
	vec4 glowColor = texture2D(glow, vec2(gl_TexCoord[0].x, 1 - gl_TexCoord[0].y));
	glowColor = vec4(glowColor.rgb, 1);
	gl_FragColor = texture2D(texture, vec2(gl_TexCoord[0].x, 1 - gl_TexCoord[0].y)) * glowColor;
}