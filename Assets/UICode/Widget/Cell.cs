using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 基础UI节点
/// </summary>
public class Cell : AbsCell {
    #region 静态函数
    /// <summary>
    /// 播放音效
    /// </summary>
    public static Action<int> audioPlayHandler {
        get;
        set;
    }
    #endregion 静态函数

    public Cell() { }

    public Cell(Transform transform) {
        Init(transform);
    }

    protected void PlayAudio(int id) {
        if (audioPlayHandler == null) {
            Debug.LogWarning("audioPlayHandler == null");
            return;
        }

        if (id == 0) {
            //cc.log("playAudio id=0");
            return;
        }
        audioPlayHandler(id);
    }

    protected Selectable selectable { get; set; }

    public Graphic targetGraphic {
        get {
            if (selectable == null) {
                return null;
            }
            return selectable.targetGraphic;
        }
        set {
            if (selectable == null) {
                return;
            }
            selectable.targetGraphic = value;
        }
    }

    public static int minWidth = 50;
    public static int minHeight = 50;

    /// <summary>
    /// 扩大可点击区域
    /// </summary>
    public void ExpandTouchArea() {
        if (selectable == null) {
            Debug.LogError($"{rectTransform.GetHierarchyPath()} ExpandTouchArea selectable = null", rectTransform);
            return;
        }
        var graphic = selectable.targetGraphic;
        if (graphic == null) {
            Debug.LogError($"{rectTransform.GetHierarchyPath()} ExpandTouchArea selectable.targetGraphic = null", rectTransform);
            return;
        }

        var padding = graphic.raycastPadding;
        if (padding != Vector4.zero) {
            //已经设置过参数
            return;
        }
        var size = graphic.rectTransform.sizeDelta;
        var width = size.x -padding.x - padding.z;

        if (width >= minWidth) {
            //已满足最低限度要求
            return;
        }
        else {
            var halfOffsetX = (minWidth - size.x) * 0.5f;
            padding.x = padding.z = -halfOffsetX;
        }

        var height = size.y - padding.y - padding.w;
        if (height >= minHeight) {
            //已满足最低限度要求
            return;
        }
        else {
            var halfOffsetY = (minHeight - size.y) * 0.5f;
            padding.y = padding.w = -halfOffsetY;
        }

        graphic.raycastPadding = padding;
    }

    public bool raycastTarget {
        get {
            if (targetGraphic == null) {
                Debug.LogError(transform + " get raycastTarget null");
                return false;
            }
            return targetGraphic.raycastTarget;
        }
        set {
            if (targetGraphic == null) {
                Debug.LogError(transform + " set raycastTarget null");
                return;
            }
            targetGraphic.raycastTarget = value;
        }
    }

    protected IText m_MainText;
    public IText mainText {
        get {
            if (m_MainText == null) {
                Transform xform = transform.Find("_Text");
                if (xform != null) {
                    m_MainText = xform.GetComponent<IText>();
                }
                else {
                    //if can't find Icon, use self instead; it's text button/toggle
                    m_MainText = transform.GetComponent<IText>();
                    if (m_MainText == null) {
                        Transform textChild = transform.Find("_Icon/_Text");
                        if (textChild != null) {
                            m_MainText = textChild.GetComponent<IText>();
                        }
                        if (m_MainText == null) {
                            textChild = transform.Find("_Stamp/_Text");
                            if (textChild != null) {
                                m_MainText = textChild.GetComponent<IText>();
                            }
                        }
                        if (m_MainText == null) {
                            textChild = transform.Find("_NumText/_Icon/_Text");
                            if (textChild != null) {
                                m_MainText = textChild.GetComponent<IText>();
                            }
                        }
                    }
                }
            }
            return m_MainText;
        }
    }

    protected Graphic m_Bg;
    public Graphic bg {
        get {
            if (m_Bg == null) {
                Transform xform = transform.Find("_Bg");
                if (xform != null) {
                    m_Bg = xform.GetComponent<Graphic>();
                }
                else {
                    m_Bg = transform.GetComponent<Graphic>();
                }
            }
            return m_Bg;
        }
    }

    protected Graphic m_Icon;
    public Graphic icon {
        get {
            if (m_Icon == null) {
                Transform xform = transform.Find("_Icon");
                if (xform != null) {
                    m_Icon = xform.GetComponent<Graphic>();
                }
                else {
                    //if can't find Icon, use self instead; it's image button/toggle
                    m_Icon = transform.GetComponent<Graphic>();
                }
            }
            return m_Icon;
        }
    }

    protected Graphic m_IconFrame;
    public Graphic iconFrame {
        get {
            if (m_IconFrame == null) {
                Transform xform = transform.Find("_IconFrame");
                if (xform != null) {
                    m_IconFrame = xform.GetComponent<Graphic>();
                }
                else {
                    
                }
            }
            return m_IconFrame;
        }
    }

