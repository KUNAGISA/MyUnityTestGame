using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class BarCell : Cell {
    public BarCell() : base() { }
    public BarCell(Transform inst) : base(inst) { }

    public override void Init(Transform inst) {
        m_Scrollbar = inst.GetComponent<Scrollbar>();
        if (m_Scrollbar == null) {
            Debug.LogError("BarCell Scrollbar null, name:" + inst);
        }
        base.Init(inst);
    }

    private Scrollbar m_Scrollbar;
    private bool m_initListener = false;
    private Action<float> m_handler;
    private Action<float, BarCell> m_handlerWithSelf;

    private void initListener() {
        if (m_initListener) {
            return;
        }
        m_initListener = true;
        m_Scrollbar.onValueChanged.AddListener(onValueChanged);
    }

    /// <summary>
    /// 解耦与UGUI的点击事件
    /// 方便添加通用事件，避免UnityAction未触发时不能正常清空的问题
    /// </summary>
    private void onValueChanged(float value) {
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
    public void SetListener(Action<float, BarCell> call) {
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

    public RectTransform handleRect {
        get { return m_Scrollbar.handleRect; }
        set { m_Scrollbar.handleRect = value; }
    }

    public int numberOfSteps {
        get { return m_Scrollbar.numberOfSteps; }
        set { m_Scrollbar.numberOfSteps = value; }
    }

    public float size {
        get { return m_Scrollbar.size; }
        set { m_Scrollbar.size = value; }
    }

    public float value {
        get { return m_Scrollbar.value; }
        set { m_Scrollbar.value = value; }
    }
}
