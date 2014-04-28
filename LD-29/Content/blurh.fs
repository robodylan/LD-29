uniform sampler2D texture;
uniform vec2 texelSize;

void main()
{
	vec4 color = vec4(0, 0, 0, 0);
	for(int i = -5; i < 6; i++)
		color += texture2D(texture, gl_TexCoord[0].xy + vec2(texelSize.x * i * 4, 0));

	color /= 11;
	gl_FragColor = color;
}