    protected IText m_SubText;
    public IText subText {
        get {
            if (m_SubText == null) {
                Transform xform = transform.Find("_SubText");
                if (xform != null) {
                    m_SubText = xform.GetComponent<IText>();
                }
                else {
                    Transform textChild = transform.Find("_Stamp/_SubText");
                    if (textChild != null) {
                        m_SubText = textChild.GetComponent<IText>();
                    }
                    else {
                        textChild = transform.Find("_Text/_Stamp/_SubText");
                        if (textChild != null) {
                            m_SubText = textChild.GetComponent<IText>();
                        }
                        if (m_SubText == null) {
                            textChild = transform.Find("_NumText/_Icon/_SubText");
                            if (textChild != null) {
                                m_SubText = textChild.GetComponent<IText>();
                            }
                        }
                        if (m_SubText == null) {
                            textChild = transform.Find("_Text/_SubText");
                            if (textChild != null) {
                                m_SubText = textChild.GetComponent<IText>();
                            }
                        }
                    }
                }
            }
            return m_SubText;
        }
    }

    protected Graphic m_TextIcon;
    public Graphic textIcon {
        get {
            if (m_TextIcon == null) {
                Transform xform = transform.Find("_Text/_Icon");
                if (xform != null) {
                    m_TextIcon = xform.GetComponent<Graphic>();
                }
                else {
                    xform = transform.Find("_SubText/_Icon");
                    if (xform != null) {
                        m_TextIcon = xform.GetComponent<Graphic>();
                    }
                }
            }
            return m_TextIcon;
        }
    }

    protected Graphic m_NumIcon;
    public Graphic numIcon {
        get {
            if (m_NumIcon == null) {
                Transform xform = transform.Find("_NumIcon");
                if (xform != null) {
                    m_NumIcon = xform.GetComponent<Graphic>();
                }
                else {
                    Transform iconChild = transform.Find("_NumText/_Icon");
                    if (iconChild != null) {
                        m_NumIcon = iconChild.GetComponent<Graphic>();
                    }
                }
            }
            return m_NumIcon;
        }
    }

    protected IText m_NumText;
    public IText numText {
        get {
            if (m_NumText == null) {
                Transform xform = transform.Find("_NumText");
                if (xform != null) {
                    m_NumText = xform.GetComponent<IText>();
                }
                else {
                    Transform textChild = transform.Find("_NumIcon/_Text");
                    if (textChild != null) {
                        m_NumText = textChild.GetComponent<IText>();
                    }
                }
            }
            return m_NumText;
        }
    }

    protected Graphic m_LvIcon;
    public Graphic lvIcon {
        get {
            if (m_LvIcon == null) {
                Transform xform = transform.Find("_LvIcon");
                if (xform != null) {
                    m_LvIcon = xform.GetComponent<Graphic>();
                }
                else {
                    Transform iconChild = transform.Find("_LvText/_Icon");
                    if (iconChild != null) {
                        m_LvIcon = iconChild.GetComponent<Graphic>();
                    }
                }
            }
            return m_LvIcon;
        }
    }

    protected IText m_LvText;
    public IText lvText {
        get {
            if (m_LvText == null) {
                Transform xform = transform.Find("_LvText");
                if (xform != null) {
                    m_LvText = xform.GetComponent<IText>();
                }
                else {
                    Transform textChild = transform.Find("_LvIcon/_Text");
                    if (textChild != null) {
                        m_LvText = textChild.GetComponent<IText>();
                    }
                    else {
                        textChild = transform.Find("_Text/_LvText");
                        if (textChild != null) {
                            m_LvText = textChild.GetComponent<IText>();
                        }
                    }
                }
            }
            return m_LvText;
        }
    }

    private void initTitleAndInfos() {
        if (m_Titles != null) {
            return;
        }
        IText[] texts = transform.GetComponentsInChildren<IText>();
        List<IText> list1 = new List<IText>(texts.Length);
        List<IText> list2 = new List<IText>(texts.Length);
        for (int i = 0; i < texts.Length; i++) {
            string name = texts[i].gameObject.name;
            if (name.Equals("_Titles", StringComparison.Ordinal)) {
                list1.Add(texts[i]);
            }
            else if (name.Equals("_Infos", StringComparison.Ordinal)) {
                list2.Add(texts[i]);
            }
        }
        m_Titles = list1.ToArray();
        m_Infos = list2.ToArray();
    }

    private IText[] m_Titles;
    public IText[] titles {
        get {
            initTitleAndInfos();
            return m_Titles;
        }
    }

    private IText[] m_Infos;
    public IText[] infos {
        get {
            initTitleAndInfos();
            return m_Infos;
        }
    }

