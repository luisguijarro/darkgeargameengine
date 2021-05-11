namespace dge.G2D
{
    internal static partial class ShadersSources
    {
        internal static string BasicGUIvs = @"#version 330 core
        
        layout(location = 0) in int vId;
        layout(location = 1) in vec2 vPos;
        layout(location = 2) in vec2 TCoord;

        out vec2 tc;
        
        uniform vec4[2] utexcoords;
        uniform vec2 tcDisplacement;
        uniform ivec4 MarginsFromTheEdge;
        uniform vec2 v_size;
        uniform mat4 trasform;
        uniform mat4 view;
        uniform mat4 perspective;
        
        void main()
        {
            vec2 vPosT = vec2(vPos.x*v_size.x, vPos.y*v_size.y);
            vec2 vPosFin = vec2(vPos.x*v_size.x, vPos.y*v_size.y);
            
            switch(vId)
            {
                case 1:
                    vPosFin = vec2(vPosT.x, MarginsFromTheEdge.y);
                    break;
                case 2:
                    vPosFin = vec2(vPosT.x, (v_size.y-MarginsFromTheEdge.w));
                    break;
                case 4:
                    vPosFin = vec2(MarginsFromTheEdge.x, vPosT.y);
                    break;
                case 5:
                    vPosFin = vec2(MarginsFromTheEdge.x, MarginsFromTheEdge.y);
                    break;
                case 6:
                    vPosFin = vec2(MarginsFromTheEdge.x, (v_size.y-MarginsFromTheEdge.w));
                    break;
                case 7:
                    vPosFin = vec2(MarginsFromTheEdge.x, vPosT.y);
                    break;
                case 8:
                    vPosFin = vec2((v_size.x-MarginsFromTheEdge.z), vPosT.y);
                    break;
                case 9:
                    vPosFin = vec2((v_size.x-MarginsFromTheEdge.z), MarginsFromTheEdge.y);
                    break;
                case 10:
                    vPosFin = vec2((v_size.x-MarginsFromTheEdge.z), (v_size.y-MarginsFromTheEdge.w));
                    break;
                case 11:
                    vPosFin = vec2((v_size.x-MarginsFromTheEdge.z), vPosT.y);
                    break;
                case 13:
                    vPosFin = vec2(vPosT.x, MarginsFromTheEdge.y);
                    break;
                case 14:
                    vPosFin = vec2(vPosT.x, (v_size.y-MarginsFromTheEdge.w));
                    break;
                default:
                    vPosFin = vPosT;
                    break;
            }
            
            int trunc_s = int(trunc(float(vId/4)));
            int trunc_t = int(vId-(trunc(float(vId/4))*4));

            tc = vec2(utexcoords[0][trunc_s] + tcDisplacement.s, utexcoords[1][trunc_t] + tcDisplacement.t);
            
            gl_Position = perspective * view * trasform * vec4(vPosFin.x, vPosFin.y, 0.0, 1.0);
        }
        ";

        internal static string BasicGUIfs = @"#version 330 core
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
            FragColor = finalColor;
        }
        ";
    }
}