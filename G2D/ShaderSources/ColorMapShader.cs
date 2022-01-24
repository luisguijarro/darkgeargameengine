// Shader empleado para el selector de color del GUI.

namespace dge.G2D
{
    internal static partial class ShadersSources
    {
        internal static string ColorMapvs = @"#version 330 core
        
        layout(location = 0) in int vId;
        layout(location = 1) in vec2 vPos;

        out vec4 VColor;
        
        
        uniform vec2 v_size;
        uniform mat4 trasform;
        uniform mat4 view;
        uniform mat4 perspective;
        
        void main()
        {
            switch (vId)
            {
                case 1: //1x 0y
                    VColor = vec4(1, 0, 0, 1);
                    break;
                case 2: //1x 0y
                    VColor = vec4(1, 1, 0, 1);
                    break;
                case 3:
                    VColor = vec4(0, 1, 0, 1);
                    break;
                case 4:
                    VColor = vec4(0, 1, 1, 1);
                    break;
                case 5: //1x, 1y
                    VColor = vec4(0, 0, 1, 1);
                    break;
                case 6: //1x, 1y
                    VColor = vec4(1, 0, 1, 1);
                    break;
                    
                default:
                    VColor = vec4(0, 0, 0, 1);
                    break;
            }
            
            gl_Position = perspective * view * trasform * vec4(vPos.x*v_size.x, vPos.y*v_size.y, 0.0, 1.0);
        }
        ";

        internal static string ColorMapfs = @"#version 330 core
        
        in vec4 VColor;
        out vec4 FragColor;

        void main()
        {            
            FragColor = vec4(VColor);
        }
        ";
    }
}