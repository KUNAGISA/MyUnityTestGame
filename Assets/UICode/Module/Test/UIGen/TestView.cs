
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace AutoGenerateUI.Test {
    /// <summary>
    /// 程序生成的UI代码类，请勿修改
    /// </summary>
    public sealed partial class TestUI  {
        public TestUI() {

        }
        public TestUI(Transform inst) {
            Init(inst);
        }
        
        public RectTransform Panel  { get; private set; }
        public ButtonCell TestBtn  { get; private set; }
        public RawImage TestRawImg  { get; private set; }

        public void Init(Transform inst) {
            
            Transform PanelXform = inst.Find("Panel");
            if ( PanelXform != null) {
                Panel = PanelXform as RectTransform;
            } else {
                Debug.LogError("Panel Can't Find Under inst");
            }
            Transform TestBtnXform = PanelXform.Find("TestBtn");
            if ( TestBtnXform != null) {
                TestBtn = new ButtonCell(TestBtnXform);
            } else {
                Debug.LogError("TestBtn Can't Find Under PanelXform");
            }
            Transform TestRawImgXform = PanelXform.Find("TestRawImg");
            if ( TestRawImgXform != null) {
                TestRawImg = TestRawImgXform.GetComponent<RawImage>();
            } else {
                Debug.LogError("TestRawImg Can't Find Under PanelXform");
            }
        }

        public void Free() {
            
            Panel = null;
            if (TestBtn != null) {
                TestBtn.Free();
            }
            TestBtn = null;
            TestRawImg = null;
        }
    }
}
