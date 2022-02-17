using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class ButtonCell : Cell {
    public ButtonCell() : base() { }
    public ButtonCell(Transform inst) : base(inst) { }

    public override void Init(Transform inst) {
        m_Button = inst.GetComponent<Button>();
        if (m_Button == null) {
            Debug.LogError("UButton Button null, name:" + inst);
        }
        selectable = m_Button;
        base.Init(inst);
        ExpandTouchArea();
    }


    public int clickAudioId = 0;

    private Button m_Button;
    private bool m_initListener = false;
    private Action m_handler;
    private Action<ButtonCell> m_handlerWithSelf;

    private void initListener() {
        if (m_initListener) {
            return;
        }
        m_initListener = true;
        m_Button.onClick.AddListener(onClick);
    }

    private float m_nextClickTime = 0;
    public float clickGap = 0.15f;

    /// <summary>
    /// 解耦与UGUI的点击事件
    /// 方便添加通用事件，避免UnityAction未触发时不能正常清空的问题
    /// </summary>
    private void onClick() {
        if (m_nextClickTime > Time.time) {
            return;
        }
        m_nextClickTime = Time.time + clickGap;

        //播放点击音效
        PlayAudio(clickAudioId);

        if (m_handler != null) {
            m_handler();
        }
        else if (m_handlerWithSelf != null) {
            m_handlerWithSelf(this);
        }
    }

    /// <summary>
    /// 设置点击事件回调，不支持多个回调，每次设置覆盖之前的回调
    /// </summary>
    /// <param name="call"></param>
    public void SetListener(Action call) {
        initListener();
        m_handler = call;
        m_handlerWithSelf = null;
    }

    /// <summary>
    /// 设置点击事件回调，不支持多个回调，每次设置覆盖之前的回调
    /// </summary>
    /// <param name="call"></param>
    public void SetListener(Action<ButtonCell> call) {
        initListener();
        m_handler = null;
        m_handlerWithSelf = call;
    }

    /// <summary>
    /// 清空点击回调
    /// </summary>
    public void ClearListener() {
        m_handler = null;
        m_handlerWithSelf = null;
    }

    public override void Free() {
        base.Free();
        ClearListener();
    }

    public bool interactable {
        get {
            return m_Button.interactable;
        }
        set {
            m_Button.interactable = value;
            m_Button.enabled = value;
        }
    }

    public void SetExpand() {
        Transform child = transform.Find("_Expand");
        if (child != null) {
            child.GetComponent<Blank>().raycastTarget = false;
        }
    }


    private bool m_isGray;
    public void SetGray() {
        if (m_isGray) {
            return;
        }
        m_isGray = true;

        if (bg != null) {
            m_Bg.SetGray();
        }
        if (icon != null) {
            m_Icon.SetGray();
        }
        if (mainText != null) {
            m_MainText.SetGray();
        }
        if (costIcon != null) {
            m_CostIcon.SetGray();
        }
        if (costText != null) {
            m_CostText.SetGray();
        }
        if (subText != null) {
            m_SubText.SetGray();
        }
        if (numText != null) {
            m_NumText.SetGray();
        }
        if (numIcon != null) {
            m_NumIcon.SetGray();
        }
        if (quality != null) {
            m_Quality.SetGray();
        }
        if (mask != null) {
            m_Mask.SetGray();
        }
    }

    public void UnsetGray() {
        if (!m_isGray) {
            return;
        }
        m_isGray = false;

        if (bg != null) {
            m_Bg.UnsetGray();
        }
        if (icon != null) {
            m_Icon.UnsetGray();
        }
        if (mainText != null) {
            m_MainText.UnsetGray();
        }
        if (costIcon != null) {
            m_CostIcon.UnsetGray();
        }
        if (costText != null) {
            m_CostText.UnsetGray();
        }
        if (subText != null) {
            m_SubText.UnsetGray();
        }
        if (numText != null) {
            m_NumText.UnsetGray();
        }
        if (numIcon != null) {
            m_NumIcon.UnsetGray();
        }
        if (quality != null) {
            m_Quality.UnsetGray();
        }
        if (mask != null) {
            m_Mask.UnsetGray();
        }
    }

    public bool IsGray() {
        return m_isGray;
    }

    private bool m_enabled = true;
    public void Enable() {
        if (m_enabled) {
            return;
        }
        m_enabled = true;
        interactable = true;
    }

    /// <summary>
    /// 显示
    /// </summary>
    public void Active() {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    public void UnActive() {
        gameObject.SetActive(false);
    }

    public void Disable() {
        if (!m_enabled) {
            return;
        }
        m_enabled = false;
        interactable = false;
    }

    #region 配合TabGroup使用
    private bool m_isInitChecke;
    private void initChecked() {
        if (m_isInitChecke) {
            return;
        }
        m_isInitChecke = true;
        _ = this.select;
    }

    public void ShowSelect(bool isSelect) {
        initChecked();
        if (enableNode != null) {
            enableNode.SetActive(isSelect);
        }
        if (disableNode != null) {
            disableNode.SetActive(!isSelect);
        }
        if (m_Select != null) {
            m_Select.SetActive(isSelect);
        }
    }
    #endregion TabGroup
}
