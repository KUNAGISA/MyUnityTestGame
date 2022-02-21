
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
        public ScrollRectExtra Scroll  { get; private set; }
        public RectTransform Viewport  { get; private set; }
        public RectTransform Content  { get; private set; }
        public ScrollRectExtra ChildScroll1  { get; private set; }
        public ScrollRectExtra ChildScroll2  { get; private set; }
        public ScrollRectExtra ChildScroll3  { get; private set; }
        public ScrollRectExtra ChildScroll4  { get; private set; }

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
            Transform ScrollXform = PanelXform.Find("Scroll");
            if ( ScrollXform != null) {
                Scroll = ScrollXform.GetComponent<ScrollRectExtra>();
            } else {
                Debug.LogError("Scroll Can't Find Under PanelXform");
            }
            Transform ViewportXform = ScrollXform.Find("Viewport");
            if ( ViewportXform != null) {
                Viewport = ViewportXform as RectTransform;
            } else {
                Debug.LogError("Viewport Can't Find Under ScrollXform");
            }
            Transform ContentXform = ViewportXform.Find("Content");
            if ( ContentXform != null) {
                Content = ContentXform as RectTransform;
            } else {
                Debug.LogError("Content Can't Find Under ViewportXform");
            }
            Transform ChildScroll1Xform = ContentXform.Find("ChildScroll1");
            if ( ChildScroll1Xform != null) {
                ChildScroll1 = ChildScroll1Xform.GetComponent<ScrollRectExtra>();
            } else {
                Debug.LogError("ChildScroll1 Can't Find Under ContentXform");
            }
            Transform ChildScroll2Xform = ContentXform.Find("ChildScroll2");
            if ( ChildScroll2Xform != null) {
                ChildScroll2 = ChildScroll2Xform.GetComponent<ScrollRectExtra>();
            } else {
                Debug.LogError("ChildScroll2 Can't Find Under ContentXform");
            }
            Transform ChildScroll3Xform = ContentXform.Find("ChildScroll3");
            if ( ChildScroll3Xform != null) {
                ChildScroll3 = ChildScroll3Xform.GetComponent<ScrollRectExtra>();
            } else {
                Debug.LogError("ChildScroll3 Can't Find Under ContentXform");
            }
            Transform ChildScroll4Xform = ContentXform.Find("ChildScroll4");
            if ( ChildScroll4Xform != null) {
                ChildScroll4 = ChildScroll4Xform.GetComponent<ScrollRectExtra>();
            } else {
                Debug.LogError("ChildScroll4 Can't Find Under ContentXform");
            }
        }

        public void Free() {
            
            Panel = null;
            if (TestBtn != null) {
                TestBtn.Free();
            }
            TestBtn = null;
            TestRawImg = null;
            Scroll = null;
            Viewport = null;
            Content = null;
            ChildScroll1 = null;
            ChildScroll2 = null;
            ChildScroll3 = null;
            ChildScroll4 = null;
        }
    }
}
