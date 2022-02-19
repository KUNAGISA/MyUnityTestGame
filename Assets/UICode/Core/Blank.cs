using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Serialization;

namespace UnityEngine.UI {
    /// <summary>
    /// Blank is a empty textured element in the UI hierarchy.
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    [AddComponentMenu("UI/Blank", 11)]
    public class Blank : MaskableGraphic {
        protected Blank() {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill) {
            toFill.Clear();
        }

//         private Texture m_Texture;
//         public override Texture mainTexture {
//             get {
//                 if (m_Texture != null) {
//                     return m_Texture;
//                 }
//                 return s_WhiteTexture;
//             }
//         }
// 
//         protected override void Start() {
//             base.Start();
// #if UNITY_EDITOR
//             if (Application.isPlaying) {
//                 CheckDynamicAtlas();
//             }
// #else
//              CheckDynamicAtlas();
// #endif
//         }
// 
//         private void CheckDynamicAtlas() {
//             if (m_AtlasType == eDynamicAtlas.None) {
//                 return;
//             }
// 
//             var altas = DynamicAtlas.Get(m_AtlasType);
//             m_Texture = altas.GetTexture(0);
//         }
    }
}
