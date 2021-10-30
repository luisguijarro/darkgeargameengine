namespace dge.G2D
{
    internal static partial class ShadersSources
    {
        internal static string ColorLightMapvs = @"#version 330 core
        
        layout(location = 0) in int vId;
        layout(location = 1) in vec2 vPos;

        out vec3 VColor;
        out vec3 SelectedColor;
        
        uniform vec3 InColor;
        
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

        internal static string ColorLightMapfs = @"#version 330 core
        
        in vec3 VColor;
        in vec3 SelectedColor;
        out vec4 FragColor;

        void main()
        {
            float position = gl_PointCoord.y;
            vec4 blanco = vec4(1,1,1,1);
            vec4 negro = vec4(0,0,0, gl_PointCoord.y);

            vec4 FinalColor = mix (vec4(SelectedColor.xyz, 1), blanco, 1-gl_PointCoord.x);
            FinalColor = mix(FinalColor, negro, gl_PointCoord.y);
           
            FragColor = vec4(FinalColor.xyz, 1);
        }
        ";
    }
}