    protected Graphic m_CostIcon;
    public Graphic costIcon {
        get {
            if (m_CostIcon == null) {
                Transform xform = transform.Find("_CostIcon");
                if (xform != null) {
                    m_CostIcon = xform.GetComponent<Graphic>();
                }
                else {
                    Transform iconChild = transform.Find("_CostText/_Icon");
                    if (iconChild != null) {
                        m_CostIcon = iconChild.GetComponent<Graphic>();
                    }
                }
            }
            return m_CostIcon;
        }
    }

    protected IText m_CostText;
    public IText costText {
        get {
            if (m_CostText == null) {
                Transform xform = transform.Find("_CostText");
                if (xform != null) {
                    m_CostText = xform.GetComponent<IText>();
                }
                else {
                    Transform textChild = transform.Find("_CostIcon/_Text");
                    if (textChild != null) {
                        m_CostText = textChild.GetComponent<IText>();
                    }
                }
            }
            return m_CostText;
        }
    }

    private GameObject m_Badge;
    public GameObject badge {
        get {
            if (m_Badge == null) {
                Transform xform = transform.Find("_Badge");
                if (xform != null) {
                    m_Badge = xform.gameObject;
                    xform = xform.Find("_Text");
                    if (xform != null) {
                        m_BadgeText = xform.GetComponent<IText>();
                    }
                }
            }
            return m_Badge;
        }
    }

    private IText m_BadgeText;
    public IText badgeText {
        get {
            if (badge == null) {
                return null;
            }
            return m_BadgeText;
        }
    }

    protected Graphic m_Mask;
    public Graphic mask {
        get {
            if (m_Mask == null) {
                Transform xform = transform.Find("_Mask");
                if (xform != null) {
                    m_Mask = xform.GetComponent<Graphic>();
                }
                else {
                    Transform iconChild = transform.Find("_Quality/_Mask");
                    if (iconChild != null) {
                        m_Mask = iconChild.GetComponent<Graphic>();
                    }
                }
            }
            return m_Mask;
        }
    }

    private IText m_MaskText;
    public IText maskText {
        get {
            if (m_MaskText == null) {
                Transform xform = transform.Find("_MaskText");
                if (xform != null) {
                    m_MaskText = xform.GetComponent<IText>();
                }
                else {
                    Transform textChild = transform.Find("_Mask/_Text");
                    if (textChild != null) {
                        m_MaskText = textChild.GetComponent<IText>();
                    }
                }
            }
            return m_MaskText;
        }
    }

    private Graphic m_Flag;
    public Graphic flag {
        get {
            if (m_Flag == null) {
                Transform xform = transform.Find("_Flag");
                if (xform != null) {
                    m_Flag = xform.GetComponent<Graphic>();
                }
            }
            return m_Flag;
        }
    }

    private IText m_FlagText;
    public IText flagText {
        get {
            if (m_FlagText == null) {
                Transform xform = transform.Find("_FlagText");
                if (xform != null) {
                    m_FlagText = xform.GetComponent<IText>();
                }
                else {
                    Transform textChild = transform.Find("_Flag/_Text");
                    if (textChild != null) {
                        m_FlagText = textChild.GetComponent<IText>();
                    }
                }
            }
            return m_FlagText;
        }
    }

    private Graphic m_Sign;
    public Graphic sign {
        get {
            if (m_Sign == null) {
                Transform xform = transform.Find("_Sign");
                if (xform != null) {
                    m_Sign = xform.GetComponent<Graphic>();
                }
            }
            return m_Sign;
        }
    }

    private IText m_SignText;
    public IText signText {
        get {
            if (m_SignText == null) {
                Transform xform = transform.Find("_SignText");
                if (xform != null) {
                    m_SignText = xform.GetComponent<IText>();
                }
                else {
                    Transform textChild = transform.Find("_Sign/_Text");
                    if (textChild != null) {
                        m_SignText = textChild.GetComponent<IText>();
                    }
                }
            }
            return m_SignText;
        }
    }

    protected Graphic m_SignIcon;
    public Graphic signIcon {
        get {
            if (m_SignIcon == null) {
                Transform xform = transform.Find("_SignIcon");
                if (xform != null) {
                    m_SignIcon = xform.GetComponent<Graphic>();
                }
                else {
                    Transform iconChild = transform.Find("_Sign/_SignIcon");
                    if (iconChild != null) {
                        m_SignIcon = iconChild.GetComponent<Graphic>();
                    }
                }
            }
            return m_SignIcon;
        }
    }

    private Transform m_Point;
    public Transform point {
        get {
            if (m_Point == null) {
                Transform xform = transform.Find("_Point");
                if (xform != null) {
                    m_Point = xform;
                }
            }
            return m_Point;
        }
    }

