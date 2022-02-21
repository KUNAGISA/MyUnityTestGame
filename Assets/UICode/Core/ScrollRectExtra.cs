using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.UI {
    public sealed class ScrollRectExtra : ScrollRect {

        public GameObject upthrowGo;

        private bool m_UpthrowEvent = false;


        public override void OnBeginDrag(PointerEventData eventData) {

            if (upthrowGo) {

                var dragHorizontal = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);
                m_UpthrowEvent = dragHorizontal != horizontal;
                if (m_UpthrowEvent) {
                    //当前操作方向不等于滑动方向，将事件传给父对象
                    ExecuteEvents.Execute(upthrowGo, eventData, ExecuteEvents.beginDragHandler);
                    return;
                }
            }

            base.OnBeginDrag(eventData);

        }

        public override void OnScroll(PointerEventData data) {
            if (upthrowGo) {
                if (m_UpthrowEvent) {
                    ExecuteEvents.Execute(upthrowGo, data, ExecuteEvents.scrollHandler);
                    return;
                }
            }
            base.OnScroll(data);

        }

        public override void OnEndDrag(PointerEventData eventData) {
            if (upthrowGo) {
                if (m_UpthrowEvent) {
                    ExecuteEvents.Execute(upthrowGo, eventData, ExecuteEvents.endDragHandler);
                    m_UpthrowEvent = false;
                    return;
                }
            }
            base.OnEndDrag(eventData);

        }

        public override void OnDrag(PointerEventData eventData) {
            if (upthrowGo) {
                if (m_UpthrowEvent) {
                    ExecuteEvents.Execute(upthrowGo, eventData, ExecuteEvents.dragHandler);
                    return;
                }
            }
            base.OnDrag(eventData);

        }

    }
}

