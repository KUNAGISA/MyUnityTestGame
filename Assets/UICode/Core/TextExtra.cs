using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI {
    public class TextExtra : Text, IText {
        [SerializeField]
        [TextArea(1, 1)]
        protected string m_LocalizeKey;
        public string localizeKey {
            get { return m_LocalizeKey; }
            set { m_LocalizeKey = value; }
        }
    }

}

