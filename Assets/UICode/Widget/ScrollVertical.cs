using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions {
    public sealed class ScrollVertical : AbsScrollLayout {
        public ScrollVertical(ScrollRect scroll, Type[] cellTypes,
            Action<ScrollRecord> onCreate, Action<ScrollRecord> onPop, Action<ScrollRecord> onPush)
            : base(scroll, cellTypes, onCreate, onPop, onPush) {
            LayoutGroup layoutGroup = m_content.GetComponent<LayoutGroup>();
            if (layoutGroup == null) {
                m_padding = new RectOffset();
                return;
            }
            if (layoutGroup is VerticalLayoutGroup) {
                VerticalLayoutGroup verLayout = layoutGroup as VerticalLayoutGroup;
                m_padding = verLayout.padding;
                m_spacing.y = verLayout.spacing;
                m_maxPerLine = 1;
                if (verLayout.childForceExpandWidth) {
                    float width = m_content.rect.width - m_padding.left - m_padding.right;
                    Scrollbar bar = scroll.verticalScrollbar;
                    if (bar != null) {
                        width -= (bar.transform as RectTransform).rect.width - scroll.verticalScrollbarSpacing * 2;
                    }
                    if (width < 0) {
                        Debug.LogError("ScrollVertical error content.width:" + width);
                    }
                    foreach (var prefab in m_prefabs) {
                        prefab.SetSize(width, -1);
                    }
                }
            }
            else if (layoutGroup is GridLayoutGroup) {
                //不支持变长
                GridLayoutGroup gridLayout = layoutGroup as GridLayoutGroup;
                m_padding = gridLayout.padding;
                m_spacing = gridLayout.spacing;
                float width = m_content.rect.width - m_padding.left - m_padding.right;
                Scrollbar bar = scroll.verticalScrollbar;
                if (bar != null) {
                    width -= (bar.transform as RectTransform).rect.width - scroll.verticalScrollbarSpacing * 2;
                }
                if (width < 0) {
                    Debug.LogError("ScrollVertical error content.width:" + width);
                }
                m_maxPerLine = Mathf.FloorToInt((width + m_spacing.x) / (gridLayout.cellSize.x + m_spacing.x));
            }
            else {
                Debug.LogError("ScrollVertical unsupport Layout:" + layoutGroup);
            }
            if (layoutGroup.enabled) {
                Debug.LogError("ScrollVertical Layout must disable:" + scroll);
            }
            GameObject.Destroy(layoutGroup);

            //Debug.Log("ScrollVertical" + m_viewRect.height + " pad:" + m_padding + " space:" + m_spacing);
        }

        protected override void SetPostionByIndex(int index, ScrollRecord record) {
            Vector2 pos;
            ScrollPrefab prefab = m_prefabs[record.prefabIndex];
            if (index >= m_maxPerLine) {
                ScrollRecord prevRecord = m_records[index - m_maxPerLine];
                ScrollPrefab prevPrefab = m_prefabs[prevRecord.prefabIndex];
                pos = prevRecord.anchoredPosition;
                pos.y -= prevRecord.size.y * prevPrefab.pivot.y + record.size.y * (1 - prefab.pivot.y) + m_spacing.y;
                record.SetAnchoredPosition(pos);
            }
            else {
                int offset = index % m_maxPerLine;
                pos = new Vector2(m_padding.left, m_padding.top);
                pos.x += record.size.x * (offset + prefab.pivot.x) + offset * m_spacing.x;
                pos.y += record.size.y * (1 - prefab.pivot.y);
                pos.y = -pos.y;
                record.SetAnchoredPosition(pos);
            }
        }

        public override void DelRecord(ScrollRecord record) {
            int index = m_records.IndexOf(record);
            if (index == -1) {
                Debug.LogError("ScrollVertical.DelRecord can't find record:" + record.id + " " + record.kind);
                return;
            }
            m_records.RemoveAt(index);
            if (record.cell != null) {
                Push(record);
            }

            ScrollRecord curRecord;
            if (m_maxPerLine == 1) {
                //格子大小可能不一致，不能直接替换为上一个的位置，需要计算得到
                float size = record.size.y + m_spacing.y;
                for (int i = index; i < m_records.Count; i++) {
                    curRecord = m_records[i];
                    Vector2 anchoredPosition = curRecord.anchoredPosition;
                    anchoredPosition.y += size;
                    curRecord.SetAnchoredPosition(anchoredPosition);
                }
                if (index <= m_headIndex) {
                    m_headIndex = m_headIndex > 0 ? m_headIndex - 1 : 0;
                    ChangeContentPosition(-size);
                }
                Build();
            }
            else {
                Vector2 prevPosition = record.anchoredPosition;
                for (int i = index; i < m_records.Count; i++) {
                    curRecord = m_records[i];
                    Vector2 anchoredPosition = curRecord.anchoredPosition;
                    curRecord.SetAnchoredPosition(prevPosition);
                    prevPosition = anchoredPosition;
                }
                if (index <= m_headIndex) {
                    OnShowPrev();
                }
                Build();
            }
        }

        public override void InsertRecord(int index, ScrollRecord record) {
            if (index > m_records.Count) {
                Debug.LogError("ScrollVertical.InsertRecord out range:" + index + " " + m_records.Count);
                return;
            }

            ScrollPrefab prefab = m_prefabs[record.prefabIndex];
            record.InitPivot(prefab.pivot);
            record.InitSize(prefab.size);
            SetPostionByIndex(index, record);
            if (index == m_records.Count) {
                m_records.Add(record);
            }
            else {
                m_records.Insert(index, record);
            }

            ScrollRecord curRecord;
            if (m_maxPerLine == 1) {
                float size = record.size.y + m_spacing.y;
                for (int i = index + 1; i < m_records.Count; i++) {
                    curRecord = m_records[i];
                    Vector2 anchoredPosition = curRecord.anchoredPosition;
                    anchoredPosition.y -= size;
                    curRecord.SetAnchoredPosition(anchoredPosition);
                }
                if (index < m_headIndex) {
                    m_headIndex = m_headIndex + 1;
                    ChangeContentPosition(size);
                }
                Build();
            }
            else {
                ScrollRecord nextRecord = null;
                if (index < m_records.Count - 1) {
                    curRecord = m_records[index + 1];
                    for (int i = index + 1; i < m_records.Count - 1; i++) {
                        nextRecord = m_records[i + 1];
                        curRecord.SetAnchoredPosition(nextRecord.anchoredPosition);
                        curRecord = nextRecord;
                    }
                }

                nextRecord = m_records[m_records.Count - 1];
                SetPostionByIndex(m_records.Count - 1, nextRecord);
                if (index < m_headIndex) {
                    OnShowPrev();
                }
                Build();
            }
        }

        public override void InsertRecords(int index, IEnumerable<ScrollRecord> records) {
            if (index > m_records.Count) {
                Debug.LogError("ScrollVertical.InsertRecords out range:" + index + " " + m_records.Count);
                return;
            }

            if (index == m_records.Count) {
                m_records.AddRange(records);
            }
            else {
                m_records.InsertRange(index, records);
            }
            float size = 0;
            int length = 0;
            foreach (var record in records) {
                ScrollPrefab prefab = m_prefabs[record.prefabIndex];
                record.InitPivot(prefab.pivot);
                record.InitSize(prefab.size);
                SetPostionByIndex(index + length, record);
                size += record.size.y + m_spacing.y;
                ++length;
            }

            ScrollRecord curRecord;
            if (m_maxPerLine == 1) {
                for (int i = index + length; i < m_records.Count; i++) {
                    curRecord = m_records[i];
                    Vector2 anchoredPosition = curRecord.anchoredPosition;
                    anchoredPosition.y -= size;
                    curRecord.SetAnchoredPosition(anchoredPosition);
                }
                if (index < m_headIndex) {
                    m_headIndex = m_headIndex + length;
                    ChangeContentPosition(size);
                }
                Build();
            }
            else {
                int start = index + length;
                int end = m_records.Count - length;
                ScrollRecord nextRecord;
                for (int i = start; i < end; i++) {
                    curRecord = m_records[i];
                    nextRecord = m_records[i + length];
                    curRecord.SetAnchoredPosition(nextRecord.anchoredPosition);
                }
                for (int i = end; i < m_records.Count; i++) {
                    curRecord = m_records[i];
                    SetPostionByIndex(i, curRecord);
                }
                if (index < m_headIndex) {
                    OnShowPrev();
                }
                Build();
            }
        }

        public override void ResizeRecord(ScrollRecord record, Vector2 size) {
            if (record.size == size) {
                return;
            }

            int index = m_records.IndexOf(record);
            if (index == -1) {
                Debug.LogError("ScrollVertical.ResizeRecord can't find record:" + record.id + " " + record.kind);
                return;
            }

            float offset = size.y - record.size.y;
            Vector2 anchoredPosition = record.anchoredPosition;
            anchoredPosition.y -= offset * (1 - record.pivot.y);
            record.SetAnchoredPosition(anchoredPosition);
            record.SetSize(size);

            ScrollRecord curRecord;
            if (m_maxPerLine == 1) {
                for (int i = index + 1; i < m_records.Count; i++) {
                    curRecord = m_records[i];
                    anchoredPosition = curRecord.anchoredPosition;
                    anchoredPosition.y -= offset;
                    curRecord.SetAnchoredPosition(anchoredPosition);
                }
                Build();
                if (index <= m_headIndex) {
                    ChangeContentPosition(offset);
                }
            }
            else {
                Debug.LogError("ScrollVertical.ResizeRecord unsupport grid");
            }
        }

        public override void ShowRecord(ScrollRecord record, float ratio, bool reset) {
            int index = m_records.IndexOf(record);
            if (index == -1) {
                Debug.LogError("ScrollVertical.ShowRecord can't find record:" + record.id + " " + record.kind);
                return;
            }

            Vector2 anchoredPosition = m_content.anchoredPosition;
            if (!reset) {
                if (anchoredPosition.y + m_viewRect.height >= -record.max.y) {
                    return;
                }
            }
            anchoredPosition.y = -record.min.y - (m_viewRect.height - record.size.y) * ratio;
            float max = m_content.rect.height - m_viewRect.height;
            if (anchoredPosition.y > max) {
                anchoredPosition.y = max;
            }
            else if (anchoredPosition.y < 0) {
                anchoredPosition.y = 0;
            }
            m_content.anchoredPosition = anchoredPosition;
            m_curPos = m_content.anchoredPosition;
        }

        private void ChangeContentPosition(float offset) {
            Vector2 anchoredPosition = m_content.anchoredPosition;
            anchoredPosition.y += offset;
            m_content.anchoredPosition = anchoredPosition;
            m_curPos = m_content.anchoredPosition;
        }

        public override void Build() {
            if (m_records.Count < 1) {
                m_showCount = 0;
                return;
            }
            Vector2 sizeDelta = m_content.sizeDelta;
            ScrollRecord lastRecord = m_records[m_records.Count - 1];
            float extent = -lastRecord.anchoredPosition.y + lastRecord.size.y * lastRecord.pivot.y + m_padding.bottom;
            sizeDelta.y = extent;
            m_content.sizeDelta = sizeDelta;
            OnShowNext();
        }

        protected override bool Drag() {
            float dist = m_lastPos.y - m_curPos.y;
            if (Mathf.Abs(dist) < kOffset) {
                return false;
            }
            if (dist < 0f) {
                OnShowNext();
            }
            else {
                OnShowPrev();
            }
            return true;
        }

        private void OnShowNext() {
            float offset = 0;
            int cursor = m_headIndex;
            int cursorNext = cursor + m_maxPerLine;
            float border = m_viewRect.height + kOffset;
            ScrollRecord record, recordInLine;
            m_showCount = 0;
            for (; cursor < m_records.Count;) {
                record = m_records[cursor];
                offset = -(record.max.y + m_curPos.y);
                if (offset < -kOffset) {
                    for (int index = cursor; index < cursorNext; index++) {
                        recordInLine = m_records[index];
                        if (recordInLine.cell != null) {
                            Push(recordInLine);
                        }
                        m_headIndex = cursorNext;
                    }
                }
                else {
                    offset = -(record.min.y + m_curPos.y);
                    if (offset < border) {
                        for (int index = cursor; index < cursorNext; index++) {
                            if (index >= m_records.Count) {
                                cursor = index;
                                break;
                            }
                            recordInLine = m_records[index];
                            if (recordInLine.cell == null) {
                                Pop(recordInLine);
                            }
                            m_showCount += 1;
                        }
                    }
                    else {
                        //超出边界
                        break;
                    }
                }
                cursor = cursorNext;
                cursorNext = cursor + m_maxPerLine;
            }
            for (; cursor < m_records.Count; cursor++) {
                record = m_records[cursor];
                if (record.cell != null) {
                    Push(record);
                }
            }
        }

        private void OnShowPrev() {
            float offset = 0;
            int cursor = m_headIndex + (m_showCount - 1) / m_maxPerLine * m_maxPerLine;
            int cursorNext = cursor + m_maxPerLine;
            float border = m_viewRect.height + kOffset;
            ScrollRecord record, recordInLine;
            m_showCount = 0;
            m_headIndex = 0;
            for (; cursor >= 0;) {
                if (cursor >= m_records.Count) {
                    cursorNext = cursor;
                    cursor = cursor - m_maxPerLine;
                    continue;
                }
                record = m_records[cursor];
                offset = -(record.min.y + m_curPos.y);
                if (offset > border) {
                    for (int index = cursor; index < cursorNext; index++) {
                        if (index >= m_records.Count) {
                            break;
                        }
                        recordInLine = m_records[index];
                        if (recordInLine.cell != null) {
                            Push(recordInLine);
                        }
                    }
                }
                else {
                    offset = -(record.max.y + m_curPos.y);
                    if (offset > -kOffset) {
                        for (int index = cursor; index < cursorNext; index++) {
                            if (index >= m_records.Count) {
                                break;
                            }
                            recordInLine = m_records[index];
                            if (recordInLine.cell == null) {
                                Pop(recordInLine);
                            }
                            m_showCount += 1;
                        }
                        m_headIndex = cursor;
                    }
                    else {
                        cursor = cursorNext - 1;
                        break;
                    }
                }
                cursorNext = cursor;
                cursor = cursor - m_maxPerLine;
            }
            for (; cursor >= 0; cursor--) {
                if (cursor >= m_records.Count) {
                    continue;
                }
                record = m_records[cursor];
                if (record.cell != null) {
                    Push(record);
                }
            }
        }
    }
}
