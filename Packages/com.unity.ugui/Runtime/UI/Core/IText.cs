using System;

namespace UnityEngine.UI {
    /// <summary>
    /// Text and TextMeshPro interface
    /// </summary>
    public interface IText {
        /// <summary>
        /// ���ػ���Ψһ��ʾ���ձ�ʾ����Ҫ���ػ�
        /// </summary>
        string localizeKey { get; set; }
        string text { get; set; }
        bool raycastTarget { get; set; }
        RectTransform rectTransform { get; }
        GameObject gameObject { get; }
        float preferredHeight { get; }
        float preferredWidth { get; }
        Color color { get; set; }
        bool enabled { get; set; }
        Material material { get; set; }

        void SetMaterialDirty();
        T GetComponent<T>();
    }
}
