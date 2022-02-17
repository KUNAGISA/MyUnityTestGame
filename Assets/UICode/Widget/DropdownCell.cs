using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DropdownCell : Cell {
    public DropdownCell() : base() { }
    public DropdownCell(Transform inst) : base(inst) { }

    public override void Init(Transform inst) {
        m_Dropdown = inst.GetComponent<Dropdown>();
        if (m_Dropdown == null) {
            Debug.LogError("UDropdown Dropdown null,name:" + inst);
        }
        selectable = m_Dropdown;
        base.Init(inst);
        ExpandTouchArea();
    }

    private Dropdown m_Dropdown;
    private bool m_initListener = false;
    private Action<int> m_handler;
    private Action<int,DropdownCell> m_handlerWithSelf;

    private void initListener() {
        if (m_initListener) {
            return;
        }
        m_initListener = true;
        m_Dropdown.onValueChanged.AddListener(onValueChanged);
    }

    /// <summary>
    /// 解耦与UGUI的点击事件
    /// 方便添加通用事件，避免UnityAction<int>未触发时不能正常清空的问题
    /// </summary>
    private void onValueChanged(int value) {
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
    public void SetListener(Action<int> call) {
        initListener();
        m_handler = call;
        m_handlerWithSelf = null;
    }

    /// <summary>
    /// 设置点击事件回调，不支持多个回调，每次设置覆盖之前的回调
    /// </summary>
    /// <param name="call"></param>
    public void SetListener(Action<int, DropdownCell> call) {
        initListener();
        m_handler = null;
        m_handlerWithSelf = call;
    }

    ///<summary>
    ///清空点击回调
    ///</summary>
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
            return m_Dropdown.interactable;
        }
        set {
            m_Dropdown.interactable = value;
        }
    }

    public int value {
        get {
            return m_Dropdown.value;
        }
        set {
            m_Dropdown.value = value;
            m_Dropdown.RefreshShownValue();
        }
    }

    public Text captionText {
        get { return m_Dropdown.captionText; }
        set { m_Dropdown.captionText = value; }
    }

    public void ClearOptions() {
        m_Dropdown.ClearOptions();
    }

    public List<Dropdown.OptionData> options {
        get { return m_Dropdown.options; }
        set { m_Dropdown.options = value; }
    }

    public void AddOptions(List<Dropdown.OptionData> options) {
        m_Dropdown.AddOptions(options);
    }

    public void Add(Dropdown.OptionData data) {
        options.Add(data);
    }

}
