using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class SliderCell : Cell {
    public SliderCell() : base() { }
    public SliderCell(Transform inst) : base(inst) { }

    public override void Init(Transform inst) {
        m_Slider = inst.GetComponent<Slider>();
        if (m_Slider == null) {
            Debug.LogError("SliderCell Slider null, name:" + inst);
        }
        selectable = m_Slider;
        base.Init(inst);
    }

    private Slider m_Slider;
    private bool m_initListener = false;
    private Action<float> m_handler;
    private Action<float, SliderCell> m_handlerWithSelf;

    private void initListener() {
        if (m_initListener) {
            return;
        }
        m_initListener = true;
        m_Slider.onValueChanged.AddListener(onValueChanged);
    }

    /// <summary>
    /// 解耦与UGUI的点击事件
    /// 方便添加通用事件，避免UnityAction未触发时不能正常清空的问题
    /// </summary>
    private void onValueChanged(float value) {
        if (m_initValue) {
            return;
        }
        if (m_handler != null) {
            m_handler(value);
        }
        else if (m_handlerWithSelf != null) {
            m_handlerWithSelf(value, this);
        }
    }

    /// <summary>
    /// 设置点击事件回调，不支持多个回调，每次设置覆盖之前的回调
    /// </summary>
    /// <param name="call"></param>
    public void SetListener(Action<float> call) {
        initListener();
        m_handler = call;
        m_handlerWithSelf = null;
    }

    /// <summary>
    /// 设置点击事件回调，不支持多个回调，每次设置覆盖之前的回调
    /// </summary>
    /// <param name="call"></param>
    public void SetListener(Action<float, SliderCell> call) {
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

    public Slider.Direction direction {
        get { return m_Slider.direction; }
        set { m_Slider.direction = value; }
    }

    public RectTransform fillRect {
        get { return m_Slider.fillRect; }
        set { m_Slider.fillRect = value; }
    }

    public RectTransform handleRect {
        get { return m_Slider.handleRect; }
        set { m_Slider.handleRect = value; }
    }

    public float maxValue {
        get { return m_Slider.maxValue; }
        set { m_Slider.maxValue = value; }
    }

    public float minValue {
        get { return m_Slider.minValue; }
        set { m_Slider.minValue = value; }
    }

    public float normalizedValue {
        get { return m_Slider.normalizedValue; }
        set { m_Slider.normalizedValue = value; }
    }

    public float value {
        get { return m_Slider.value; }
        set { m_Slider.value = value; }
    }

    public bool wholeNumbers {
        get { return m_Slider.wholeNumbers; }
        set { m_Slider.wholeNumbers = value; }
    }

    public void setInteractable(bool value) {
        m_Slider.interactable = value;
    }

    private Text m_HandleText;
    public Text handleText {
        get {
            if (m_HandleText == null) {
                Transform xform = m_Slider.handleRect.Find("_Text");
                if (xform != null) {
                    m_HandleText = xform.GetComponent<Text>();
                }
            }
            return m_HandleText;
        }
    }

    private bool m_initValue = false;

    //初始化选中状态，不触发要回调事件，用于打开界面的设置
    public void InitValue(float value) {
        if (Mathf.Approximately(this.value, value)) {
            //cc.log(this.node.name + " InitChecked skip:", isChecked);
            return;
        }
        m_initValue = true;
        this.value = value;
        m_initValue = false;
    }

    public void InitMaxValue(float maxValue) {
        if (Mathf.Approximately(this.maxValue, maxValue)) {
            //cc.log(this.node.name + " InitChecked skip:", isChecked);
            return;
        }
        m_initValue = true;
        this.maxValue = maxValue;
        m_initValue = false;
    }

}
