namespace dge.G2D
{
    internal static partial class ShadersSources
    {
        internal static string Basic2DIlluminatedvs = @"#version 330 core
        
        layout(location = 0) in int vId;
        layout(location = 1) in vec2 vPos;
        layout(location = 2) in vec2 TCoord;

        out vec2 tc;
        out vec3 FragPos;
        
        uniform vec4 utexcoords;
        uniform vec2 v_size;
        uniform mat4 trasform;
        uniform mat4 view;
        uniform mat4 perspective;
        
        void main()
        {
            switch (vId)
            {
                case 1:
                    tc = vec2(utexcoords.x, utexcoords.y); //0x 0y
                    break;
                case 2:
                    tc = vec2(utexcoords.x, utexcoords.w); //0x 1y
                    break;
                case 3:
                    tc = vec2(utexcoords.z, utexcoords.w); //1x 1y
                    break;
                case 4:
                    tc = vec2(utexcoords.z, utexcoords.y); //1x 0y
                    break;
                default:
                    tc = TCoord;
                    break;
            }

            FragPos = vec3(trasform * vec4(vPos.x*v_size.x, vPos.y*v_size.y, 0.0, 1.0));
            
            gl_Position = perspective * view * trasform * vec4(vPos.x*v_size.x, vPos.y*v_size.y, 0.0, 1.0);
        }
        ";


        internal static string Basic2DIlluminatedfs = @"#version 330 core
        in vec2 tc;
        out vec4 FragColor;

        in vec3 FragPos;
        
        uniform bool Silhouette;
        uniform bool TexturePassed;
        uniform sampler2D s2Dtexture;
        uniform vec4 Color;
        uniform vec3 tColor;

        uniform vec4 GlobalLightColor;
        uniform vec4[] lightsPosRange;
        uniform vec4[] lightsColor;
        uniform vec4[] lightRotationAngle;
        
        

        void main()
        {
            vec4 ColorLights = vec4(0,0,0,0);
            for (int i=0;i<lightsPosRange.length();i++)
            {
                if (lightsPosRange[i].z > FragPos.z)
                {
                    float LightIntensity = 0.0; // Iniciamos
                    vec2 DistanceVec = FragPos.xy - lightsPosRange[i].xy;
                    float Distance = sqrt(dot(DistanceVec,DistanceVec));

                    //Calcular por cuanto hay que multiplicar la incidencia de luz según la distancia.
                    if (Distance <= lightsPosRange[i].w)
                    {
                        LightIntensity = (1.0/lightsPosRange[i].w) * (lightsPosRange[i].w-Distance);
                    }
                    ColorLights += (lightsColor[i] * LightIntensity);
                }
            }
            ColorLights /= lightsPosRange.length();

            vec4 finalColor = Color;
            if (TexturePassed)
            {
                finalColor = texture(s2Dtexture, tc);
            }
            if (Silhouette)
            {
                if (finalColor.xyz == tColor.xyz)
                {
                    finalColor = vec4(0.0, 0.0, 0.0, 0.0);
                }
                else
                {
                    if (finalColor.w != 0.0)
                    {
                        finalColor = vec4(Color.xyz, 1.0);
                    }
                    else
                    {
                        finalColor = vec4(0.0, 0.0, 0.0, 0.0);
                    }
                }
            }
            else
            {
                if (finalColor.xyz == tColor.xyz)
                {
                    finalColor = vec4(0.0, 0.0, 0.0, 0.0);
                }
                else
                {
                    if (TexturePassed)
                    {
                        finalColor = finalColor * Color;
                    }
                }
            }
            FragColor = finalColor * GlobalLightColor * ColorLights;
        }
        ";
    }
}