    private GameObject m_Stamp;
    public GameObject stamp {
        get {
            if (m_Stamp == null) {
                Transform xform = transform.Find("_Stamp");
                if (xform != null) {
                    m_Stamp = xform.gameObject;
                }
                else {
                    Transform tform = transform.Find("_Text/_Stamp");
                    if (tform != null) {
                        m_Stamp = tform.gameObject;
                    }
                }
            }
            return m_Stamp;
        }
    }

    protected GameObject m_Select;
    public GameObject select {
        get {
            if (m_Select == null) {
                Transform xform = transform.Find("_Select");
                if (xform != null) {
                    m_Select = xform.gameObject;
                }
            }
            return m_Select;
        }
    }

    private IText m_SelectText;
    public IText selectText {
        get {
            if (m_SelectText == null) {
                Transform xform = transform.Find("_SelectText");
                if (xform != null) {
                    m_SelectText = xform.GetComponent<IText>();
                }
                else {
                    Transform textChild = transform.Find("_Select/_Text");
                    if (textChild != null) {
                        m_SelectText = textChild.GetComponent<IText>();
                    }
                }
            }
            return m_SelectText;
        }
    }

    #region Enable/Disable

    private GameObject m_EnableNode;
    public GameObject enableNode {
        get {
            if (m_EnableNode == null) {
                enableInit();
            }
            return m_EnableNode;
        }
    }

    private GameObject m_DisableNode;
    public GameObject disableNode {
        get {
            if (m_DisableNode == null) {
                disableInit();
            }
            return m_DisableNode;
        }
    }

    private IText m_EnableText;
    public IText enableText {
        get {
            if (m_EnableText == null) {
                enableInit();
            }
            return m_EnableText;
        }
    }

    private IText m_DisableText;
    public IText disableText {
        get {
            if (m_DisableText == null) {
                disableInit();
            }
            return m_DisableText;
        }
    }

    private Graphic m_EnableIcon;
    public Graphic enableIcon {
        get {
            if (m_EnableIcon == null) {
                enableInit();
            }
            return m_EnableIcon;
        }
    }

    private Graphic m_DisableIcon;
    public Graphic disableIcon {
        get {
            if (m_DisableIcon == null) {
                disableInit();
            }
            return m_DisableIcon;
        }
    }

    private bool m_EnableInit;
    private void enableInit() {
        if (m_EnableInit) {
            return;
        }
        m_EnableInit = true;

        Transform xform = transform.Find("_Enable");
        if (xform != null) {
            m_EnableNode = xform.gameObject;

            Transform textChild = xform.Find("_Text");
            if (textChild != null) {
                m_EnableText = textChild.GetComponent<IText>();
            }

            Transform child = xform.Find("_Icon");
            if (child != null) {
                m_EnableIcon = child.GetComponent<Graphic>();
            }
        }
    }

    private bool m_DisableInit;
    private void disableInit() {
        if (m_DisableInit) {
            return;
        }
        m_DisableInit = true;

        Transform xform = transform.Find("_Disable");
        if (xform != null) {
            m_DisableNode = xform.gameObject;

            Transform textChild = xform.Find("_Text");
            if (textChild != null) {
                m_DisableText = textChild.GetComponent<IText>();
            }

            Transform child = xform.Find("_Icon");
            if (child != null) {
                m_DisableIcon = child.GetComponent<Graphic>();
            }
        }
    }

    #endregion

    protected Graphic m_Quality;
    public Graphic quality {
        get {
            if (m_Quality == null) {
                Transform xform = transform.Find("_Quality");
                if (xform != null) {
                    m_Quality = xform.GetComponent<Graphic>();
                }
            }
            return m_Quality;
        }
    }

    protected Graphic m_Mark;
    public Graphic mark {
        get {
            if (m_Mark == null) {
                Transform xform = transform.Find("_Mark");
                if (xform != null) {
                    m_Mark = xform.GetComponent<Graphic>();
                }
            }
            return m_Mark;
        }
    }

    private Graphic m_Category;
    /// <summary>
    /// 种类标识
    /// </summary>
    public Graphic category {
        get {
            if (m_Category == null) {
                Transform xform = transform.Find("_Category");
                if (xform != null) {
                    m_Category = xform.GetComponent<Graphic>();
                }
            }
            return m_Category;
        }
    }

    private RawImage m_RawImg;
    public RawImage rawImg {
        get {
            if (m_RawImg == null) {
                Transform xform = transform.Find("_RawImg");
                if (xform != null) {
                    m_RawImg = xform.GetComponent<RawImage>();
                }
                else {
                    m_RawImg = transform.GetComponent<RawImage>();
                }
            }
            return m_RawImg;
        }
    }
}
