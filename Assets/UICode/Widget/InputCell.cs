using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class InputCell : Cell {
    public InputCell() : base() { }
    public InputCell(Transform inst) : base(inst) { }

    public override void Init(Transform inst) {
        m_InputField = inst.GetComponent<InputField>();
        if (m_InputField != null) {
            selectable = m_InputField;
        }
        else {
            m_TMPInputField = inst.GetComponent<TMPro.TMP_InputField>();
            if (m_TMPInputField == null) {
                Debug.LogError("UInput InputField null, name:" + inst);
            }
            selectable = m_TMPInputField;
        }
        base.Init(inst);
    }

    private InputField m_InputField;
    private TMPro.TMP_InputField m_TMPInputField;
    private bool m_initListener = false;
    private Action<string> m_handler;
    private Action<string, InputCell> m_handlerWithSelf;
    private Action<string> m_handlerEnd;
    private Action<string, InputCell> m_handlerEndWithSelf;

    private void initListener() {
        if (m_initListener) {
            return;
        }
        m_initListener = true;
        if (m_InputField != null) {
            m_InputField.onValueChanged.AddListener(onValueChanged);
            m_InputField.onEndEdit.AddListener(onEndEdit);
        }
        else {
            m_TMPInputField.onValueChanged.AddListener(onValueChanged);
            m_TMPInputField.onEndEdit.AddListener(onEndEdit);
        }
    }

    /// <summary>
    /// 解耦与UGUI的点击事件
    /// 方便添加通用事件，避免UnityAction未触发时不能正常清空的问题
    /// </summary>
    private void onValueChanged(string value) {
        if (m_handler != null) {
            m_handler(value);
        }
        else if (m_handlerWithSelf != null) {
            m_handlerWithSelf(value, this);
        }
    }

    /// <summary>
    /// 解耦与UGUI的点击事件
    /// 方便添加通用事件，避免UnityAction未触发时不能正常清空的问题
    /// </summary>
    private void onEndEdit(string value) {
        if (m_handlerEnd != null) {
            m_handlerEnd(value);
        }
        else if (m_handlerEndWithSelf != null) {
            m_handlerEndWithSelf(value, this);
        }
    }

    /// <summary>
    /// 设置点击事件回调，不支持多个回调，每次设置覆盖之前的回调
    /// </summary>
    /// <param name="call"></param>
    public void SetListener(Action<string> call, Action<string> endEdit = null) {
        initListener();
        m_handler = call;
        m_handlerWithSelf = null;
        m_handlerEnd = endEdit;
        m_handlerEndWithSelf = null;
    }

    /// <summary>
    /// 设置点击事件回调，不支持多个回调，每次设置覆盖之前的回调
    /// </summary>
    /// <param name="call"></param>
    public void SetListener(Action<string, InputCell> call, Action<string, InputCell> endEdit = null) {
        initListener();
        m_handler = null;
        m_handlerWithSelf = call;
        m_handlerEnd = null;
        m_handlerEndWithSelf = endEdit;
    }

    /// <summary>
    /// 清空点击回调
    /// </summary>
    public void ClearListener() {
        m_handler = null;
        m_handlerWithSelf = null;
        m_handlerEnd = null;
        m_handlerEndWithSelf = null;
    }

    /// <summary>
    /// 输入字符有效性检查
    /// delegate char OnValidateInput(string text, int charIndex, char addedChar)
    /// </summary>
    /// <param name="validate"></param>
    public void SetValidateInput(InputField.OnValidateInput validate) {
        m_InputField.onValidateInput = validate;
    }

    public void SetTMPValidateInput(TMPro.TMP_InputField.OnValidateInput validate) {
        m_TMPInputField.onValidateInput = validate;
    }

    public override void Free() {
        base.Free();
        ClearListener();
    }

    public bool interactable {
        get {
            if (m_InputField != null) {
                return m_InputField.interactable;
            }
            else {
                return m_TMPInputField.interactable;
            }
        }
        set {
            if (m_InputField != null) {
                m_InputField.interactable = value;
                m_InputField.enabled = value;
            }
            else {
                m_TMPInputField.interactable = value;
                m_TMPInputField.enabled = value;
            }
        }
    }

    public bool isFocused {
        get {
            if (m_InputField != null) {
                return m_InputField.isFocused;
            }
            else {
                return m_TMPInputField.isFocused;
            }
        }
    }

    public Graphic placeholder {
        get {
            if (m_InputField != null) {
                return m_InputField.placeholder;
            }
            else {
                return m_TMPInputField.placeholder;
            }
        }
        set {
            if (m_InputField != null) {
                m_InputField.placeholder = value;
            }
            else {
                m_TMPInputField.placeholder = value;
            }
        }
    }

    public string placeholderText {
        set {
            if (m_InputField != null) {
                Text text = m_InputField.placeholder as Text;
                text.text = value;
            }
            else if (m_TMPInputField != null) {
                TMPro.TextMeshProUGUI text = m_TMPInputField.placeholder as TMPro.TextMeshProUGUI;
                text.text = value;
            }
            else {
                Debug.LogError("set placeholderText InputField null:" + transform, transform);
            }
        }
    }

    public string text {
        get {
            if (m_InputField != null) {
                return m_InputField.text;
            }
            else {
                return m_TMPInputField.text;
            }
        }
        set {
            if (m_InputField != null) {
                m_InputField.text = value;
            }
            else {
                m_TMPInputField.text = value;
            }
        }
    }

    public void SetTextWithoutNotify(string text) {
        if (m_InputField != null) {
            m_InputField.SetTextWithoutNotify(text);
        }
        else {
            m_TMPInputField.SetTextWithoutNotify(text);
        }
    }

    public TMPro.TMP_InputField TMPInputField {
        get {
            return m_TMPInputField;
        }
    }
}
