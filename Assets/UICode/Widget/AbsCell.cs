using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI节点基类
/// </summary>
public abstract class AbsCell {
    public GameObject gameObject { get; protected set; }

    public Transform transform { get { return rectTransform; } }

    public RectTransform rectTransform { get; protected set; }

    public virtual void Init(Transform inst) {
        if (inst == null) {
            Debug.LogError(this + " Init RectTransform is null");
            return;
        }
        this.gameObject = inst.gameObject;
        this.rectTransform = inst as RectTransform;
    }

    public virtual void Free() {
        if (gameObject) {
            GameObject.Destroy(gameObject);
            rectTransform = null;
            gameObject = null;
        }

        ClearParam();
    }

    /// <summary>
    /// 常用参数
    /// </summary>
    public int id = 0;
    public int intVal;
    public long longVal;
    /// <summary>
    /// 其他不满足的参数全部自定义类结构，存储在param中
    /// </summary>
    public object param;

    public void ClearParam() {
        id = 0;
        intVal = 0;
        longVal = 0;
        param = null;
    }

    //private AnimStub m_AnimStub;
    //public AnimStub animStub {
    //    get {
    //        if (m_AnimStub == null) {
    //            Animation animation = transform.GetComponent<Animation>();
    //            if (animation != null) {
    //                m_AnimStub = AnimStub.Create(animation);
    //            }
    //            else {
    //  //                   Animator animator = transform.GetComponent<Animator>();
    //  //                   if (animator != null) {
    //  //                       AnimInfoCsv csv = Cfg.AnimInfo.Get(animator.runtimeAnimatorController.name);
    //  //                       m_AnimStub = AnimStub.Create(animator, csv.anims);
    //  //                   }
    //  //                   else {
    //                    Debug.LogError(transform + " get Animation null");
    //                    return null;
    //  //                  }
    //            }
    //        }
    //        return m_AnimStub;
    //    }
    //}

    private LayoutElement m_LayoutElement;
    public LayoutElement layoutElement {
        get {
            if (m_LayoutElement == null) {
                m_LayoutElement = transform.GetComponent<LayoutElement>();
            }
            return m_LayoutElement;
        }
    }

    private CanvasGroup m_canvasGroup;
    public CanvasGroup canvasGroup {
        get {
            if (m_canvasGroup == null) {
                m_canvasGroup = transform.GetComponent<CanvasGroup>();
            }
            return m_canvasGroup;
        }
    }

    private Graphic m_Graphic;
    public Graphic graphic {
        get {
            if (m_Graphic == null) {
                m_Graphic = transform.GetComponent<Graphic>();
            }
            return m_Graphic;
        }
    }
}
