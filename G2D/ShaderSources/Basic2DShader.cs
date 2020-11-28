namespace dge.G2D
{
    internal static partial class ShadersSources
    {
        internal static string Basic2Dvs = @"#version 330 core
        layout (location = 0) in vec2 vPos;
        layout (location = 1) in vec2 TCoord;

        out vec2 tc;
        
        uniform vec2 v_size;
        uniform mat4 trasform;
        uniform mat4 view;
        uniform mat4 perspective;
        
        void main()
        {
            tc = TCoord;
            gl_Position = perspective * view * trasform * vec4(vPos.x*v_size.x, vPos.y*v_size.y, 0.0, 1.0);
        }
        ";

        internal static string Basic2Dfs = @"#version 330 core
        in vec2 tc;
        out vec4 FragColor;
        
        uniform bool Silhouette;
        uniform bool TexturePassed;
        uniform sampler2D s2Dtexture;
        uniform vec4 Color;
        uniform vec3 tColor;
        

        void main()
        {
            vec4 finalColor = Color;
            if (TexturePassed)
            {
                finalColor = texture(s2Dtexture, tc);
            }
            if (Silhouette)
            {
                if (finalColor == vec4(tColor.xyz, 1.0))
                {
                    finalColor = vec4(0.0, 0.0, 0.0, 0.0);
                }
                else
                {
                    finalColor = Color;
                }
            }
            else
            {
                if (finalColor == vec4(tColor.xyz, 1.0))
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
            FragColor = finalColor;
        }
        ";
    }
}