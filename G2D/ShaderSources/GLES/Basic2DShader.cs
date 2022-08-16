namespace dge.G2D
{
    internal static partial class ShadersSourcesGLES
    {
        internal static string Basic2Dvs = @"#version 300 es

        precision mediump float;
        
        layout(location = 0) in int vId;
        layout(location = 1) in vec2 vPos;
        layout(location = 2) in vec2 TCoord;

        out vec2 tc;
        
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
                    tc = TCoord.st;
                    break;
            }
            
            gl_Position = perspective * view * trasform * vec4(vPos.x*v_size.x, vPos.y*v_size.y, 0.0, 1.0);
        }
        ";

        internal static string Basic2Dfs = @"#version 300 es
        precision mediump float;

        in vec2 tc;
        out vec4 FragColor;
        
        uniform bool Silhouette;
        uniform bool TexturePassed;
        uniform sampler2D s2Dtexture;
        uniform vec4 Color;
        uniform vec3 tColor;

        uniform vec4 GlobalLightColor;
        

        void main()
        {
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
            FragColor = finalColor * GlobalLightColor;
        }
        ";
    }
}