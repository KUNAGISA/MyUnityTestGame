
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace AutoGenerateUI.Test {
    /// <summary>
    /// 程序生成的UI代码类，请勿修改
    /// </summary>
    public sealed partial class TestSub  {
        public TestSub() {

        }
        public TestSub(Transform inst) {
            Init(inst);
        }
        
        public Text Test  { get; private set; }

        public void Init(Transform inst) {
            
            Transform TestXform = inst.Find("Test");
            if ( TestXform != null) {
                Test = TestXform.GetComponent<Text>();
            } else {
                Debug.LogError("Test Can't Find Under inst");
            }
        }

        public void Free() {
            
            Test = null;
        }
    }
}
