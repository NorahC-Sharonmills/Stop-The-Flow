using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ToolSetupCharacter : MonoBehaviour
{
    public GameObject Head;
    public string FindName;
}

#if UNITY_EDITOR
[CustomEditor(typeof(ToolSetupCharacter))]
public class ToolSetupCharacterEditor : Editor
{
    ToolSetupCharacter main;
    private void OnEnable()
    {
        main = (ToolSetupCharacter)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Find"))
        {
            for (int i = 0; i < main.transform.childCount; i++)
            {
                FindInGO(main.transform.GetChild(i).gameObject);
                if(i < 5)
                {
                    Vector3 pos = main.transform.GetChild(i).localPosition;
                    pos.y = -0.1f;
                    main.transform.GetChild(i).localPosition = pos;
                }   
                else
                {
                    Vector3 pos = main.transform.GetChild(i).localPosition;
                    pos.y = -2.1f;
                    main.transform.GetChild(i).localPosition = pos;
                }    
            }
        }    
    }

    private void FindInGO(GameObject g)
    {
        if(g.name == main.FindName)
        {
            Debug.Log(g.name);
            if(g.transform.childCount == 0)
            {
                GameObject ga = Instantiate(main.Head, g.transform);
                ga.transform.localPosition = Vector3.zero;
                ga.transform.localRotation = Quaternion.identity;
                ga.SetActive(true);
            }    
        }    

        // Now recurse through each child GO (if there are any):
        foreach (Transform childT in g.transform)
        {
            //Debug.Log("Searching " + childT.name  + " " );
            FindInGO(childT.gameObject);
        }
    }
}
#endif
