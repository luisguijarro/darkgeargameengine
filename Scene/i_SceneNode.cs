using System;
using System.Collections.Generic;

namespace dge
{
    public interface I_SceneNode
    {
        internal virtual void InternalDraw() {}
        protected virtual void Draw() {}// Dibuja la escena.
        string Name {get; set;}
    }
}