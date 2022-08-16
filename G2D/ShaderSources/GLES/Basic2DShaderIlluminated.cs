namespace dge.G2D
{
    internal static partial class ShadersSourcesGLES
    {
        internal static string Basic2DIlluminatedvs = @"#version 300 es
        
        precision mediump float;
        
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


        internal static string Basic2DIlluminatedfs = @"#version 300 es
        
        precision mediump float;

        in vec2 tc;
        out vec4 FragColor;

        in vec3 FragPos;
        
        uniform bool Silhouette;
        uniform bool TexturePassed;
        uniform sampler2D s2Dtexture;
        uniform vec4 Color;
        uniform vec3 tColor;

        uniform int n_luces;

        uniform vec4 GlobalLightColor;
        uniform vec4 lightsPosRange[200];
        uniform vec4 lightsColor[200];
        uniform vec4 lightRotationAngle[200];
        
        

        void main()
        {
            vec3 ColorLights = GlobalLightColor.xyz; //vec3(1,1,1);
            for (int i=0;i<n_luces;i++)
            {
                if (lightsPosRange[i].z <= FragPos.z)
                {
                    float rango = lightsPosRange[i].w;
                    float Distancia = distance(lightsPosRange[i].xyz,FragPos.xyz);
                    float LightIntensity = max(min(1.0-((1.0/rango)*Distancia), 1.0), 0.0);
                    //ColorLights *= (lightsColor[i].xyz * (LightIntensity));
                    ColorLights = mix (ColorLights, lightsColor[i].xyz, LightIntensity);
                }
            }
            //ColorLights /= lightsPosRange.length();

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
            FragColor = finalColor * vec4(ColorLights, 1);
        }
        ";
    }
}