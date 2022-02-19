//#define UseLocalize
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using System.Text;
using System.IO;
using System.Collections.Generic;
/// <summary>
/// UI代码生成类
/// </summary>
public class UICodeGenerater {
    private static string TemplateClass = @"
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace AutoGenerateUI@SubSpace {
    /// <summary>
    /// 程序生成的UI代码类，请勿修改
    /// </summary>
    public sealed partial class @ClassName  {
        public @ClassName() {

        }
        public @ClassName(Transform inst) {
            Init(inst);
        }
        @Property

        public void Init(Transform inst) {
            @InitFuncBody
        }

        public void Free() {
            @FreeFuncBody
        }
    }
}
";

    static string TemplateProperty = @"
        public @TypeName @FieldName  { get; private set; }";

    static string TemplateInitFuncBody = @"
            Transform @FieldNameXform = @ParentName.Find(""@XformName"");
            if ( @FieldNameXform != null) {
                @FieldName = @FieldNameXform.GetComponent<@TypeName>();
            } else {
                Debug.LogError(""@FieldName Can't Find Under @ParentName"");
            }";

    static string TemplateInitFuncBodyList = @"
            List<@TypeName> tmp@FieldNames = new List<@TypeName>();
            foreach (Transform child in @ParentName){
                if (child.name != ""@XformName""){
                    continue;
                }
                @TypeName item = child.GetComponent<@TypeName>();
                tmp@FieldNames.Add(item);
            }
            @FieldNames = tmp@FieldNames.ToArray();";

    static string TemplateInitNodeBody = @"
            Transform @FieldNameXform = @ParentName.Find(""@XformName"");
            if ( @FieldNameXform != null) {
                @FieldName = @FieldNameXform as @TypeName;
            } else {
                Debug.LogError(""@FieldName Can't Find Under @ParentName"");
            }";

    static string TemplateInitNodeBodyList = @"
            List<@TypeName> tmp@FieldNames = new List<@TypeName>();
            foreach (Transform child in @ParentName){
                if (child.name != ""@XformName""){
                    continue;
                }
                @TypeName item = child as @TypeName;
                tmp@FieldNames.Add(item);
            }
            @FieldNames = tmp@FieldNames.ToArray();";

    static string TemplateInitCellBody = @"
            Transform @FieldNameXform = @ParentName.Find(""@XformName"");
            if ( @FieldNameXform != null) {
                @FieldName = new @TypeName(@FieldNameXform);
            } else {
                Debug.LogError(""@XformName Can't Find Under @ParentName"");
            }";

    static string TemplateInitCellBodyList = @"
            List<@TypeName> tmp@FieldNames = new List<@TypeName>();
            foreach (Transform child in @ParentName){
                if (child.name != ""@XformName""){
                    continue;
                }
                @TypeName item = new @TypeName(child);
                tmp@FieldNames.Add(item);
            }
            @FieldNames = tmp@FieldNames.ToArray();";

    static string TemplateFreeCell = @"
            if (@FieldName != null) {
                @FieldName.Free();
            }
            @FieldName = null;";

    static string TemplateFreeCellList = @"
            if (@FieldName != null) {
                for (int index = 0; index < @FieldName.Length; index++) {
                    var cell = @FieldName[index];
                    cell.Free();
                }
            }
            @FieldName = null;";

    static string TemplateFreeFunc = @"
            @FieldName = null;";

    static string TemplateProperty_Private = @"
        private @TypeName @FieldName  { get; set; }";


    [MenuItem("Tools/UI代码生成", false, -30)]
    [MenuItem("GameObject/Tool", false, -300)]
    [MenuItem("GameObject/Tool/UI代码生成", false, -30)]
    [MenuItem("Assets/Tool/UI代码生成", false, -30)]
    public static void GenerateCode() {
        if (Selection.activeGameObject == null) {
            Debug.LogError("请先选中UI");
            return;
        }

        var gos = Selection.gameObjects;
        for (int i = gos.Length - 1; i >= 0; i--) {
            var go = gos[i];
            m_subGenIndex = 0;
            m_subGens.Clear();
            s_subSpace = string.Empty;
            m_fields.Clear();
            StartGenerate(go);
        }

        AssetDatabase.Refresh();
    }

    private static string s_subSpace = string.Empty;
    private static void StartGenerate(GameObject uiRoot) {
        string spaceName = s_subSpace;
        string fileName = uiRoot.name;
        if (uiRoot.name.IndexOf("@") != -1) {
            string[] names = uiRoot.name.Split('@');
            fileName = names[0];
            spaceName = names[names.Length - 1];
            if (string.IsNullOrEmpty(s_subSpace)) {
                //第一个命名空间作为之后全部的命名空间，除非再次特别申明
                s_subSpace = spaceName;
            }
        }
        else {
            if (uiRoot.name.IndexOf("=Sub") == -1) {
                Debug.LogError("Set subspace ! please");
                return;
            }
        }
        fileName = fileName.Split('=')[0];
        Debug.Log("StartGenerate UI:" + uiRoot.name + " File:" + fileName);

        StringBuilder propertyBuilder = new StringBuilder();
        StringBuilder initFuncBuilder = new StringBuilder();
        StringBuilder freeFuncBuilder = new StringBuilder();

        //DoGenerate("rect", uiRoot.transform, propertyBuilder, initFuncBuilder, freeFuncBuilder);

        string typeName = GetType(uiRoot.transform);
        if (!IsTransform(typeName)) {
            bool isCell = false;
            string customType;
            if (s_CustomTypes.TryGetValue(typeName, out customType)) {
                isCell = true;
                typeName = customType;
            }

            string fieldName = "root";
            string fieldPath = "inst";
            string property = TemplateProperty.Replace("@TypeName", typeName)
                .Replace("@FieldName", fieldName);
            propertyBuilder.Append(property);

            if (isCell) {
                string initFunc = "@FieldName = new @TypeName(@XformName);".Replace("@TypeName", typeName)
                    .Replace("@FieldName", fieldName)
                    .Replace("@XformName", fieldPath);
                initFuncBuilder.Append(initFunc);

                string freeFunc = TemplateFreeCell.Replace("@FieldName", fieldName);
                freeFuncBuilder.Append(freeFunc);
            }
            else {
                string initFunc = "@FieldName = @XformName.GetComponent<@TypeName>();".Replace("@TypeName", typeName)
                        .Replace("@FieldName", fieldName)
                        .Replace("@XformName", fieldPath);

                initFuncBuilder.Append(initFunc);

                string freeFunc = TemplateFreeFunc.Replace("@FieldName", fieldName);
                freeFuncBuilder.Append(freeFunc);
            }
        }

        foreach (Transform childXform in uiRoot.transform) {
            DoGenerate("inst", childXform, propertyBuilder, initFuncBuilder, freeFuncBuilder);
        }

        string template = TemplateClass.Replace("@ClassName", fileName.Replace("View", "UI"))
            .Replace("@Property", propertyBuilder.ToString())
            .Replace("@InitFuncBody", initFuncBuilder.ToString())
            .Replace("@FreeFuncBody", freeFuncBuilder.ToString());

        //Debug.Log("template:" + template);

        //string path = Application.dataPath + "/Scripts/Module/";
        string path = Application.dataPath + "/UICode/Module/";
        if (!string.IsNullOrEmpty(spaceName)) {
            template = template.Replace("@SubSpace", "." + spaceName);
            path += spaceName + "/UIGen/";
        }
        else {
            template = template.Replace("@SubSpace", string.Empty);
            path += "UIGen/";
        }
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        string filePath = Path.Combine(path, fileName + ".cs");
        File.WriteAllText(filePath, template, System.Text.Encoding.UTF8);
        Debug.Log($"UICodeGenerate:{filePath} fields:{m_fields.Count} sub:{m_subGenIndex}/{m_subGens.Count}");

        if (m_subGenIndex < m_subGens.Count) {
            m_fields.Clear();
            ++m_subGenIndex;
            StartGenerate(m_subGens[m_subGenIndex - 1]);
        }
    }

    private static List<string> m_fields = new List<string>(50);
    private static int m_subGenIndex = 0;
    private static List<GameObject> m_subGens = new List<GameObject>();

    /// <summary>
    /// 自定义的常用结构体
    /// </summary>
    private static Dictionary<string, string> s_CustomTypes = new Dictionary<string, string>() {
            { "Cell", "Cell" },
            { "Button", "ButtonCell" },
            { "Toggle", "ToggleCell" },
            { "Slider", "SliderCell" },
            { "InputField", "InputCell" },
            { "ScrollRect", "ScrollCell" },
            { "Scrollbar", "BarCell" },
            { "Dropdown", "DropdownCell" },
            { "TMPro.TMP_InputField", "InputCell" },
    };

    private static string GetType(Transform xform) {
        string typeName;
        if (xform.GetComponent<Button>()) {
            typeName = typeof(Button).Name;
        }
        else if (xform.GetComponent<Toggle>()) {
            typeName = typeof(Toggle).Name;
        }
        else if (xform.GetComponent<ToggleGroup>()) {
            typeName = typeof(ToggleGroup).Name;
        }
        else if (xform.GetComponent<Slider>()) {
            typeName = typeof(Slider).Name;
        }
        else if (xform.GetComponent<Scrollbar>()) {
            typeName = typeof(Scrollbar).Name;
        }
        else if (xform.GetComponent<ScrollRect>()) {
            typeName = typeof(ScrollRect).Name;
        }
        else if (xform.GetComponent<Dropdown>()) {
            typeName = typeof(Dropdown).Name;
        }
        else if (xform.GetComponent<TMPro.TextMeshProUGUI>()) {
            //typeName = typeof(TMPro.TextMeshProUGUI).Name;
            //typeName = "TMPro.TextMeshProUGUI";
            typeName = "TMPro.TextMeshProUGUI";
        }
        else if (xform.GetComponent<TMPro.TMP_InputField>()) {
            //typeName = typeof(TMPro.TMP_InputField).Name;
            typeName = "TMPro.TMP_InputField";
        }
        else if (xform.GetComponent<InputField>()) {
            typeName = typeof(InputField).Name;
        }
        else if (xform.GetComponent<RawImage>()) {
            typeName = typeof(RawImage).Name;
        }
        else if (xform.GetComponent<Image>()) {
            typeName = typeof(Image).Name;
        }
        else if (xform.GetComponent<TextExtra>()) {
            //typeName = typeof(Text).Name;
            typeName = "Text";
        }
        else if (xform.GetComponent<SpriteRenderer>()) {
            typeName = typeof(SpriteRenderer).Name;
        }
        else if (xform.GetComponent<TextMesh>()) {
            typeName = typeof(TextMesh).Name;
        }
        else if (xform.GetComponent<TMPro.TextMeshPro>()) {
            //typeName = typeof(TMPro.TextMeshPro).Name;
            typeName = "TMPro.TextMeshPro";
        }
        else if (xform.GetComponent<RectTransform>()) {
            typeName = typeof(RectTransform).Name;
        }
        else {
            typeName = typeof(Transform).Name;
        }
        return typeName;
    }


    private class XformInfo {
        public Transform xform { get; private set; }
        public string fieldName { get; private set; }
        public string fieldPath { get; private set; }
        public string fieldNameLower { get; private set; }
        public string typeName { get; private set; }
        public string rawTypeName { get; private set; }
        //是否作为单独的类
        public bool isCell { get; private set; }
        //是否跳过子节点
        public bool isSkip { get; private set; }
        //是否数组
        public bool isArray { get; private set; }
        //是否是带_的特殊对象（_前缀会被忽略，因此此处一般通常是isLocalizeText）
        public bool is_ { get; private set; }
        //字段文本区分
        public string diffString { get; private set; }

        public XformInfo(Transform xform) {
            this.xform = xform;
            Analysiss(xform);
        }

        private void Analysiss(Transform xform) {
            fieldName = xform.name;
            fieldNameLower = fieldName.ToLower();
            fieldPath = fieldName;
            rawTypeName = UICodeGenerater.GetType(xform);
            typeName = rawTypeName;

            string customType;
            if (s_CustomTypes.TryGetValue(rawTypeName, out customType)) {
                isCell = true;
                typeName = customType;
            }

            if (fieldName.IndexOf('=') != -1) {
                string[] names = fieldName.Split('=');
                fieldName = names[0];
                for (int i = 1; i < names.Length; i++) {
                    string key = names[i];
                    if (key == "List" || key == "Array") {
                        isArray = true;

                        names[i] = null;
                    }
                    else if (key == "Sub") {
                        isCell = true;
                        isSkip = true;
                        typeName = fieldName;

                        m_subGens.Add(xform.gameObject);
                        names[i] = null;
                    }
                    else if (s_CustomTypes.TryGetValue(key, out customType)
                      || s_CustomTypes.TryGetValue(rawTypeName, out customType)
                      ) {
                        isCell = true;
                        typeName = customType;

                        names[i] = null;
                    }
                }
                for (int i = 1; i < names.Length; i++) {
                    string key = names[i];
                    if (!string.IsNullOrEmpty(key)) {
                        Debug.LogError("CustomType Error UI Name:" + fieldName + " -> " + key);
                    }
                }
            }

            if (fieldName.StartsWith("_")) {
                is_ = true;
                diffString = Mathf.Abs(xform.GetInstanceID()).ToString();
            }
            else {
                diffString = null;
            }
        }
    }

    //忽略不需要的UI节点，提高初始化速度
    private static bool IgnorTransform(Transform xform) {
        string name = xform.name;
        if (name.StartsWith("_")) {
            return true;
        }

        if (name.IndexOf(" ") != -1) {
            Debug.LogError("Error UI Name:" + name);
            return true;
        }

        if (m_fields.IndexOf(name) != -1) {
            return true;
        }

        return false;
    }

    private static bool IsTransform(string type) {
        if (type == typeof(Transform).Name || type == typeof(RectTransform).Name) {
            return true;
        }

        return false;
    }

    private static void DoGenerate(string parentName, Transform xform,
        StringBuilder propertyBuilder, StringBuilder initFuncBuilder, StringBuilder freeFuncBuilder) {
        if (IgnorTransform(xform)) {
            return;
        }

        XformInfo info = new XformInfo(xform);
        string fieldName = info.is_ ? info.fieldName + info.diffString : info.fieldName;
        if (info.isArray) {
            string property;
            if (info.is_) {
                property = TemplateProperty_Private.Replace("@TypeName", info.typeName + "[]")
                .Replace("@FieldName", fieldName + "s")
                .Replace("@XformName", info.fieldPath);
            }
            else {
                property = TemplateProperty.Replace("@TypeName", info.typeName + "[]")
                .Replace("@FieldName", fieldName + "s")
                .Replace("@XformName", info.fieldPath);
            }
            propertyBuilder.Append(property);

            if (info.isCell) {
                string initFunc = TemplateInitCellBodyList.Replace("@TypeName", info.typeName)
                    .Replace("@FieldName", fieldName)
                    .Replace("@ParentName", parentName)
                    .Replace("@XformName", info.fieldPath);
                initFuncBuilder.Append(initFunc);

                string freeFunc = TemplateFreeCellList.Replace("@FieldName", fieldName + "s");
                freeFuncBuilder.Append(freeFunc);
            }
            else {
                string initFunc;
                if (IsTransform(info.rawTypeName)) {
                    initFunc = TemplateInitNodeBodyList.Replace("@TypeName", info.typeName)
                        .Replace("@FieldName", fieldName)
                        .Replace("@ParentName", parentName)
                        .Replace("@XformName", info.fieldPath);
                }
                else {
                    initFunc = TemplateInitFuncBodyList.Replace("@TypeName", info.typeName)
                        .Replace("@FieldName", fieldName)
                        .Replace("@ParentName", parentName)
                        .Replace("@XformName", info.fieldPath);
                }
                initFuncBuilder.Append(initFunc);

                string freeFunc = TemplateFreeFunc.Replace("@FieldName", fieldName + "s");
                freeFuncBuilder.Append(freeFunc);
            }
        }
        else {
            string property;
            if (info.is_) {
                property = TemplateProperty_Private.Replace("@TypeName", info.typeName)
                .Replace("@FieldName", fieldName)
                .Replace("@XformName", info.fieldPath);
            }
            else {
                property = TemplateProperty.Replace("@TypeName", info.typeName)
                .Replace("@FieldName", fieldName)
                .Replace("@XformName", info.fieldPath);
            }
            propertyBuilder.Append(property);

            if (info.isCell) {
                string initFunc = TemplateInitCellBody.Replace("@TypeName", info.typeName)
                    .Replace("@FieldName", fieldName)
                    .Replace("@ParentName", parentName)
                    .Replace("@XformName", info.fieldPath);
                initFuncBuilder.Append(initFunc);

                string freeFunc = TemplateFreeCell.Replace("@FieldName", fieldName);
                freeFuncBuilder.Append(freeFunc);
            }
            else {
                string initFunc;
                if (IsTransform(info.rawTypeName)) {
                    initFunc = TemplateInitNodeBody.Replace("@TypeName", info.typeName)
                        .Replace("@FieldName", fieldName)
                        .Replace("@ParentName", parentName)
                        .Replace("@XformName", info.fieldPath);
                }
                else {
                    initFunc = TemplateInitFuncBody.Replace("@TypeName", info.typeName)
                        .Replace("@FieldName", fieldName)
                        .Replace("@ParentName", parentName)
                        .Replace("@XformName", info.fieldPath);
                }
                initFuncBuilder.Append(initFunc);

                string freeFunc = TemplateFreeFunc.Replace("@FieldName", fieldName);
                freeFuncBuilder.Append(freeFunc);
            }
        }

        m_fields.Add(xform.name);
        if (info.isSkip) {
            return;
        }
        if (info.rawTypeName == typeof(ScrollRect).Name) {
            ScrollRect scrollRect = xform.GetComponent<ScrollRect>();
            foreach (Transform child in scrollRect.viewport) {
                if (child == scrollRect.content) {
                    continue;
                }
                DoGenerate(fieldName + ".viewport", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
            }
            foreach (Transform child in scrollRect.content) {
                DoGenerate(fieldName + ".content", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
            }
            foreach (Transform child in xform) {
                if (child == scrollRect.viewport) {
                    continue;
                }
                if (scrollRect.horizontalScrollbar != null && child == scrollRect.horizontalScrollbar.transform) {
                    continue;
                }
                if (scrollRect.verticalScrollbar != null && child == scrollRect.verticalScrollbar.transform) {
                    continue;
                }
                DoGenerate(fieldName + "Xform", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
            }
        }
        else if (info.rawTypeName == typeof(Scrollbar).Name) {
            Scrollbar scrollbar = xform.GetComponent<Scrollbar>();
            foreach (Transform child in xform) {
                if (child == scrollbar.handleRect) {
                    continue;
                }
                DoGenerate(fieldName + "Xform", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
            }
            if (null != scrollbar.handleRect) {
                foreach (Transform sub in scrollbar.handleRect) {
                    DoGenerate(fieldName + ".handleRect", sub, propertyBuilder, initFuncBuilder, freeFuncBuilder);
                }
            }
        }
        else if (info.rawTypeName == typeof(Slider).Name) {
            Slider slider = xform.GetComponent<Slider>();
            foreach (Transform child in xform) {
                if (child == slider.fillRect) {
                    continue;
                }
                else if (child == slider.handleRect) {
                    continue;
                }
                DoGenerate(fieldName + "Xform", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
            }
            if (null != slider.fillRect) {
                foreach (Transform sub in slider.fillRect) {
                    DoGenerate(fieldName + ".fillRect", sub, propertyBuilder, initFuncBuilder, freeFuncBuilder);
                }
            }
            if (null != slider.handleRect) {
                foreach (Transform sub in slider.handleRect) {
                    DoGenerate(fieldName + ".handleRect", sub, propertyBuilder, initFuncBuilder, freeFuncBuilder);
                }
            }
        }
        else if (info.rawTypeName == typeof(InputField).Name) {
            InputField input = xform.GetComponent<InputField>();
            foreach (Transform child in xform) {
                if (input.placeholder != null && child == input.placeholder.transform) {
                    continue;
                }
                if (input.textComponent != null && child == input.textComponent.transform) {
                    continue;
                }
                DoGenerate(fieldName + "Xform", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
            }
        }
        else {
            if (info.isArray) {
                foreach (Transform child in xform) {
                    DoGenerate_List(fieldName + "s", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
                }
            }
            else {
                foreach (Transform child in xform) {
                    DoGenerate(fieldName + "Xform", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
                }
            }
        }

    }

    #region 针对=List的Array对象遍历

    static string TemplateInitFuncBody_List = @"
            List<@TypeName> tmp@FieldNames = new List<@TypeName>();
            foreach (var obj in @ParentName) {
                Transform @FieldNameXform = obj.transform.Find(""@XformName"");
                if ( @FieldNameXform == null) {
                    continue;
                }
                @TypeName item = @FieldNameXform.GetComponent<@TypeName>();
                tmp@FieldNames.Add(item);
            }
            @FieldNames = tmp@FieldNames.ToArray();";

    static string TemplateInitNodeBody_List = @"
            List<@TypeName> tmp@FieldNames = new List<@TypeName>();
            foreach (var obj in @ParentName) {
                Transform @FieldNameXform = obj.transform.Find(""@XformName"");
                if ( @FieldNameXform == null) {
                    continue;
                }
                @TypeName item = @FieldNameXform as @TypeName;
                tmp@FieldNames.Add(item);
            }
            @FieldNames = tmp@FieldNames.ToArray();";

    static string TemplateInitCellBody_List = @"
            List<@TypeName> tmp@FieldNames = new List<@TypeName>();
            foreach (var obj in @ParentName) {
                Transform @FieldNameXform = obj.transform.Find(""@XformName"");
                if ( @FieldNameXform == null) {
                    continue;
                }
                @TypeName item = new @TypeName(@FieldNameXform);
                tmp@FieldNames.Add(item);
            }
            @FieldNames = tmp@FieldNames.ToArray();";


    private static void DoGenerate_List(string parentName, Transform xform,
        StringBuilder propertyBuilder, StringBuilder initFuncBuilder, StringBuilder freeFuncBuilder) {
        if (IgnorTransform(xform)) {
            return;
        }

        XformInfo info = new XformInfo(xform);
        string fieldName = info.is_ ? info.fieldName + info.diffString : info.fieldName;
        if (info.isArray) {
            Debug.LogError("禁止套娃：" + fieldName);
        }
        else {
            string property;
            if (info.is_) {
                property = TemplateProperty_Private.Replace("@TypeName", info.typeName + "[]")
                .Replace("@FieldName", fieldName + "s")
                .Replace("@XformName", info.fieldPath);
            }
            else {
                property = TemplateProperty.Replace("@TypeName", info.typeName + "[]")
                .Replace("@FieldName", fieldName + "s")
                .Replace("@XformName", info.fieldPath);
            }
            propertyBuilder.Append(property);

            if (info.isCell) {
                string initFunc = TemplateInitCellBody_List.Replace("@TypeName", info.typeName)
                    .Replace("@FieldName", fieldName)
                    .Replace("@ParentName", parentName)
                    .Replace("@XformName", info.fieldPath);
                initFuncBuilder.Append(initFunc);

                string freeFunc = TemplateFreeCellList.Replace("@FieldName", fieldName + "s");
                freeFuncBuilder.Append(freeFunc);
            }
            else {
                string initFunc;
                if (IsTransform(info.rawTypeName)) {
                    initFunc = TemplateInitNodeBody_List.Replace("@TypeName", info.typeName)
                        .Replace("@FieldName", fieldName)
                        .Replace("@ParentName", parentName)
                        .Replace("@XformName", info.fieldPath);
                }
                else {
                    initFunc = TemplateInitFuncBody_List.Replace("@TypeName", info.typeName)
                        .Replace("@FieldName", fieldName)
                        .Replace("@ParentName", parentName)
                        .Replace("@XformName", info.fieldPath);
                }
                initFuncBuilder.Append(initFunc);

                string freeFunc = TemplateFreeFunc.Replace("@FieldName", fieldName + "s");
                freeFuncBuilder.Append(freeFunc);
            }
        }

        m_fields.Add(xform.name);
        if (info.isSkip) {
            return;
        }

        if (info.rawTypeName == typeof(ScrollRect).Name) {
            ScrollRect scrollRect = xform.GetComponent<ScrollRect>();
            foreach (Transform child in scrollRect.viewport) {
                if (child == scrollRect.content) {
                    continue;
                }
                DoGenerate_List(fieldName + ".viewport", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
            }
            foreach (Transform child in scrollRect.content) {
                DoGenerate_List(fieldName + ".content", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
            }
            foreach (Transform child in xform) {
                if (child == scrollRect.viewport) {
                    continue;
                }
                if (scrollRect.horizontalScrollbar != null && child == scrollRect.horizontalScrollbar.transform) {
                    continue;
                }
                if (scrollRect.verticalScrollbar != null && child == scrollRect.verticalScrollbar.transform) {
                    continue;
                }
                DoGenerate_List(fieldName + "s", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
            }
        }
        else if (info.rawTypeName == typeof(Scrollbar).Name) {
            Scrollbar scrollbar = xform.GetComponent<Scrollbar>();
            foreach (Transform child in xform) {
                if (child == scrollbar.handleRect) {
                    continue;
                }
                DoGenerate_List(fieldName + "s", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
            }
            if (null != scrollbar.handleRect) {
                foreach (Transform sub in scrollbar.handleRect) {
                    DoGenerate_List(fieldName + ".handleRect", sub, propertyBuilder, initFuncBuilder, freeFuncBuilder);
                }
            }
        }
        else if (info.rawTypeName == typeof(Slider).Name) {
            Slider slider = xform.GetComponent<Slider>();
            foreach (Transform child in xform) {
                if (child == slider.fillRect) {
                    continue;
                }
                else if (child == slider.handleRect) {
                    continue;
                }
                DoGenerate_List(fieldName + "s", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
            }
            if (null != slider.fillRect) {
                foreach (Transform sub in slider.fillRect) {
                    DoGenerate_List(fieldName + ".fillRect", sub, propertyBuilder, initFuncBuilder, freeFuncBuilder);
                }
            }
            if (null != slider.handleRect) {
                foreach (Transform sub in slider.handleRect) {
                    DoGenerate_List(fieldName + ".handleRect", sub, propertyBuilder, initFuncBuilder, freeFuncBuilder);
                }
            }
        }
        else if (info.rawTypeName == typeof(InputField).Name) {
            InputField input = xform.GetComponent<InputField>();
            foreach (Transform child in xform) {
                if (input.placeholder != null && child == input.placeholder.transform) {
                    continue;
                }
                if (input.textComponent != null && child == input.textComponent.transform) {
                    continue;
                }
                DoGenerate_List(fieldName + "s", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
            }
        }
        else {
            if (info.isArray) {
                Debug.LogError("禁止套娃：" + fieldName);
            }
            else {
                foreach (Transform child in xform) {
                    DoGenerate_List(fieldName + "s", child, propertyBuilder, initFuncBuilder, freeFuncBuilder);
                }
            }
        }
    }

    #endregion
}
