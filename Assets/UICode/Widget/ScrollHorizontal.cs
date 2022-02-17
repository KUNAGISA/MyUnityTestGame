using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI.Extensions {
    public sealed class ScrollHorizontal : AbsScrollLayout {
        public ScrollHorizontal(ScrollRect scroll, Type[] cellTypes, 
            Action<ScrollRecord> onCreate, Action<ScrollRecord> onPop, Action<ScrollRecord> onPush)
            : base(scroll, cellTypes, onCreate, onPop, onPush) {
            LayoutGroup layoutGroup = m_content.GetComponent<LayoutGroup>();
            if (layoutGroup == null) {
                m_padding = new RectOffset();
                return;
            }
            if (layoutGroup is HorizontalLayoutGroup) {
                HorizontalLayoutGroup horLayout = layoutGroup as HorizontalLayoutGroup;
                m_padding = horLayout.padding;
                m_spacing.x = horLayout.spacing;
                m_maxPerLine = 1;
                if (horLayout.childForceExpandHeight) {
                    float height = m_content.rect.height - m_padding.top - m_padding.bottom;
                    Scrollbar bar = scroll.horizontalScrollbar;
                    if (bar != null) {
                        height -= (bar.transform as RectTransform).rect.height - scroll.horizontalScrollbarSpacing * 2;
                    }
                    if (height < 0) {
                        Debug.LogError("ScrollHorizontal error content.height:" + height);
                    }
                    foreach (var prefab in m_prefabs) {
                        prefab.SetSize(-1, height);
                    }
                }
            }
            else if (layoutGroup is GridLayoutGroup) {
                //不支持变长
                GridLayoutGroup gridLayout = layoutGroup as GridLayoutGroup;
                m_padding = gridLayout.padding;
                m_spacing = gridLayout.spacing;
                float height = m_content.rect.height - m_padding.top - m_padding.bottom;
                Scrollbar bar = scroll.horizontalScrollbar;
                if (bar != null) {
                    height -= (bar.transform as RectTransform).rect.height - scroll.horizontalScrollbarSpacing * 2;
                }
                if (height < 0) {
                    Debug.LogError("ScrollHorizontal error content.height:" + height);
                }
                m_maxPerLine = Mathf.FloorToInt((height + m_spacing.y) / (gridLayout.cellSize.y + m_spacing.y));
            }
            else {
                Debug.LogError("ScrollHorizontal unsupport Layout:" + layoutGroup);
            }
            if (layoutGroup.enabled) {
                Debug.LogError("ScrollHorizontal Layout must disable:" + scroll);
            }
            GameObject.Destroy(layoutGroup);
        }

        protected override void SetPostionByIndex(int index, ScrollRecord record) {
            Vector2 pos;
            ScrollPrefab prefab = m_prefabs[record.prefabIndex];
            if (index >= m_maxPerLine) {
                ScrollRecord prevRecord = m_records[index - m_maxPerLine];
                ScrollPrefab prevPrefab = m_prefabs[prevRecord.prefabIndex];
                pos = prevRecord.anchoredPosition;
                pos.x += prevRecord.size.x * (1 - prevPrefab.pivot.x) + record.size.x * prefab.pivot.x + m_spacing.x;
                record.SetAnchoredPosition(pos);
            }
            else {
                int offset = index % m_maxPerLine;
                pos = new Vector2(m_padding.left, m_padding.top);
                pos.x += record.size.x * prefab.pivot.x;
                pos.y += record.size.y * (offset + 1 - prefab.pivot.y) + offset * m_spacing.y;
                pos.y = -pos.y;
                record.SetAnchoredPosition(pos);
            }
        }

        public override void DelRecord(ScrollRecord record) {
            int index = m_records.IndexOf(record);
            if (index == -1) {
                Debug.LogError("ScrollHorizontal.DelRecord can't find record:" + record.id + " " + record.kind);
                return;
            }
            m_records.RemoveAt(index);
            if (record.cell != null) {
                Push(record);
            }

            ScrollRecord curRecord;
            if (m_maxPerLine == 1) {
                //格子大小可能不一致，不能直接替换为上一个的位置，需要计算得到
                float size = record.size.x + m_spacing.x;
                for (int i = index; i < m_records.Count; i++) {
                    curRecord = m_records[i];
                    Vector2 anchoredPosition = curRecord.anchoredPosition;
                    anchoredPosition.x -= size;
                    curRecord.SetAnchoredPosition(anchoredPosition);
                }
                if (index <= m_headIndex) {
                    m_headIndex = m_headIndex > 0 ? m_headIndex - 1 : 0;
                    ChangeContentPosition(size);
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
                if (index <= this.m_headIndex) {
                    OnShowPrev();
                }
                Build();
            }
        }
        public override void InsertRecord(int index, ScrollRecord record) {
            if (index > m_records.Count) {
                Debug.LogError("ScrollHorizontal.InsertRecord out range:" + index + " " + m_records.Count);
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
                float size = record.size.x + m_spacing.x;
                for (int i = index + 1; i < m_records.Count; i++) {
                    curRecord = m_records[i];
                    Vector2 anchoredPosition = curRecord.anchoredPosition;
                    anchoredPosition.x += size;
                    curRecord.SetAnchoredPosition(anchoredPosition);
                }
                if (index <= m_headIndex) {
                    m_headIndex = m_headIndex + 1;
                    ChangeContentPosition(-size);
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
                if (index < this.m_headIndex) {
                    OnShowPrev();
                }
                Build();
            }
        }

        public override void InsertRecords(int index, IEnumerable<ScrollRecord> records) {
            if (index > m_records.Count) {
                Debug.LogError("ScrollHorizontal.InsertRecords out range:" + index + " " + m_records.Count);
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
                size += record.size.x + m_spacing.x;
                ++length;
            }

            ScrollRecord curRecord;
            if (m_maxPerLine == 1) {                
                for (int i = index + length; i < m_records.Count; i++) {
                    curRecord = m_records[i];
                    Vector2 anchoredPosition = curRecord.anchoredPosition;
                    anchoredPosition.x += size;
                    curRecord.SetAnchoredPosition(anchoredPosition);
                }
                if (index <= m_headIndex) {
                    m_headIndex = m_headIndex + length;
                    ChangeContentPosition(-size);
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
                if (index < this.m_headIndex) {
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
                Debug.LogError("ScrollHorizontal.ResizeRecord can't find record:" + record.id + " " + record.kind);
                return;
            }

            float offset = size.x - record.size.x;
            Vector2 anchoredPosition = record.anchoredPosition;
            anchoredPosition.x += offset * record.pivot.x;
            record.SetAnchoredPosition(anchoredPosition);
            record.SetSize(size);

            ScrollRecord curRecord;
            if (m_maxPerLine == 1) {
                for (int i = index + 1; i < m_records.Count; i++) {
                    curRecord = m_records[i];
                    anchoredPosition = curRecord.anchoredPosition;
                    anchoredPosition.x += offset;
                    curRecord.SetAnchoredPosition(anchoredPosition);
                }
                Build();
                if (index <= m_headIndex) {
                    ChangeContentPosition(-offset);
                }
            }
            else {
                Debug.LogError("ScrollHorizontal.ResizeRecord unsupport grid");
            }
        }

        public override void ShowRecord(ScrollRecord record, float ratio, bool reset) {
            int index = m_records.IndexOf(record);
            if (index == -1) {
                Debug.LogError("ScrollHorizontal.ShowRecord can't find record:" + record.id + " " + record.kind);
                return;
            }

            Vector2 anchoredPosition = m_content.anchoredPosition;
            if (!reset) {
                if (anchoredPosition.x + m_viewRect.width >= -record.max.x) {
                    return;
                }
            }
            anchoredPosition.x = -record.min.x + (m_viewRect.width - record.size.x) * ratio;
            float max = m_content.rect.width - m_viewRect.width;
            if (-anchoredPosition.x > max) {
                anchoredPosition.x = -max;
            }
            else if (anchoredPosition.x > 0) {
                anchoredPosition.x = 0;
            }
            m_content.anchoredPosition = anchoredPosition;
            m_curPos = m_content.anchoredPosition;
        }

        private void ChangeContentPosition(float offset) {
            Vector2 anchoredPosition = m_content.anchoredPosition;
            anchoredPosition.x += offset;
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
            float extent = lastRecord.anchoredPosition.x + lastRecord.size.x * (1 - lastRecord.pivot.x) + m_padding.right;
            sizeDelta.x = extent;
            m_content.sizeDelta = sizeDelta;
            OnShowNext();
        }


        protected override bool Drag() {
            float dist = m_lastPos.x - m_curPos.x;
            if (Mathf.Abs(dist) < kOffset) {
                return false;
            }
            if (dist > 0f) {
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
            float border = m_viewRect.width + kOffset;
            ScrollRecord record, recordInLine;
            m_showCount = 0;
            for (; cursor < m_records.Count;) {
                record = m_records[cursor];
                offset = record.max.x + m_curPos.x;
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
                    offset = record.min.x + m_curPos.x;
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
            float border = m_viewRect.width + kOffset;
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
                offset = record.min.x + m_curPos.x;
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
                    offset = record.max.x + m_curPos.x;
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
