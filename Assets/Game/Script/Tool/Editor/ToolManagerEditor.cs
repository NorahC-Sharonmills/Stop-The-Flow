﻿using System.Collections;
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
        //private Enum.WaterType WaterType;
        //private Enum.AttackType AttackType;

        private GameObject[] Characters;
        private GameObject[] Enemys;
        private GameObject[] Objects;
        private GameObject[] Clears;

        private List<Editor> editors1;
        private List<Editor> editors2;
        private List<Editor> editors3;
        private List<Editor> editors4;
        private bool active = false;
        private bool clear = false;

        ToolManager tool;
        private float size = 6.95f;
        Vector2 scrollPos1;
        Vector2 scrollPos2;
        Vector2 scrollPos3;
        Vector2 scrollPos4;
        Vector2 scrollPos5;
        //string level_name;

        string select = "id";

        //private int level_select = 0;

        private void OnEnable()
        {
            ObjectType = Enum.ObjectType.None;
            Characters = Resources.LoadAll<GameObject>("Prefabs/Character");
            Enemys = Resources.LoadAll<GameObject>("Prefabs/Enemy");
            Objects = Resources.LoadAll<GameObject>("Prefabs/Object");
            Clears = Resources.LoadAll<GameObject>("Prefabs/Clear");

            tool = (ToolManager)target;

            editors1 = new List<Editor>();
            for (int i = 0; i < Characters.Length; i++)
            {
                editors1.Add(null);
            }
            editors2 = new List<Editor>();
            for (int i = 0; i < Enemys.Length; i++)
            {
                editors2.Add(null);
            }
            editors3 = new List<Editor>();
            for (int i = 0; i < Objects.Length; i++)
            {
                editors3.Add(null);
            }
            editors4 = new List<Editor>();
            for (int i = 0; i < Clears.Length; i++)
            {
                editors4.Add(null);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("<", GUILayout.Width(25)))
            {
                tool.Level -= 1;
                if (tool.Level < 0)
                    tool.Level = 0;


                ClearAllObject();
                var dataObject = JsonUtility.FromJson<Game.Level>(LoadLevel(tool.Level)).LevelData;
                tool.AttackType = dataObject.AttackType;
                if(tool.AttackType == Enum.AttackType.Water)
                {
                    tool.WaterType = dataObject.WaterType;
                }      
                SpawnObject(dataObject);
                SaveData();
            }
            tool.Level = EditorGUILayout.IntField(tool.Level);
            if (GUILayout.Button(">", GUILayout.Width(25)))
            {
                var data = Resources.LoadAll<TextAsset>("Level");
                tool.Level += 1;
                if (tool.Level > data.Length - 1)
                    tool.Level = data.Length - 1;

                ClearAllObject();
                var dataObject = JsonUtility.FromJson<Game.Level>(LoadLevel(tool.Level)).LevelData;
                tool.AttackType = dataObject.AttackType;
                if (tool.AttackType == Enum.AttackType.Water)
                {
                    tool.WaterType = dataObject.WaterType;
                }
                SpawnObject(dataObject);
                SaveData();
            }

            if (GUILayout.Button("Load Level", GUILayout.Width(100)))
            {
                ClearAllObject();
                var dataObject = JsonUtility.FromJson<Game.Level>(LoadLevel(tool.Level)).LevelData;
                tool.AttackType = dataObject.AttackType;
                if (tool.AttackType == Enum.AttackType.Water)
                {
                    tool.WaterType = dataObject.WaterType;
                }
                SpawnObject(dataObject);
                SaveData();
            }

            GameObject instantiate_water = null;
            for(int i = 0; i < tool.Objects.Count; i++)
            {
                if(tool.Objects[i] == null)
                {
                    continue;
                }

                if(tool.Objects[i].name == "Water")
                {
                    instantiate_water = tool.Objects[i];
                    break;
                }
            }    

            if (tool.AttackType == Enum.AttackType.Water)
            {
                if (tool.WaterObject == null)
                {
                    tool.WaterObject = Resources.Load<GameObject>("Prefabs/Water");
                }

                if (instantiate_water == null)
                {
                    var water = Instantiate(tool.WaterObject);
                    water.name = "Water";
                    tool.Objects.Add(water);
                }
            }
            else
            {
                if (instantiate_water != null)
                {
                    tool.Objects.Remove(instantiate_water);
                    DestroyImmediate(instantiate_water);
                }
            }

            if (GUILayout.Button("Clear", GUILayout.Width(100)))
            {
                ClearAllObject();
            }
            GUILayout.EndHorizontal();
            clear = EditorGUILayout.Toggle("Clear level after save data: ", clear);
            active = EditorGUILayout.Toggle("Locked active tool: ", active);
            size = EditorGUILayout.Slider(size, 5, 20);
            tool.Camera.orthographicSize = size;
            tool.AttackType = (Enum.AttackType)EditorGUILayout.EnumPopup("Type of attack: ", tool.AttackType);
            if (tool.AttackType == Enum.AttackType.Water)
            {
                tool.WaterType = (Enum.WaterType)EditorGUILayout.EnumPopup("Direction of water: ", tool.WaterType);
            }
            GUILayout.Label("--------------------------------------");
            ObjectType = (Enum.ObjectType)EditorGUILayout.EnumPopup("Primitive to create:", ObjectType);
            switch (ObjectType)
            {
                case Enum.ObjectType.Character:
                    CharacterType = (Enum.CharacterType)EditorGUILayout.EnumPopup("Character to create:", CharacterType);
                    EditorGUILayout.Space();  
                    EditorGUILayout.BeginHorizontal();
                    scrollPos1 = EditorGUILayout.BeginScrollView(scrollPos1, GUILayout.Height(300));

                    for (int i = 0; i < Characters.Length; i++)
                    {
                        if (editors1[i] == null)
                            editors1[i] = Editor.CreateEditor(Characters[i]);
                        if (Characters[i].GetComponent<Game.Character>().CharacterType == CharacterType)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUIStyle bgColor = new GUIStyle();
                            EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                            editors1[i].OnPreviewGUI(GUILayoutUtility.GetRect(100, 100), bgColor);
                            editors1[i].OnInspectorGUI();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space(5);
                            EditorGUILayout.BeginVertical();
                            EditorGUILayout.LabelField("Name: " + Characters[i].name);
                            if (GUILayout.Button("Choose", GUILayout.Width(100)))
                            {
                                GameObject _object = Instantiate(Characters[i]);
                                _object.name = _object.name.Replace("(Clone)", "");
                                _object.transform.position = tool.Center.transform.position;
                                Selection.activeGameObject = _object;
                                tool.Objects.Add(_object);
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                        }
                    }
                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndHorizontal();
                    break;
                case Enum.ObjectType.Enemy:
                    if (tool.AttackType == Enum.AttackType.Water)
                    {
   
                    }
                    else
                    {
                        EnemyType = (Enum.EnemyType)EditorGUILayout.EnumPopup("Enemy to create:", EnemyType);
                        EditorGUILayout.Space();

                        EditorGUILayout.BeginHorizontal();
                        scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Height(300));

                        for (int i = 0; i < Enemys.Length; i++)
                        {
                            if (editors2[i] == null)
                                editors2[i] = Editor.CreateEditor(Enemys[i]);
                            if (Enemys[i].GetComponent<Game.Enemy>().EnemyType == EnemyType)
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                                GUIStyle bgColor = new GUIStyle();
                                editors2[i].OnPreviewGUI(GUILayoutUtility.GetRect(100, 100), bgColor);
                                editors2[i].OnInspectorGUI();
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.Space(5);
                                EditorGUILayout.BeginVertical();
                                EditorGUILayout.LabelField("Name: " + Enemys[i].name);
                                if (GUILayout.Button("Choose", GUILayout.Width(100)))
                                {
                                    GameObject _object = Instantiate(Enemys[i]);
                                    _object.name = _object.name.Replace("(Clone)", "");
                                    _object.transform.position = tool.Center.transform.position;
                                    Selection.activeGameObject = _object;
                                    tool.Objects.Add(_object);
                                }
                                EditorGUILayout.EndVertical();
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.Space();
                            }    
                        }

                        EditorGUILayout.EndScrollView();
                        EditorGUILayout.EndHorizontal();
                    }
                    break;
                case Enum.ObjectType.Object:
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    scrollPos3 = EditorGUILayout.BeginScrollView(scrollPos3, GUILayout.Height(300));

                    for (int i = 0; i < Objects.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                        GUIStyle bgColor = new GUIStyle();
                        if (editors3[i] == null)
                            editors3[i] = Editor.CreateEditor(Objects[i]);
                        editors3[i].OnPreviewGUI(GUILayoutUtility.GetRect(100, 100), bgColor);
                        editors3[i].OnInspectorGUI();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space(5);
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.LabelField("Name: " + Objects[i].name);
                        if (GUILayout.Button("Choose", GUILayout.Width(100)))
                        {
                            GameObject _object = Instantiate(Objects[i]);
                            _object.name = _object.name.Replace("(Clone)", "");
                            _object.transform.position = tool.Center.transform.position;
                            Selection.activeGameObject = _object;
                            tool.Objects.Add(_object);
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndHorizontal();
                    break;
                case Enum.ObjectType.Clear:
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    scrollPos4 = EditorGUILayout.BeginScrollView(scrollPos4, GUILayout.Height(300));

                    for (int i = 0; i < Clears.Length; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginHorizontal(GUILayout.Width(100));
                        GUIStyle bgColor = new GUIStyle();
                        if (editors4[i] == null)
                            editors4[i] = Editor.CreateEditor(Clears[i]);
                        editors4[i].OnPreviewGUI(GUILayoutUtility.GetRect(100, 100), bgColor);
                        editors4[i].OnInspectorGUI();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space(5);
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.LabelField("Name: " + Clears[i].name);
                        if (GUILayout.Button("Choose", GUILayout.Width(100)))
                        {
                            GameObject _object = Instantiate(Clears[i]);
                            _object.name = _object.name.Replace("(Clone)", "");
                            _object.transform.position = tool.Center.transform.position;
                            Selection.activeGameObject = _object;
                            tool.Objects.Add(_object);
                        }
                        EditorGUILayout.EndVertical();
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
            tool.LevelName = EditorGUILayout.TextField("Level Name: ", tool.LevelName);
            if (GUILayout.Button("Save Level"))
            {
                SaveData();  
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SaveData()
        {
            Game.Level info = new Game.Level();
            info.LevelData = new Game.LevelData();
            info.LevelData.SizeCamera = size;
            info.LevelData.AttackType = tool.AttackType;
            info.LevelData.WaterType = tool.WaterType;
            info.LevelData.Datas = new List<Game.ObjectData>();
            for (int i = 0; i < tool.Objects.Count; i++)
            {
                Game.ObjectData data = new Game.ObjectData();
                if(tool.Objects[i].name == "Water")
                {
                    data.ObjectType = Enum.ObjectType.None;
                    data.IsEnable = true;
                    data.CharacterType = Enum.CharacterType.None;
                    data.CharacterType = Enum.CharacterType.None;
                    data.NameObject = "Water";
                    data.Postion = tool.Objects[i].transform.position;
                    data.Rotation = tool.Objects[i].transform.rotation;
                    data.LocalScale = tool.Objects[i].transform.localScale;
                }
                else
                {
                    var entity = tool.Objects[i].GetComponent<Game.Entity>();
                    data.ObjectType = entity.ObjectType;
                    data.IsEnable = tool.Objects[i].activeSelf;
                    switch (data.ObjectType)
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
                }
                
                info.LevelData.Datas.Add(data);
            }
            if (!string.IsNullOrEmpty(tool.LevelName))
            {
                var _str = JsonUtility.ToJson(info);
                var path = "Assets/Resources/Level/" + tool.LevelName + ".json";
                Debug.Log(_str);
                Debug.Log(string.Format("path {0}", path));

                if (path.Length > 0)
                {
                    System.IO.File.WriteAllText(path, _str);
                }

                EditorUtility.SetDirty(this);
                AssetDatabase.Refresh();

                if (clear)
                {
                    ClearAllObject();
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Chưa có tên", "Chưa nhập tên level chị oiiiiiii, nhập nhé...", "Okayy");
            }
        }
        
        private void ClearAllObject()
        {
            Debug.Log("Clear All Object");
            if (tool.Objects != null)
            {
                for (int i = 0; i < tool.Objects.Count; i++)
                {
                    DestroyImmediate(tool.Objects[i]);
                }
                tool.Objects = new List<GameObject>();
            }
        }

        private string LoadLevel(int value)
        {
            string path = string.Format("Level/level_{0}", value + 1);
            var level = Resources.Load<TextAsset>(path);
            Debug.Log(level.text);
            tool.LevelName = level.name;
            return level.text;
        }

        private void SpawnObject(Game.LevelData data)
        {
            var objects = data.Datas;
            string path = "";
            GameObject obj = null;
            for(int i = 0; i < objects.Count; i++)
            {
                switch(objects[i].ObjectType)
                {
                    case Enum.ObjectType.None:
                        path = string.Format("Prefabs/Water");
                        obj = Instantiate(Resources.Load<GameObject>(path));
                        obj.name = obj.name.Replace("(Clone)", "");
                        obj.transform.localPosition = objects[i].Postion;
                        obj.transform.localRotation = objects[i].Rotation;
                        obj.transform.localScale = objects[i].LocalScale;
                        obj.SetActive(objects[i].IsEnable);
                        break;
                    case Enum.ObjectType.Character:
                        path = string.Format("Prefabs/Character/{0}", objects[i].NameObject);
                        obj = Instantiate(Resources.Load<GameObject>(path));
                        obj.name = obj.name.Replace("(Clone)", "");
                        obj.transform.localPosition = objects[i].Postion;
                        obj.transform.localRotation = objects[i].Rotation;
                        obj.transform.localScale = objects[i].LocalScale;
                        obj.SetActive(objects[i].IsEnable);
                        break;
                    case Enum.ObjectType.Enemy:
                        path = string.Format("Prefabs/Enemy/{0}", objects[i].NameObject);
                        obj = Instantiate(Resources.Load<GameObject>(path));
                        obj.name = obj.name.Replace("(Clone)", "");
                        obj.transform.localPosition = objects[i].Postion;
                        obj.transform.localRotation = objects[i].Rotation;
                        obj.transform.localScale = objects[i].LocalScale;
                        obj.SetActive(objects[i].IsEnable);
                        break;
                    case Enum.ObjectType.Object:
                        path = string.Format("Prefabs/Object/{0}", objects[i].NameObject);
                        obj = Instantiate(Resources.Load<GameObject>(path));
                        obj.name = obj.name.Replace("(Clone)", "");
                        obj.transform.localPosition = objects[i].Postion;
                        obj.transform.localRotation = objects[i].Rotation;
                        obj.transform.localScale = objects[i].LocalScale;
                        obj.SetActive(objects[i].IsEnable);
                        break;
                    case Enum.ObjectType.Clear:
                        path = string.Format("Prefabs/Clear/{0}", objects[i].NameObject);
                        obj = Instantiate(Resources.Load<GameObject>(path));
                        obj.name = obj.name.Replace("(Clone)", "");
                        obj.transform.localPosition = objects[i].Postion;
                        obj.transform.localRotation = objects[i].Rotation;
                        obj.transform.localScale = objects[i].LocalScale;
                        obj.SetActive(objects[i].IsEnable);
                        break;
                }

                tool.Objects.Add(obj);
            }
        }
    }
}

