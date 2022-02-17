using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections.Generic;

public sealed class ScrollCell : Cell {
    public ScrollCell() : base() { }
    public ScrollCell(Transform inst) : base(inst) { }

    public override void Init(Transform inst) {
        m_ScrollRect = inst.GetComponent<ScrollRect>();
        if (m_ScrollRect == null) {
            Debug.LogError("UScroll ScrollRect null, name:" + inst);
        }
        base.Init(inst);
    }

    private ScrollRect m_ScrollRect;
    private bool m_initListener = false;
    private Action<Vector2> m_handler;
    private Action<Vector2, ScrollCell> m_handlerWithSelf;

    private void initListener() {
        if (m_initListener) {
            return;
        }
        m_initListener = true;
        m_ScrollRect.onValueChanged.AddListener(onValueChanged);
    }

    /// <summary>
    /// 解耦与UGUI的点击事件
    /// 方便添加通用事件，避免UnityAction未触发时不能正常清空的问题
    /// </summary>
    private void onValueChanged(Vector2 value) {
        if (isLoop) {
            m_layout.Refresh();
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
    public void SetListener(Action<Vector2> call) {
        initListener();
        m_handler = call;
        m_handlerWithSelf = null;
    }

    /// <summary>
    /// 设置点击事件回调，不支持多个回调，每次设置覆盖之前的回调
    /// </summary>
    /// <param name="call"></param>
    public void SetListener(Action<Vector2, ScrollCell> call) {
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

    public bool horizontal {
        get { return m_ScrollRect.horizontal; }
    }

    public bool enabled {
        get {
            return m_ScrollRect.enabled;
        }
        set {
            m_ScrollRect.enabled = value;
        }
    }


    public RectTransform content {
        get { return m_ScrollRect.content; }
        set { m_ScrollRect.content = value; }
    }

    public float decelerationRate {
        get { return m_ScrollRect.decelerationRate; }
        set { m_ScrollRect.decelerationRate = value; }
    }

    public float elasticity {
        get { return m_ScrollRect.elasticity; }
        set { m_ScrollRect.elasticity = value; }
    }

    public float horizontalNormalizedPosition {
        get { return m_ScrollRect.horizontalNormalizedPosition; }
        set { m_ScrollRect.horizontalNormalizedPosition = value; }
    }

    public Scrollbar horizontalScrollbar {
        get { return m_ScrollRect.horizontalScrollbar; }
        set { m_ScrollRect.horizontalScrollbar = value; }
    }

    public float horizontalScrollbarSpacing {
        get { return m_ScrollRect.horizontalScrollbarSpacing; }
        set { m_ScrollRect.horizontalScrollbarSpacing = value; }
    }

    public bool inertia {
        get { return m_ScrollRect.inertia; }
        set { m_ScrollRect.inertia = value; }
    }

    public Vector2 normalizedPosition {
        get { return m_ScrollRect.normalizedPosition; }
        set { m_ScrollRect.normalizedPosition = value; }
    }

    public float scrollSensitivity {
        get { return m_ScrollRect.scrollSensitivity; }
        set { m_ScrollRect.scrollSensitivity = value; }
    }

    public Vector2 velocity {
        get { return m_ScrollRect.velocity; }
        set { m_ScrollRect.velocity = value; }
    }

    public float verticalNormalizedPosition {
        get { return m_ScrollRect.verticalNormalizedPosition; }
        set { m_ScrollRect.verticalNormalizedPosition = value; }
    }

    public Scrollbar verticalScrollbar {
        get { return m_ScrollRect.verticalScrollbar; }
        set { m_ScrollRect.verticalScrollbar = value; }
    }

    public float verticalScrollbarSpacing {
        get { return m_ScrollRect.verticalScrollbarSpacing; }
        set { m_ScrollRect.verticalScrollbarSpacing = value; }
    }

    public RectTransform viewport {
        get { return m_ScrollRect.viewport; }
        set { m_ScrollRect.viewport = value; }
    }

    public void TrunTo(AbsCell item) {
        RectTransform itemXform = item.rectTransform;
        if (m_ScrollRect.horizontal) {
            float nodeWidth = viewport.rect.width;
            float contentWidth = content.rect.width;
            float x = nodeWidth * rectTransform.pivot.x - itemXform.rect.width * itemXform.pivot.x + itemXform.anchoredPosition.x;
            if (nodeWidth > contentWidth) {
                this.content.SetAnchoredX(x);
            }
            else {
                this.content.SetAnchoredX(-x);
            }
        }
        else {
            float nodeHeight = viewport.rect.height;
            float contentHeight = content.rect.height;
            float y = nodeHeight * rectTransform.pivot.y - itemXform.rect.height * (1 - itemXform.pivot.y) - itemXform.anchoredPosition.y;
            if (nodeHeight > contentHeight) {
                this.content.SetAnchoredY(y);
            }
            else {
                this.content.SetAnchoredY(-y);
            }
        }
    }

    public bool isLoop { get; private set; }

    private AbsScrollLayout m_layout;

    /// <summary>
    /// 开启无限滚动循环
    /// 支持多个子节点预制件，ScrollRecord.prefabIndex中设置
    /// viewport不支持有偏移值，在layout中设置
    /// </summary>
    /// <param name="cellType">子节点类型，需继承自AbsCell</param>
    /// <param name="onCreate">生成回调</param>
    /// <param name="onPop">出现回调</param>
    /// <param name="onPush">消失回调</param>
    public void InitLoop(Type cellType, Action<ScrollRecord> onCreate, Action<ScrollRecord> onPop, Action<ScrollRecord> onPush) {
        InitLoop(new Type[] { cellType }, onCreate, onPop, onPush);
    }

    public void InitLoop(Type[] cellTypes, Action<ScrollRecord> onCreate, Action<ScrollRecord> onPop, Action<ScrollRecord> onPush) {
        if (isLoop) {
            Debug.LogError("UScroll InitLoop repeat");
            return;
        }
        isLoop = true;
        initListener();

        if (m_ScrollRect.vertical) {
            m_ScrollRect.horizontal = false;
            m_layout = new ScrollVertical(m_ScrollRect, cellTypes, onCreate, onPop, onPush);
        }
        else {
            m_ScrollRect.vertical = false;
            m_layout = new ScrollHorizontal(m_ScrollRect, cellTypes, onCreate, onPop, onPush);
        }
    }

    private RectOffset m_padding;
    public RectOffset padding {
        get {
            if (m_padding == null) {
                LayoutGroup layoutGroup = content.GetComponent<LayoutGroup>();
                if (layoutGroup != null) {
                    m_padding = layoutGroup.padding;
                }
                else {
                    m_padding = new RectOffset();
                }
            }
            return m_padding;
        }
    }

    public int recordCount {
        get {
            if (m_layout == null) {
                return 0;
            }
            return m_layout.records.Count;
        }
    }

    public int headIndex {
        get {
            if (m_layout == null) {
                return 0;
            }
            return m_layout.headIndex;
        }
    }

    public int showCount {
        get {
            if (m_layout == null) {
                return 0;
            }
            return m_layout.showCount;
        }
    }

    public ScrollRecord GetRecord(int index) {
        if (index < 0 || index >= m_layout.records.Count) {
            Debug.LogError("ScrollLoop.GetRecord:" + index + " out range:" + m_layout.records.Count);
            return null;
        }
        ScrollRecord record = m_layout.records[index];
        return record;
    }

    public ScrollRecord GetLastRecord(int index) {
        int idx = m_layout.records.Count - 1 - index;
        if (idx < 0) {
            Debug.LogError("ScrollLoop.GetLastRecord: index < 0 " + index + " count:" + m_layout.records.Count);
            return null;
        }
        ScrollRecord record = m_layout.records[idx];
        return record;
    }

    public ScrollRecord FindRecord(int id) {
        for (int i = 0; i < m_layout.records.Count; i++) {
            ScrollRecord record = m_layout.records[i];
            if (record.id == id) {
                return record;
            }
        }
        Debug.LogError("FindRecord fail, id:" + id);
        return null;
    }

    public bool TryGetScord(int id, out ScrollRecord record) {
        record = null;
        for (int i = 0; i < m_layout.records.Count; i++) {
            var recordTmp = m_layout.records[i];
            if (recordTmp.id == id) {
                record = recordTmp;
                return true;
            }
        }
        return false;
    }

    public ScrollRecord FindRecord(int id, int kind) {
        for (int i = 0; i < m_layout.records.Count; i++) {
            ScrollRecord record = m_layout.records[i];
            if (record.id == id && record.kind == kind) {
                return record;
            }
        }
        Debug.LogError("ScrollLoop.FindRecord fail, id:" + id + " kind:" + kind);
        return null;
    }

    public bool TryGetRecord(int id, int kind, out ScrollRecord record) {
        for (int i = 0; i < m_layout.records.Count; i++) {
            var recordTmp = m_layout.records[i];
            if (recordTmp.id == id && recordTmp.kind == kind) {
                record = recordTmp;
                return true;
            }
        }
        record = null;
        return false;
    }

    public bool TryGetScordByUid(long uid, out ScrollRecord record) {
        record = null;
        for (int i = 0; i < m_layout.records.Count; i++) {
            var recordTmp = m_layout.records[i];
            if (recordTmp.uid == uid) {
                record = recordTmp;
                return true;
            }
        }
        return false;
    }
    public bool TryGetScordByUid(long uid, int kind, out ScrollRecord record) {
        record = null;
        for (int i = 0; i < m_layout.records.Count; i++) {
            var recordTmp = m_layout.records[i];
            if (recordTmp.uid == uid && recordTmp.kind == kind) {
                record = recordTmp;
                return true;
            }
        }
        return false;
    }
    public ScrollRecord FindRecord(object data) {
        for (int i = 0; i < m_layout.records.Count; i++) {
            ScrollRecord record = m_layout.records[i];
            if (record.data == data) {
                return record;
            }
        }
        Debug.LogError("FindRecord fail, data:" + data);
        return null;
    }

    public ScrollRecord FindRecordByUid(long uid) {
        for (int i = 0; i < m_layout.records.Count; i++) {
            ScrollRecord record = m_layout.records[i];
            if (record.uid == uid) {
                return record;
            }
        }
        Debug.LogError("FindRecord fail, uid:" + uid);
        return null;
    }

    public int IndexOf(ScrollRecord record) {
        if (record == null) {
            Debug.LogError("ScrollLoop.FindIndex null");
            return -1;
        }
        return m_layout.records.IndexOf(record);
    }

    public void AddRecord(ScrollRecord record) {
        if (record == null) {
            Debug.LogError("ScrollLoop.AddRecord null");
            return;
        }
        m_layout.AddRecord(record);
    }

    public void AddRecords(IEnumerable<ScrollRecord> records) {
        if (records == null) {
            Debug.LogError("ScrollLoop.AddRecords null");
            return;
        }
        m_layout.AddRecords(records);
    }

    public void DelRecordsByKind(int kind) {
        //Debug.Log("DelRecordsKind    " + m_layout.records.Count);
        List<ScrollRecord> records = new List<ScrollRecord>();
        for (int i = 0; i < m_layout.records.Count; i++) {
            ScrollRecord record = m_layout.records[i];
            if (record.kind == kind) {
                records.Add(record);
            }
        }
        foreach (var t in records) {
            m_layout.DelRecord(t);
        }
    }

    public void DelRecordsByKindAndPrefabIndex(int kind, int prefabIndex) {
        //Debug.Log("DelRecordsKind    " + m_layout.records.Count);
        List<ScrollRecord> records = new List<ScrollRecord>();
        for (int i = 0; i < m_layout.records.Count; i++) {
            ScrollRecord record = m_layout.records[i];
            if (record.kind == kind && record.prefabIndex == prefabIndex) {
                records.Add(record);
            }
        }
        foreach (var t in records) {
            m_layout.DelRecord(t);
        }
    }

    public List<ScrollRecord> FindRecordsByKind(int kind) {
        List<ScrollRecord> records = new List<ScrollRecord>();
        for (int i = 0; i < m_layout.records.Count; i++) {
            ScrollRecord record = m_layout.records[i];
            if (record.kind == kind) {
                records.Add(record);
            }
        }
        return records;
    }

    public List<ScrollRecord> FindRecordsByKind(int kind, int prefabIndex) {
        List<ScrollRecord> records = new List<ScrollRecord>();
        for (int i = 0; i < m_layout.records.Count; i++) {
            ScrollRecord record = m_layout.records[i];
            if (record.kind == kind && record.prefabIndex == prefabIndex) {
                records.Add(record);
            }
        }
        return records;
    }

    public List<ScrollRecord> FindRecordsByPrefabIndex(int prefabIndex) {
        List<ScrollRecord> records = new List<ScrollRecord>();
        for (int i = 0; i < m_layout.records.Count; i++) {
            ScrollRecord record = m_layout.records[i];
            if (record.prefabIndex == prefabIndex) {
                records.Add(record);
            }
        }
        return records;
    }

    public void moveSort(int start, int end) {
        if (end <= start) {
            return;
        }
        for (int i = 0; i < m_layout.records.Count; i++) {
            ScrollRecord record = m_layout.records[i];
            if (record.id > start && record.id < end) {
                record.id -= 1;
            }
        }
    }

    public void DelRecord(ScrollRecord record) {
        if (record == null) {
            Debug.LogError("ScrollLoop.DelRecord null");
            return;
        }
        m_layout.DelRecord(record);
    }

    public void InsertRecord(int index, ScrollRecord record) {
        if (record == null) {
            Debug.LogError("ScrollLoop.InsertRecord null");
            return;
        }
        m_layout.InsertRecord(index, record);
    }

    public void InsertRecords(int index, IEnumerable<ScrollRecord> records) {
        if (records == null) {
            Debug.LogError("ScrollLoop.InsertRecords null");
            return;
        }
        m_layout.InsertRecords(index, records);
    }

    /// <summary>
    /// 更新指定项的大小
    /// </summary>
    /// <param name="record"></param>
    /// <param name="size">x,y只需要修改一个时，另一个设置为-1就会使用当前大小</param>
    public void ResizeRecord(ScrollRecord record, Vector2 size) {
        if (record == null) {
            Debug.LogError("ScrollLoop.ResizeRecord null");
            return;
        }
        m_layout.ResizeRecord(record, size);
    }

    /// <summary>
    /// 设置指定的记录到显示位置
    /// </summary>
    /// <param name="record"></param>
    /// <param name="ratio">在viewport中的位置比例,0左上 1右下</param>
    /// <param name="reset">即使已经在显示区域，也重新设置位置</param>
    public void ShowRecord(ScrollRecord record, float ratio, bool reset) {
        if (record == null) {
            Debug.LogError("ScrollLoop.ShowRecord null");
            return;
        }
        m_layout.ShowRecord(record, ratio, reset);
    }

    public void SortRecord(Comparison<ScrollRecord> cmp) {
        if (cmp == null) {
            Debug.LogError("ScrollLoop.SortRecord null");
            return;
        }
        m_layout.SortRecord(cmp);
    }

    public void Build() {
        m_layout.Build();
    }

    public void Clear() {
        m_layout.Clear();
    }

    public void Refresh() {
        if (isLoop) {
            m_layout.Refresh();
        }
    }
}
