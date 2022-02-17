using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions {
    /// <summary>
    /// 只支持在Anchors左上角的预制件，即anchorMin=anchorMax=[0,1]
    /// 只支持从左向右，从上向下拖拽
    /// </summary>
    public class ScrollPrefab {
        public RectTransform rectTransform { get; private set; }
        public Vector2 size { get; private set; }
        public Vector2 pivot { get; private set; }
        public ScrollPrefab(Transform transform) {
            rectTransform = transform as RectTransform;
            size = rectTransform.sizeDelta;
            pivot = rectTransform.pivot;
        }

        public void SetSize(float x, float y) {
            Vector2 v = size;
            if (x > 0) {
                v.x = x;
            }
            if (y > 0) {
                v.y = y;
            }
            size = v;
            rectTransform.sizeDelta = v;
        }
    }

    public class ScrollRecord {
        public ScrollRecord(int id) {
            this.id = id;
        }

        public ScrollRecord(int id, int kind) {
            this.id = id;
            this.kind = kind;
        }

        public ScrollRecord(int id, int kind, int prefabIndex) {
            this.id = id;
            this.kind = kind;
            this.prefabIndex = prefabIndex;
        }

        public void Init(int id, int kind, int prefabIndex) {
            this.id = id;
            this.kind = kind;
            this.prefabIndex = prefabIndex;
        }

        public int id {
            get;
            set;
        }

        public int kind {
            get;
            set;
        }

        public int prefabIndex {
            get;
            protected set;
        }

        public AbsCell cell {
            get;
            protected set;
        }

        private bool m_sizeInited;
        private Vector2 m_size;
        public Vector2 size {
            get {
                if (!m_sizeInited) {
                    return Vector2.zero;
                }
                return m_size;
            }
        }

        public Vector2 pivot {
            get;
            protected set;
        }

        private bool m_anchoredPositionInited;
        private Vector2 m_anchoredPosition;
        public Vector2 anchoredPosition {
            get {
                if (!m_anchoredPositionInited) {
                    return Vector2.zero;
                }
                return m_anchoredPosition;
            }
        }

        private Vector2 m_min;
        public Vector2 min {
            get { return m_min; }
        }

        private Vector2 m_max;
        public Vector2 max {
            get { return m_max; }
        }

        public int index;
        public object data;
        public long uid;

        public void InitPrefabIndex(int index) {
            this.prefabIndex = index;
        }

        public void InitPivot(Vector2 pivot) {
            this.pivot = pivot;
        }

        /// <summary>
        /// 初始化大小，内部使用
        /// </summary>
        /// <param name="size"></param>
        internal void InitSize(Vector2 size) {
            if (m_sizeInited) {
                if (m_size.x > 0) {
                    size.x = m_size.x;
                }
                if (m_size.y > 0) {
                    size.y = m_size.y;
                }
            }
            m_sizeInited = true;
            m_size = size;
        }

        private void CalculateRange() {
            m_min.x = anchoredPosition.x - size.x * pivot.x;
            m_min.y = anchoredPosition.y + size.y * (1 - pivot.y);
            m_max.x = anchoredPosition.x + size.x * (1 - pivot.x);
            m_max.y = anchoredPosition.y - size.y * pivot.y;
        }

        public void SetCell(AbsCell cell) {
            this.cell = cell;
            if (cell != null && cell.rectTransform.sizeDelta != size) {
                cell.rectTransform.sizeDelta = size;
            }
        }

        /// <summary>
        /// 设置大小
        /// x,y只需要设置一个时，另一个设置为-1就会使用默认大小
        /// </summary>
        /// <param name="size"></param>
        public void SetSize(Vector2 size) {
            if (m_sizeInited) {
                if (size.x <= 0) {
                    size.x = m_size.x;
                }
                if (size.y <= 0) {
                    size.y = m_size.y;
                }
            }
            m_sizeInited = true;
            m_size = size;
            if (m_anchoredPositionInited) {
                CalculateRange();
            }
            if (cell != null && cell.rectTransform.sizeDelta != size) {
                cell.rectTransform.sizeDelta = size;
            }
        }

        /// <summary>
        /// 设置位置，外部一般不调用
        /// </summary>
        /// <param name="anchoredPosition"></param>
        public void SetAnchoredPosition(Vector2 anchoredPosition) {
            m_anchoredPositionInited = true;
            m_anchoredPosition = anchoredPosition;
            if (m_sizeInited) {
                CalculateRange();
            }
            if (cell != null) {
                cell.rectTransform.anchoredPosition = anchoredPosition;
            }
        }

        public override string ToString() {
            string str = string.Format("<ScrollRecord id:{0} kind:{1} prefab:{2} pos:{3} size:{4} data:{5}>",
                id, kind, prefabIndex, anchoredPosition, size, data
            );
            return str;
        }
    }

    /// <summary>
    /// 无限滚动布局抽象类
    /// 禁止设置viewport，content的偏移大小，只能在Layout中设置padding
    /// </summary>
    public abstract class AbsScrollLayout {
        protected const float kOffset = 15;
        private static Vector2 kHidePos = new Vector2(-1000, 1000);

        private Type[] m_cellTypes;
        protected Action<ScrollRecord> m_onCellCreate;
        protected Action<ScrollRecord> m_onCellPop;
        protected Action<ScrollRecord> m_onCellPush;

        public AbsScrollLayout(ScrollRect scroll, Type[] cellTypes, 
            Action<ScrollRecord> onCreate, Action<ScrollRecord> onPop, Action<ScrollRecord> onPush) {
            m_cellTypes = cellTypes;
            m_onCellCreate = onCreate;
            m_onCellPop = onPop;
            m_onCellPush = onPush;

            m_rectTransform = scroll.transform as RectTransform;
            m_content = scroll.content;
            m_viewRect = m_rectTransform.rect;

            int prefabCount = m_content.childCount;
            if (prefabCount != m_cellTypes.Length) {
                Debug.LogErrorFormat("cellTypes not equal prefab count! " + m_cellTypes.Length + "/" + prefabCount);
            }
            m_prefabs = new ScrollPrefab[prefabCount];
            m_pools = new Stack<AbsCell>[prefabCount];
            for (int i = 0; i < prefabCount; i++) {
                RectTransform child = m_content.GetChild(i) as RectTransform;
                if (child == null || !child.gameObject.activeSelf) {
                    continue;
                }
                child.gameObject.SetActive(false);
                child.anchoredPosition = kHidePos;
                ScrollPrefab prefab = new ScrollPrefab(child);
                m_prefabs[i] = prefab;
                m_pools[i] = new Stack<AbsCell>();
            }

            m_curPos = m_content.anchoredPosition;
            m_lastPos = m_curPos;
        }

        protected RectTransform m_rectTransform;
        protected RectTransform m_content;
        protected Rect m_viewRect;
        protected RectOffset m_padding;
        protected Vector2 m_spacing;

        protected int m_headIndex;
        protected int m_showCount = 0;

        protected int m_maxPerLine = 1;
        protected ScrollPrefab[] m_prefabs;
        private Stack<AbsCell>[] m_pools;
        protected List<ScrollRecord> m_records = new List<ScrollRecord>(30);

        protected Vector2 m_curPos;
        protected Vector2 m_lastPos;

        public int headIndex {
            get { return m_headIndex; }
        }

        public int showCount {
            get { return m_showCount; }
        }

        public List<ScrollRecord> records {
            get {
                return m_records;
            }
        }

        private bool m_dynamic = false;
        public void SetDynamic(bool enable) {
            m_dynamic = enable;
        }

        public void Refresh() {
            if (m_showCount <= 0) {
                return;
            }
            m_curPos = m_content.anchoredPosition;
            if (m_showCount >= m_records.Count) {
                m_lastPos = m_curPos;
                return;
            }
            if (m_lastPos == m_curPos) {
                return;
            }
            if (m_dynamic) {
                m_viewRect = m_rectTransform.rect;
            }
            if (Drag()) {
                m_lastPos = m_curPos;
            }
        }

        protected abstract bool Drag();

        public abstract void Build();

        public void Clear() {
            int max = m_headIndex + m_showCount;
            for (int i = m_headIndex; i < max; i++) {
                ScrollRecord record = m_records[i];
                Push(record);
            }
            m_headIndex = 0;
            m_showCount = 0;
            m_records.Clear();
            m_curPos = Vector2.zero;
            m_lastPos = Vector2.zero;
            m_content.anchoredPosition = Vector2.zero;
        }

        protected abstract void SetPostionByIndex(int index, ScrollRecord record);

        public void AddRecord(ScrollRecord record) {
            ScrollPrefab prefab = m_prefabs[record.prefabIndex];
            record.InitPivot(prefab.pivot);
            record.InitSize(prefab.size);

            int index = m_records.Count;
            SetPostionByIndex(index, record);
            m_records.Add(record);
        }

        public void AddRecords(IEnumerable<ScrollRecord> records) {
            foreach (var record in records) {
                AddRecord(record);
            }
        }

        public abstract void DelRecord(ScrollRecord record);

        public abstract void InsertRecord(int index, ScrollRecord record);

        public abstract void InsertRecords(int index, IEnumerable<ScrollRecord> records);

        public abstract void ResizeRecord(ScrollRecord record, Vector2 size);

        public abstract void ShowRecord(ScrollRecord record, float ratio, bool reset);

        public void SortRecord(Comparison<ScrollRecord> cmp) {
            int max = m_headIndex + m_showCount;
            for (int i = m_headIndex; i < max; i++) {
                ScrollRecord record = m_records[i];
                Push(record);
            }
            m_headIndex = 0;
            m_showCount = 0;
            m_curPos = Vector2.zero;
            m_lastPos = Vector2.zero;
            m_content.anchoredPosition = Vector2.zero;
            m_records.Sort(cmp);
            for (int i = 0; i < m_records.Count; i++) {
                SetPostionByIndex(i, m_records[i]);
            }
            Build();
        }

        protected AbsCell Pop(ScrollRecord record) {
            int prefabIndex = record.prefabIndex;
            if (prefabIndex >= m_pools.Length || prefabIndex < 0) {
                Debug.LogError("ScrollLoopPool.Pop prefabIndex:" + prefabIndex + " out range:" + m_pools.Length);
                return null;
            }
            Stack<AbsCell> stack = m_pools[prefabIndex];
            AbsCell cell;
            if (stack.Count <= 0) {
                ScrollPrefab prefab = m_prefabs[prefabIndex];
                RectTransform inst = Object.Instantiate(prefab.rectTransform);
                var value_type = m_cellTypes[prefabIndex];
#if ILRuntime
                if (value_type is ILRuntime.Reflection.ILRuntimeType runtimeType) {
                    var obj = runtimeType.ILType.Instantiate();
                    cell = obj.CLRInstance as AbsCellAdaptor.Adaptor;
                }
                else {
                    if (value_type is ILRuntime.Reflection.ILRuntimeWrapperType wrapperType)
                        value_type = wrapperType.RealType;
                    cell = Activator.CreateInstance(value_type) as AbsCell;
                }
#else
                cell = Activator.CreateInstance(value_type) as AbsCell;
#endif
                cell.Init(inst);
                record.SetCell(cell);
                cell.rectTransform.SetParent(m_content);
                cell.rectTransform.localScale = Vector3.one;
                cell.rectTransform.anchoredPosition3D = record.anchoredPosition;
                cell.rectTransform.gameObject.SetActive(true);
                m_onCellCreate?.Invoke(record);
            }
            else {
                cell = stack.Pop();
                cell.rectTransform.anchoredPosition3D = record.anchoredPosition;
                record.SetCell(cell);
            }

            m_onCellPop?.Invoke(record);
            return cell;
        }

        protected void Push(ScrollRecord record) {
            m_onCellPush?.Invoke(record);
            int prefabIndex = record.prefabIndex;
            Stack<AbsCell> stack = m_pools[prefabIndex];
            stack.Push(record.cell);
            record.cell.rectTransform.anchoredPosition = kHidePos;
            record.SetCell(null);
        }
    }
}
