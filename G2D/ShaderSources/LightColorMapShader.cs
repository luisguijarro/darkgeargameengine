namespace dge.G2D
{
    internal static partial class ShadersSources
    {
        internal static string ColorLightMapvs = @"#version 330 core
        
        layout(location = 0) in int vId;
        layout(location = 1) in vec2 vPos;
        
        uniform vec3 InColor;
        out vec3 SelectedColor;
        
        uniform vec2 v_size;
        uniform mat4 trasform;
        uniform mat4 view;
        uniform mat4 perspective;
        
        void main()
        {
            SelectedColor = InColor;
            
            gl_Position = perspective * view * trasform * vec4(vPos.x*v_size.x, vPos.y*v_size.y, 0.0, 1.0);
        }
        ";

        internal static string ColorLightMapfs = @"#version 400 core

in vec3 SelectedColor; // Color Inicial con el que trabajar.
out vec4 FragColor; //Color de Salida del Shader.
uniform float p_size; //Tamaño de cada lado del Punto.
uniform vec2 v_size; //Tamaño a Visualizar.

void main()
{
    float posXgl = gl_PointCoord.x/(1.0/p_size); //posicion en pixeles
	float posYgl = gl_PointCoord.y/(1.0/p_size); //posicion en pixeles
    if (posXgl <= 3) { posXgl = 0;}
    if (posYgl <= 2) { posYgl = 0;}
    if (posXgl >= v_size.x-2) { posXgl = v_size.x;}
    if (posYgl >= v_size.y-2) { posYgl = v_size.y;}

    float X_RelPosOnSize = (1/(v_size.x)) * posXgl; // Por1 de la posicion de pixeles en espacio pintable 
	float Y_RelPosOnSize = (1/(v_size.y)) * posYgl; // Por1 de la posicion de pixeles en espacio pintable 

    if (X_RelPosOnSize <= 1.0)
	{
		if (Y_RelPosOnSize <= 1.0)
		{
			//Pintamos
            
            vec4 blanco = vec4(1,1,1,1);
            vec4 negro = vec4(0,0,0, Y_RelPosOnSize);
            vec4 FinalColor = mix (vec4(SelectedColor.xyz, 1.0), blanco, 1.0-X_RelPosOnSize);
            FinalColor = mix(FinalColor, negro, Y_RelPosOnSize);
            
            FragColor = vec4(FinalColor.xyz, 1.0);
        }
        else
        {
            FragColor = vec4(1, 0.5, 1, 1);
        }
    }
    else
    {
        FragColor = vec4(1, 0.5, 1, 1);
    }
    
    // No pintamos
}
        ";
    }
}