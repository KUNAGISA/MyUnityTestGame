using System;

namespace UnityEngine.UI {
    /// <summary>
    /// Text and TextMeshPro interface
    /// </summary>
    public interface IText {
        /// <summary>
        /// 本地化的唯一标示，空标示不需要本地化
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
