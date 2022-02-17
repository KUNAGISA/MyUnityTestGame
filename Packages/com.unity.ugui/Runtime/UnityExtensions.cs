
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class UnityExtensions {
    public static void SetX(this Transform t, float x) {
        Vector3 position = t.position;
        position.x = x;
        t.position = position;
    }

    public static void SetY(this Transform t, float y) {
        Vector3 position = t.position;
        position.y = y;
        t.position = position;
    }

    public static void SetZ(this Transform t, float z) {
        Vector3 position = t.position;
        position.z = z;
        t.position = position;
    }

    public static void SetXY(this Transform t, float x, float y) {
        Vector3 position = t.position;
        position.x = x;
        position.y = y;
        t.position = position;
    }

    public static void SetXZ(this Transform t, float x, float z) {
        Vector3 position = t.position;
        position.x = x;
        position.z = z;
        t.position = position;
    }

    public static void SetYZ(this Transform t, float y, float z) {
        Vector3 position = t.position;
        position.y = y;
        position.z = z;
        t.position = position;
    }

    public static void SetLocalX(this Transform t, float x) {
        Vector3 position = t.localPosition;
        position.x = x;
        t.localPosition = position;
    }

    public static void SetLocalY(this Transform t, float y) {
        Vector3 position = t.localPosition;
        position.y = y;
        t.localPosition = position;
    }

    public static void SetLocalZ(this Transform t, float z) {
        Vector3 position = t.localPosition;
        position.z = z;
        t.localPosition = position;
    }

    public static void SetLocalXY(this Transform t, float x, float y) {
        Vector3 position = t.localPosition;
        position.x = x;
        position.y = y;
        t.localPosition = position;
    }

    public static void SetLocalXZ(this Transform t, float x, float z) {
        Vector3 position = t.localPosition;
        position.x = x;
        position.z = z;
        t.localPosition = position;
    }

    public static void SetLocalYZ(this Transform t, float y, float z) {
        Vector3 position = t.localPosition;
        position.y = y;
        position.z = z;
        t.localPosition = position;
    }

    public static void SetLocalScaleX(this Transform t, float x) {
        Vector3 position = t.localScale;
        position.x = x;
        t.localScale = position;
    }

    public static void SetLocalScaleY(this Transform t, float y) {
        Vector3 position = t.localScale;
        position.y = y;
        t.localScale = position;
    }

    public static void SetLocalScaleZ(this Transform t, float z) {
        Vector3 position = t.localScale;
        position.z = z;
        t.localScale = position;
    }

    public static void SetLocalScaleRecursive(this Transform t, Vector3 scale) {
        t.localScale = scale;
        for (int i = 0; i < t.childCount; i++) {
            t.GetChild(i).SetLocalScaleRecursive(scale);
        }
    }

    public static void SetEulerX(this Transform t, float x) {
        Vector3 position = t.eulerAngles;
        position.x = x;
        t.eulerAngles = position;
    }

    public static void SetEulerY(this Transform t, float y) {
        Vector3 position = t.eulerAngles;
        position.y = y;
        t.eulerAngles = position;
    }

    public static void SetEulerZ(this Transform t, float z) {
        Vector3 position = t.eulerAngles;
        position.z = z;
        t.eulerAngles = position;
    }

    public static void SetAnchoredX(this RectTransform t, float x) {
        Vector3 position = t.anchoredPosition3D;
        position.x = x;
        t.anchoredPosition3D = position;
    }

    public static void SetAnchoredY(this RectTransform t, float y) {
        Vector3 position = t.anchoredPosition3D;
        position.y = y;
        t.anchoredPosition3D = position;
    }

    public static void SetAnchoredZ(this RectTransform t, float z) {
        Vector3 position = t.anchoredPosition3D;
        position.z = z;
        t.anchoredPosition3D = position;
    }

    public static void SetAnchoredXY(this RectTransform t, float x, float y) {
        Vector3 position = t.anchoredPosition3D;
        position.x = x;
        position.y = y;
        t.anchoredPosition3D = position;
    }

    public static void SetAnchoredXZ(this RectTransform t, float x, float z) {
        Vector3 position = t.anchoredPosition3D;
        position.x = x;
        position.z = z;
        t.anchoredPosition3D = position;
    }

    public static void SetAnchoredYZ(this RectTransform t, float y, float z) {
        Vector3 position = t.anchoredPosition3D;
        position.y = y;
        position.z = z;
        t.anchoredPosition3D = position;
    }

    /// <summary>
    /// 缩放浮点误差修正
    /// </summary>
    public const int kStretchAdjust = 4;

    /// <summary>
    /// 拉伸铺满全屏，同时保持比例不变
    /// 必须等待一帧，Start后才能获取正确的parent大小
    /// </summary>
    /// <param name="t"></param>
    public static void Stretch(this RectTransform t) {
        Rect rawRect = t.rect;
        float scaleX = fullScreenSize.x / rawRect.width;
        float scaleY = fullScreenSize.y / rawRect.height;
        if (scaleX > scaleY) {
            t.sizeDelta = new Vector2(fullScreenSize.x, rawRect.height * scaleX);
        }
        else {
            t.sizeDelta = new Vector2(rawRect.width * scaleY, fullScreenSize.y);
        }
        //Debug.Log($"[Stretch]:{t.GetHierarchyPath()} size:{t.sizeDelta} target:{fullScreenSize} raw:{rawRect.width},{rawRect.height}");
    }

    public static Vector2 fullScreenSize;
    public static Vector2 safeAnchorMin, safeAnchorMax;
    public static Vector2 orientMin, orientMax;

    public static void InitSafeArea(float width, float height) {
        fullScreenSize = new Vector2(width, height);
        float ratio = 1;
        switch (Screen.orientation) {
            case ScreenOrientation.LandscapeLeft:
            case ScreenOrientation.LandscapeRight: {
                float safeAreaWidth = Screen.safeArea.width;
                if (Application.platform == RuntimePlatform.IPhonePlayer) {
                    safeAreaWidth = (Screen.width + Screen.safeArea.width) * 0.5f;
                }
                ratio = safeAreaWidth / Screen.width;
                safeAnchorMin = new Vector2(1 - ratio, 0);
                safeAnchorMax = new Vector2(ratio, 1);
                ratio = 1 / ratio;
                orientMin = new Vector2(1 - ratio, 0);
                orientMax = new Vector2(ratio, 1);
                break;
            }
            case ScreenOrientation.Portrait:
            case ScreenOrientation.PortraitUpsideDown:
                float safeAreaHeight = Screen.safeArea.height;
                if (Application.platform == RuntimePlatform.IPhonePlayer) {
                    safeAreaHeight = (Screen.height + Screen.safeArea.height) * 0.5f;
                }
                ratio = safeAreaHeight / Screen.height;
                safeAnchorMin = new Vector2(0, 1 - ratio);
                safeAnchorMax = new Vector2(1, ratio);
                ratio = 1 / ratio;
                orientMin = new Vector2(0, 1 - ratio);
                orientMax = new Vector2(1, ratio);
                break;
            default:
                safeAnchorMin = Vector2.zero;
                safeAnchorMax = Vector2.one;
                Debug.LogError($"InitSafeArea error orientation:{Screen.orientation}");
                break;
        }
        //Debug.Log($"InitSafeArea:{Screen.orientation} min:{safeAnchorMin.ToString("F5")} max:{safeAnchorMax.ToString("F5")} safe:{Screen.safeArea} {Screen.width},{Screen.height}");
    }

    /// <summary>
    /// 屏幕朝向变化填充全屏
    /// 注:锚点模式上下左右拉伸
    /// </summary>
    public static void SetFullScreenOri(this RectTransform t) {
        SetFullScreenOri(t, Vector2.zero);
    }

    /// <summary>
    /// 屏幕朝向变化填充全屏
    /// 注:锚点模式上下左右拉伸
    /// </summary>
    /// <param name="t"></param>
    /// <param name="sizeDelta">一遍设置t.sizeDelta,可保持原来高度</param>
    public static void SetFullScreenOri(this RectTransform t, Vector2 sizeDelta) {
        t.anchorMin = orientMin;
        t.anchorMax = orientMax;
        if (safeAnchorMin.x > float.Epsilon) {
            sizeDelta.x += kStretchAdjust;
        }
        if (safeAnchorMin.y > float.Epsilon) {
            sizeDelta.y += kStretchAdjust;
        }
        t.sizeDelta = sizeDelta;
    }


    public static void SetLayerRecursive(this Transform xform, int layer) {
        if (xform == null) {
            Debug.LogError("SetLayerInChild null:" + layer);
            return;
        }
        //优化效率，不重复设置
        if (xform.gameObject.layer == layer) {
            return;
        }
        xform.gameObject.layer = layer;
        foreach (Transform child in xform) {
            child.SetLayerRecursive(layer);
        }
    }

    /// <summary>
    /// 获取节点在场景中的路径
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string GetHierarchyPath(this Transform t) {
        StringBuilder builder = new StringBuilder();
        builder.Append(t.name);

        Transform parent = t.parent;
        while (parent != null) {
            builder.Insert(0, parent.name + "/");
            parent = parent.parent;
        }

        return builder.ToString();
    }

    /// <summary>
    /// 获取对象在Hierarchy中的节点路径
    /// </summary>
    public static string GetHierarchyPath(this Transform tran, Transform root) {
        return GetHierarchyPathLoop(tran, root);
    }

    /// <summary>
    /// 获取对象在Hierarchy中的节点路径
    /// </summary>
    public static string GetHierarchyPath(this GameObject obj, Transform root) {
        return GetHierarchyPath(obj.transform, root);
    }

    /// <summary>
    /// 获取对象在Hierarchy中的节点路径
    /// </summary>
    public static string GetHierarchyPath(this Component obj, Transform root) {
        return GetHierarchyPath(obj.transform, root);
    }

    private static string GetHierarchyPathLoop(Transform t, Transform root = null, string path = null) {
        if (string.IsNullOrEmpty(path)) {
            path = t.gameObject.name;
        }
        else {
            path = t.gameObject.name + "/" + path;
        }
        if (t.parent != null && t.parent != root) {
            return GetHierarchyPathLoop(t.parent, root, path);
        }
        else {
            return path;
        }
    }

    /// <summary>
    /// 获取或者添加一个脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component {
        T t = obj.GetComponent<T>();
        if (t == null) {
            t = obj.AddComponent<T>();
        }
        return t;
    }

    /// <summary>
    /// 获取或者添加一个脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this Transform t) where T : Component {
        T newT = t.GetComponent<T>();
        if (newT == null) {
            newT = t.gameObject.AddComponent<T>();
        }
        return newT;
    }

    private static Material m_grayMaterial;

    /// <summary>
    /// 创建置灰材质球
    /// </summary>
    /// <returns></returns>
    public static Material GetGrayMaterial() {
        if (m_grayMaterial == null) {
            Shader shader = Shader.Find("Custom/UI/Gray");
            if (shader == null) {
                Debug.LogError("GetGrayMaterial Shader Null");
                return null;
            }
            Material mat = new Material(shader);
            m_grayMaterial = mat;
        }

        return m_grayMaterial;
    }

    public static void SetGray(this Graphic img) {
        img.material = GetGrayMaterial();
        //img.SetMaterialDirty();
    }

    public static void UnsetGray(this Graphic img) {
        if (img.material != GetGrayMaterial()) {
            return;
        }
        img.material = null;
    }

    public static void SetGray(this IText txt) {
        txt.material = GetGrayMaterial();
        //txt.SetMaterialDirty();
    }

    public static void UnsetGray(this IText img) {
        if (img.material != GetGrayMaterial()) {
            return;
        }
        img.material = null;
    }

    /// <summary>
    /// 水平距离
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static float HorizontalDistance(this Vector3 from, Vector3 to) {
        float xGap = from.x - to.x;
        float zGap = from.z - to.z;
        float dist = Mathf.Sqrt(xGap * xGap + zGap * zGap);
        return dist;
    }

    /// <summary>
    /// 水平距离的平方
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static float HorizontalSqrDistance(this Vector3 from, Vector3 to) {
        float xGap = from.x - to.x;
        float zGap = from.z - to.z;
        float sqrDist = xGap * xGap + zGap * zGap;
        return sqrDist;
    }

    public static float HorizontalSqrDistance(this Vector2 from, Vector2 to) {
        float xGap = from.x - to.x;
        float yGap = from.y - to.y;
        float sqrDist = xGap * xGap + yGap * yGap;
        return sqrDist;
    }

    public static bool HorizontalInRang(this Vector3 from, Vector3 to, float range) {
        return HorizontalSqrDistance(from, to) <= Mathf.Pow(range, 2);
    }

    public static bool HorizontalInRang(this Vector2 from, Vector2 to, float range) {
        return HorizontalSqrDistance(from, to) <= Mathf.Pow(range, 2);
    }

    /// <summary>
    /// 判断是否在水平指定格数内
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static bool HorizontalInGrid(this Vector3 from, Vector3 to, float range) {
        float xGap = from.x - to.x;
        if (Mathf.Abs(xGap) > range) {
            return false;
        }
        float zGap = from.z - to.z;
        if (Mathf.Abs(zGap) > range) {
            return false;
        }
        return true;
    }

    #region Direction
    /// <summary>Rotate Y axis on current direction vector</summary>
    /// <param name="self"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector2 Rotate(this Vector2 self, float angle) {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float tx = self.x;
        float ty = self.y;
        self.x = (cos * tx) + (sin * ty);
        self.y = (cos * ty) - (sin * tx);
        return self;
    }

    /// <summary>Direction between 2 position</summary>
    /// <param name="from">Position</param>
    /// <param name="to">Position</param>
    /// <returns>Direction Vector</returns>
    public static Vector3 Direction(this Vector3 from, Vector3 to) {
        return (to - from).normalized;
    }
    /// <summary>Rotate X axis on current direction vector</summary>
    /// <param name="self"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector3 RotateX(this Vector3 self, float angle) {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float ty = self.y;
        float tz = self.z;
        self.y = (cos * ty) - (sin * tz);
        self.z = (cos * tz) + (sin * ty);
        return self;
    }
    /// <summary>Rotate Y axis on current direction vector</summary>
    /// <param name="self"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector3 RotateY(this Vector3 self, float angle) {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float tx = self.x;
        float tz = self.z;
        self.x = (cos * tx) + (sin * tz);
        self.z = (cos * tz) - (sin * tx);
        return self;
    }
    /// <summary>Rotate Z axis on current direction vector</summary>
    /// <param name="self"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector3 RotateZ(this Vector3 self, float angle) {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float tx = self.x;
        float ty = self.y;
        self.x = (cos * tx) - (sin * ty);
        self.y = (cos * ty) + (sin * tx);
        return self;
    }
    /// <summary>Find the relative vector from giving angle & axis</summary>
    /// <param name="self"></param>
    /// <param name="angle">0~360</param>
    /// <param name="axis">Vector direction e.g. Vector.up</param>
    /// <param name="useRadians">0~360 = false, 0~1 = true</param>
    /// <returns></returns>
    public static Vector3 RotateAroundAxis(this Vector3 self, float angle, Vector3 axis, bool useRadians = false) {
        if (useRadians)
            angle *= Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, axis);
        return (q * self);
    }
    #endregion

    #region Angle
    /// <summary>find angle between 2 position, using itself as center</summary>
    /// <param name="center"></param>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <returns></returns>
    public static float AngleBetweenPosition(this Vector3 center, Vector3 point1, Vector3 point2) {
        return Vector3.Angle((point1 - center).normalized, (point2 - center).normalized);
    }

    /// <summary>Determine the signed angle between two vectors, with normal as the rotation axis.</summary>
    /// <example>Vector3.AngleBetweenDirectionSigned(Vector3.forward,Vector3.right)</example>
    /// <param name="direction1">Direction vector</param>
    /// <param name="direction2">Direction vector</param>
    /// <param name="normal">normal vector e.g. AxisXZ = Vector3.Cross(Vector3.forward, Vector3.right);</param>
    /// <see cref="http://forum.unity3d.com/threads/need-vector3-angle-to-return-a-negtive-or-relative-value.51092/"/>
    /// <see cref="http://stackoverflow.com/questions/19675676/calculating-actual-angle-between-two-vectors-in-unity3d"/>
    public static float AngleBetweenDirectionSigned(this Vector3 direction1, Vector3 direction2, Vector3 normal) {
        return Mathf.Rad2Deg * Mathf.Atan2(Vector3.Dot(normal, Vector3.Cross(direction1, direction2)), Vector3.Dot(direction1, direction2));
        //return Vector3.Angle(direction1, direction2) * Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(direction1, direction2)));
    }
    #endregion
}

