namespace dge.G2D
{
    internal static partial class ShadersSources
    {
        internal static string BasicWritervs = @"#version 330 core
        
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
                    tc = TCoord;
                    break;
            }
            
            gl_Position = perspective * view * trasform * vec4(vPos.x*v_size.x, vPos.y*v_size.y, 0.0, 1.0);
        }
        ";

        internal static string BasicWriterfs = @"#version 330 core
        in vec2 tc;
        out vec4 FragColor;
        
        uniform sampler2D s2Dtexture;
        uniform vec4 Color;
        

        void main()
        {
            vec4 finalColor = texture(s2Dtexture, tc);
            
            finalColor = vec4(Color.xyz, finalColor.w);
            
            FragColor = finalColor;
        }
        ";
    }
}