namespace dge.G2D
{
    internal static partial class ShadersSourcesGLES
    {
        internal static string BasicGUIvs = @"#version 300 es
        
        precision mediump float;
        
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
            float vPosX = vPos.x*v_size.x;
            float vPosY = vPos.y*v_size.y;
            vec2 vPosT = vec2(vPosX, vPosY);
            vec2 vPosFin = vec2(vPosX, vPosY);

            float ycomp = v_size.y - float(MarginsFromTheEdge.w);
            float xcomp = v_size.x - float(MarginsFromTheEdge.z);
            
            switch(vId)
            {
                case 1:
                    vPosFin = vec2(vPosT.x, float(MarginsFromTheEdge.y));
                    break;
                case 2:
                    vPosFin = vec2(vPosT.x, ycomp);
                    break;
                case 4:
                    vPosFin = vec2(float(MarginsFromTheEdge.x), vPosT.y);
                    break;
                case 5:
                    vPosFin = vec2(float(MarginsFromTheEdge.x), float(MarginsFromTheEdge.y));
                    break;
                case 6:
                    vPosFin = vec2(float(MarginsFromTheEdge.x), ycomp);
                    break;
                case 7:
                    vPosFin = vec2(float(MarginsFromTheEdge.x), vPosT.y);
                    break;
                case 8:
                    vPosFin = vec2(xcomp, vPosT.y);
                    break;
                case 9:
                    vPosFin = vec2(xcomp, float(MarginsFromTheEdge.y));
                    break;
                case 10:
                    vPosFin = vec2(xcomp, ycomp);
                    break;
                case 11:
                    vPosFin = vec2(xcomp, vPosT.y);
                    break;
                case 13:
                    vPosFin = vec2(vPosT.x, float(MarginsFromTheEdge.y));
                    break;
                case 14:
                    vPosFin = vec2(vPosT.x, ycomp);
                    break;
                default:
                    vPosFin = vPosT;
                    break;
            }
            
            float f_trunc_s = trunc(float(vId) / 4.0);
            float f_trunc_t = f_trunc_s * 4.0;
            int trunc_s = int(f_trunc_s);
            int trunc_t = vId - int(f_trunc_t);

            float tcX = utexcoords[0][trunc_s] + tcDisplacement.s;
            float tcY = utexcoords[1][trunc_t] + tcDisplacement.t;
            tc = vec2(tcX, tcY);
            
            gl_Position = perspective * view * trasform * vec4(vPosFin.x, vPosFin.y, 0.0, 1.0);
        }
        ";

        internal static string BasicGUIfs = @"#version 300 es
        
        precision mediump float;

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