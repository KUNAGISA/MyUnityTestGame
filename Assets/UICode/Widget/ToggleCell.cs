using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class ToggleCell : Cell {
    public ToggleCell() : base() { }
    public ToggleCell(Transform inst) : base(inst) { }

    public override void Init(Transform inst) {
        m_Toggle = inst.GetComponent<Toggle>();
        if (m_Toggle == null) {
            Debug.LogError("UToggle Toggle null, name:" + inst);
        }
        selectable = m_Toggle;
        base.Init(inst);
        ExpandTouchArea();

        SetChecked(m_Toggle.isOn);
    }

    public int enableAudioId = 0;
    public int disableAudioId = 0;

    private bool m_colorInited;
    private Color m_EnableColor;
    private Color m_DisableColor;

    public void SetColor(Color enableColor, Color disableColor) {
        m_colorInited = true;
        m_EnableColor = enableColor;
        m_DisableColor = disableColor;
    }

    private Toggle m_Toggle;
    private bool m_initListener = false;
    private Action<bool> m_handler;
    private Action<bool, ToggleCell> m_handlerWithSelf;

    private void initListener() {
        if (m_initListener) {
            return;
        }
        m_initListener = true;
        m_Toggle.onValueChanged.AddListener(onClick);
    }

    private float m_nextClickTime = 0;
    public float clickGap = 0.15f;

    /// <summary>
    /// 解耦与UGUI的点击事件
    /// 方便添加通用事件，避免UnityAction<bool>未触发时不能正常清空的问题
    /// </summary>
    private void onClick(bool enable) {
        if (m_nextClickTime > Time.time) {
            return;
        }
        m_nextClickTime = Time.time + clickGap;

        SetChecked(enable);
        if (m_initCheck) {
            //cc.log(this.node.name + " skip InitChecked");
            return;
        }

        //播放点击音效
        if (enable) {
            PlayAudio(enableAudioId);
        }
        else {
            PlayAudio(disableAudioId);
        }

        if (m_handler != null) {
            m_handler(enable);
        }
        else if (m_handlerWithSelf != null) {
            m_handlerWithSelf(enable, this);
        }
    }

    /// <summary>
    /// 设置点击事件回调，不支持多个回调，每次设置覆盖之前的回调
    /// </summary>
    /// <param name="call"></param>
    public void SetListener(Action<bool> call) {
        initListener();
        m_handler = call;
        m_handlerWithSelf = null;
    }

    /// <summary>
    /// 设置点击事件回调，不支持多个回调，每次设置覆盖之前的回调
    /// </summary>
    /// <param name="call"></param>
    public void SetListener(Action<bool, ToggleCell> call) {
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
            return m_Toggle.interactable;
        }
        set {
            m_Toggle.interactable = value;
        }
    }

    public bool isOn {
        get {
            return m_Toggle.isOn;
        }
        set {
            m_Toggle.isOn = value;
        }
    }

    private bool m_initCheck = false;

    //初始化选中状态，不触发要回调事件，用于打开界面的设置
    public void InitChecked(bool isChecked) {
        if (m_Toggle.isOn == isChecked) {
            //cc.log(this.node.name + " InitChecked skip:", isChecked);
            return;
        }
        m_initCheck = true;
        m_Toggle.isOn = isChecked;
        SetChecked(isChecked);
        m_initCheck = false;
    }
    public void SetIsOnWithoutNotify(bool value) {
        m_Toggle.SetIsOnWithoutNotify(value);
    }
    private void SetChecked(bool isChecked) {
        if (enableNode != null) {
            enableNode.SetActive(isChecked);
        }
        if (disableNode != null) {
            disableNode.SetActive(!isChecked);
        }

        if (m_colorInited) {
            if (isChecked) {
                this.mainText.color = m_EnableColor;
            }
            else {
                this.mainText.color = m_DisableColor;
            }
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
    }
}
