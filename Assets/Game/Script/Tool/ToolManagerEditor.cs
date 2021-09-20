using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(ToolManager))]
    public class ToolManagerEditor : Editor
    {
        private Enum.ObjectType ObjectType;

        private Enum.CharacterType CharacterType;
        private Enum.EnemyType EnemyType;
        private Enum.WaterType WaterType;
        private Enum.AttackType AttackType;

        private GameObject[] Characters;
        private GameObject[] Enemys;
        private GameObject[] Objects;
        private GameObject[] Clears;

        private Editor[] editors;

        //private Enum.ObjectType selectObject;
        //private Enum.CharacterType selectCharacter;
        //private Enum.EnemyType selectEnemy;

        private bool active = false;

        ToolManager tool;
        private float size = 6.95f;
        Vector2 scrollPos1;
        Vector2 scrollPos2;
        Vector2 scrollPos3;
        Vector2 scrollPos4;
        Vector2 scrollPos5;
        string level_name;

        string select = "id";

        private void OnEnable()
        {
            ObjectType = Enum.ObjectType.None;

            //selectCharacter = Enum.CharacterType.None;
            //selectEnemy = Enum.EnemyType.None;
            //selectObject = Enum.ObjectType.None;

            Characters = Resources.LoadAll<GameObject>("Prefabs/Character");
            Enemys = Resources.LoadAll<GameObject>("Prefabs/Enemy");
            Objects = Resources.LoadAll<GameObject>("Prefabs/Object");
            Clears = Resources.LoadAll<GameObject>("Prefabs/Clear");

            tool = (ToolManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            active = EditorGUILayout.Toggle("Locked active tool: ", active);
            size = EditorGUILayout.Slider(size, 5, 20);
            tool.Camera.orthographicSize = size;

            if (GUILayout.Button("Reload", GUILayout.Width(100)))
            {
                select = "";
            }

            AttackType = (Enum.AttackType)EditorGUILayout.EnumPopup("Type of attack: ", AttackType);
            if (AttackType == Enum.AttackType.Water)
            {
                WaterType = (Enum.WaterType)EditorGUILayout.EnumPopup("Direction of water: ", WaterType);
            }
            GUILayout.Label("--------------------------------------");
            ObjectType = (Enum.ObjectType)EditorGUILayout.EnumPopup("Primitive to create:", ObjectType);
            switch (ObjectType)
            {
                case Enum.ObjectType.Character:
                    CharacterType = (Enum.CharacterType)EditorGUILayout.EnumPopup("Character to create:", CharacterType);
                    EditorGUILayout.Space();
                    if (select != "character")
                    {
                        select = "character";
                        ClearEditor();
                        editors = new Editor[Characters.Length];
                    }

                    EditorGUILayout.BeginHorizontal();
                    scrollPos1 = EditorGUILayout.BeginScrollView(scrollPos1, GUILayout.Height(300));

                    for (int i = 0; i < Characters.Length; i++)
                    {
                        if (Characters[i].GetComponent<Game.Character>().CharacterType != CharacterType)
                            continue;
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                        GUIStyle bgColor = new GUIStyle();
                        if (editors[i] == null)
                            editors[i] = Editor.CreateEditor(Characters[i]);
                        editors[i].OnInteractivePreviewGUI(GUILayoutUtility.GetRect(100, 100), bgColor);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space(5);
                        if (GUILayout.Button("Choose", GUILayout.Width(100)))
                        {
                            GameObject _object = Instantiate(Characters[i]);
                            _object.name = _object.name.Replace("(Clone)", "");
                            _object.transform.position = tool.Center.transform.position;
                            Selection.activeGameObject = _object;
                            tool.Objects.Add(_object);
                        }

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }
                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndHorizontal();
                    break;
                case Enum.ObjectType.Enemy:
                    if (AttackType == Enum.AttackType.Water)
                    {
                        //EditorGUILayout.Space();
                        //WaterType = (Enum.WaterType)EditorGUILayout.EnumPopup("Direction of water: ", WaterType);
                    }
                    else
                    {
                        EnemyType = (Enum.EnemyType)EditorGUILayout.EnumPopup("Enemy to create:", EnemyType);
                        EditorGUILayout.Space();
                        if (select != "enemy")
                        {
                            select = "enemy";
                            ClearEditor();
                            editors = new Editor[Enemys.Length];
                        }

                        EditorGUILayout.BeginHorizontal();
                        scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Height(300));

                        for (int i = 0; i < Enemys.Length; i++)
                        {
                            if (Enemys[i].GetComponent<Game.Enemy>().EnemyType != EnemyType)
                                continue;

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                            GUIStyle bgColor = new GUIStyle();
                            if (editors[i] == null)
                                editors[i] = Editor.CreateEditor(Enemys[i]);
                            editors[i].OnInteractivePreviewGUI(GUILayoutUtility.GetRect(100, 100), bgColor);
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space(5);
                            if (GUILayout.Button("Choose", GUILayout.Width(100)))
                            {
                                GameObject _object = Instantiate(Enemys[i]);
                                _object.name = _object.name.Replace("(Clone)", "");
                                _object.transform.position = tool.Center.transform.position;
                                Selection.activeGameObject = _object;
                                tool.Objects.Add(_object);
                            }

                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                        }

                        EditorGUILayout.EndScrollView();
                        EditorGUILayout.EndHorizontal();
                    }
                    break;
                case Enum.ObjectType.Object:
                    EditorGUILayout.Space();
                    if (select != "object")
                    {
                        select = "object";
                        ClearEditor();
                        editors = new Editor[Objects.Length];
                    }

                    EditorGUILayout.BeginHorizontal();
                    scrollPos3 = EditorGUILayout.BeginScrollView(scrollPos3, GUILayout.Height(300));

                    for (int i = 0; i < Objects.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                        GUIStyle bgColor = new GUIStyle();
                        if (editors[i] == null)
                            editors[i] = Editor.CreateEditor(Objects[i]);
                        editors[i].OnInteractivePreviewGUI(GUILayoutUtility.GetRect(100, 100), bgColor);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space(5);
                        if (GUILayout.Button("Choose", GUILayout.Width(100)))
                        {
                            GameObject _object = Instantiate(Objects[i]);
                            _object.name = _object.name.Replace("(Clone)", "");
                            _object.transform.position = tool.Center.transform.position;
                            Selection.activeGameObject = _object;
                            tool.Objects.Add(_object);
                        }

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndHorizontal();
                    break;
                case Enum.ObjectType.Clear:
                    EditorGUILayout.Space();
                    if (select != "clear")
                    {
                        select = "clear";
                        ClearEditor();
                        editors = new Editor[Clears.Length];
                    }

                    EditorGUILayout.BeginHorizontal();
                    scrollPos4 = EditorGUILayout.BeginScrollView(scrollPos4, GUILayout.Height(300));

                    for (int i = 0; i < Clears.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                        GUIStyle bgColor = new GUIStyle();
                        if (editors[i] == null)
                            editors[i] = Editor.CreateEditor(Clears[i]);
                        editors[i].OnInteractivePreviewGUI(GUILayoutUtility.GetRect(100, 100), bgColor);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space(5);
                        if (GUILayout.Button("Choose", GUILayout.Width(100)))
                        {
                            GameObject _object = Instantiate(Clears[i]);
                            _object.name = _object.name.Replace("(Clone)", "");
                            _object.transform.position = tool.Center.transform.position;
                            Selection.activeGameObject = _object;
                            tool.Objects.Add(_object);
                        }

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndHorizontal();
                    break;
            }

            ActiveEditorTracker.sharedTracker.isLocked = active;

            if(tool.Objects.Count > 0)
            {
                for (int i = 0; i < tool.Objects.Count; i++)
                {
                    if (tool.Objects[i] == null)
                    {
                        tool.Objects.RemoveAt(i);
                        break;
                    }
                }

                EditorGUILayout.Space(10);
                GUILayout.Label("--------------------------------------");
                EditorGUILayout.BeginHorizontal();
                scrollPos5 = EditorGUILayout.BeginScrollView(scrollPos5, GUILayout.Height(100));
                for (int i = 0; i < tool.Objects.Count; i++)
                {
                    EditorGUILayout.ObjectField("Object: ", tool.Objects[i], typeof(GameObject), true);
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            level_name = EditorGUILayout.TextField("Level Name: ", level_name);
            if (GUILayout.Button("Save Level"))
            {
                Game.Level info = new Game.Level();
                info.LevelData = new Game.LevelData();
                info.LevelData.SizeCamera = size;
                info.LevelData.AttackType = AttackType;
                info.LevelData.WaterType = WaterType;
                info.LevelData.Datas = new List<Game.ObjectData>();
                for (int i = 0; i < tool.Objects.Count; i++)
                {
                    Game.ObjectData data = new Game.ObjectData();
                    var entity = tool.Objects[i].GetComponent<Game.Entity>();
                    data.ObjectType = entity.ObjectType;
                    switch(data.ObjectType)
                    {
                        case Enum.ObjectType.Character:
                            var character = tool.Objects[i].GetComponent<Game.Character>();
                            data.CharacterType = character.CharacterType;
                            data.EnemyType = Enum.EnemyType.None;
                            data.NameObject = tool.Objects[i].name;
                            data.Postion = tool.Objects[i].transform.position;
                            data.Rotation = tool.Objects[i].transform.rotation;
                            data.LocalScale = tool.Objects[i].transform.localScale;
                            break;
                        case Enum.ObjectType.Enemy:
                            var enemy = tool.Objects[i].GetComponent<Game.Enemy>();
                            data.CharacterType = Enum.CharacterType.None;
                            data.EnemyType = enemy.EnemyType;
                            data.NameObject = tool.Objects[i].name;
                            data.Postion = tool.Objects[i].transform.position;
                            data.Rotation = tool.Objects[i].transform.rotation;
                            data.LocalScale = tool.Objects[i].transform.localScale;
                            break;
                        case Enum.ObjectType.Object:
                            data.CharacterType = Enum.CharacterType.None;
                            data.EnemyType = Enum.EnemyType.None;
                            data.NameObject = tool.Objects[i].name;
                            data.Postion = tool.Objects[i].transform.position;
                            data.Rotation = tool.Objects[i].transform.rotation;
                            data.LocalScale = tool.Objects[i].transform.localScale;
                            break;
                        case Enum.ObjectType.Clear:
                            data.CharacterType = Enum.CharacterType.None;
                            data.EnemyType = Enum.EnemyType.None;
                            data.NameObject = tool.Objects[i].name;
                            data.Postion = tool.Objects[i].transform.position;
                            data.Rotation = tool.Objects[i].transform.rotation;
                            data.LocalScale = tool.Objects[i].transform.localScale;
                            break;
                    }
                    info.LevelData.Datas.Add(data);
                }
                if(!string.IsNullOrEmpty(level_name))
                {
                    var _str = JsonUtility.ToJson(info);
                    var path = "Assets/Resources/Level/" + level_name + ".json";
                    Debug.Log(string.Format("Save Success with path {0}", path));

                    if (path.Length > 0)
                    {
                        System.IO.File.WriteAllText(path, _str);
                    }

                    EditorUtility.SetDirty(this);
                    AssetDatabase.Refresh();
                }  
                else
                {
                    EditorUtility.DisplayDialog("Chưa có tên", "Chưa nhập tên level chị oiiiiiii, nhập nhé...", "Okayy");
                }    
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ClearEditor()
        {
            if (editors != null && editors.Length > 0)
            {
                for (int i = 0; i < editors.Length; i++)
                {
                    DestroyImmediate(editors[i]);
                }
            }
        }    
    }
}

//var path = "Assets/Resources/Level/" + property.stringValue + ".json";
//var path = GetPath(GetText(index));
//Debug.Log(string.Format("Save Success with {0}", path));

//if (path.Length > 0)
//{
//    System.IO.File.WriteAllText(path, _str);
//}

//EditorUtility.SetDirty(this);
//AssetDatabase.Refresh();

//char is gameobject
//EditorGUILayout.BeginHorizontal();
//EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
//EditorGUILayout.BeginVertical();
//GUIStyle bgColor = new GUIStyle();
//bgColor.normal.background = EditorGUIUtility.whiteTexture;

//if (charsEditor[i] == null)
//    charsEditor[i] = Editor.CreateEditor(chars[i]);

//charsEditor[i].OnInteractivePreviewGUI(GUILayoutUtility.GetRect(300, 260), bgColor);

//EditorGUILayout.LabelField(chars[i].name);
//EditorGUILayout.EndVertical();

//EditorGUILayout.EndHorizontal();

//EditorGUILayout.Space(10);

//EditorGUILayout.BeginHorizontal();


//EditorGUILayout.BeginVertical();
//EditorGUILayout.Space(10);

