using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class DrawHandle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameObject LineGO;

    bool StartDrawing;

    Vector3 MousePos;

    LineRenderer LR;

    [SerializeField]
    Material LineMat;

    int CurrentIndex;

    [SerializeField]
    Camera cam;

    [SerializeField]
    Transform Collider_Prefab;

    [SerializeField]
    GameObject Line_Prefabs;

    [SerializeField, Range(0f, 2f)]
    float Scale_Prefab = 1f;

    [SerializeField, Range(0f, 10000f)]
    float Distance_Prefab = 1f;

    Transform LastInstantiated_Collider;

    public List<GameObject> actions = new List<GameObject>();

    public void OnPointerDown(PointerEventData eventData)
    {
        StartDrawing = true;
        MousePos = Input.mousePosition;
        LR = LineGO.GetComponent<LineRenderer>();
        if (LR == null)
            LR = LineGO.AddComponent<LineRenderer>();
        LR.startWidth = 0.2f;
        LR.material = LineMat;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartDrawing = false;
        Rigidbody rb = LineGO.GetComponent<Rigidbody>();
        if (rb == null)
            rb = LineGO.AddComponent<Rigidbody>();
        actions.Add(LineGO);
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        LR.useWorldSpace = false;
        if (LastInstantiated_Collider != null)
            Destroy(LastInstantiated_Collider.gameObject);
        CreateLineGo();
        CurrentIndex = 0;
    }

    void Start()
    {
        CreateLineGo();
    }

    void CreateLineGo()
    {
        //LineGO = PoolByID.Instance.GetPrefab(Line_Prefabs);
        LineGO = Instantiate(Line_Prefabs);
    }

    void FixedUpdate()
    {
        if (StartDrawing)
        {
            Vector3 Dist = MousePos - Input.mousePosition;
            float Distance_SqrMag = Dist.sqrMagnitude;
            if (Distance_SqrMag > Distance_Prefab)
            {
                LR.SetPosition(CurrentIndex, cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 1f)));
                if (LastInstantiated_Collider != null)
                {
                    Vector3 CurLinePos = LR.GetPosition(CurrentIndex);
                    LastInstantiated_Collider.gameObject.SetActive(true);
                    LastInstantiated_Collider.LookAt(CurLinePos);
                    if (LastInstantiated_Collider.rotation.y == 0)
                    {
                        LastInstantiated_Collider.eulerAngles = new Vector3(LastInstantiated_Collider.rotation.eulerAngles.x, 90, LastInstantiated_Collider.rotation.eulerAngles.z);
                    }
                    LastInstantiated_Collider.localScale = new Vector3(LastInstantiated_Collider.localScale.x, LastInstantiated_Collider.localScale.y, Vector3.Distance(LastInstantiated_Collider.position, CurLinePos) * Scale_Prefab);
                }
                //LastInstantiated_Collider = PoolByID.Instance.GetPrefab<Transform>(Collider_Prefab.gameObject, LR.GetPosition(CurrentIndex), Quaternion.identity, LineGO.transform);
                LastInstantiated_Collider = Instantiate(Collider_Prefab, LR.GetPosition(CurrentIndex), Quaternion.identity, LineGO.transform);
                LastInstantiated_Collider.gameObject.SetActive(false);
                MousePos = Input.mousePosition;
                CurrentIndex++;
                LR.positionCount = CurrentIndex + 1;
                LR.SetPosition(CurrentIndex, cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10f)));
            }
        }
    }

    bool removing = false;
    void LateUpdate()
    {
        if (!removing)
        {
            if(actions.Count > 0 && actions[0] != null)
            {
                removing = true;
                CoroutineUtils.PlayCoroutine(() =>
                {
                    Destroy(actions[0]);
                    actions.RemoveAt(0);
                    removing = false;
                }, 1f);
                //CoroutineUtils.PlayManyCoroutine(2f, 0.5f, () => {
                //    int count_while = 0;
                //    do
                //    {
                //        PoolByID.Instance.PushToPool(actions[0].transform.GetChild(0).gameObject);
                //        count_while += 1;
                //        if (count_while > 1000)
                //            break;
                //    } while (actions[0].transform.childCount > 0);
                //}, () => {
                //    PoolByID.Instance.PushToPool(actions[0]);
                //    Destroy(actions[0].GetComponent<Rigidbody>());
                //    actions.RemoveAt(0);
                //    removing = false;
                //});
            }
            else
            {
                if (actions.Count > 0 && actions[0] == null)
                {
                    actions.RemoveAt(0);
                    removing = false;
                }
                    //actions.RemoveAt(0);
            }
        }
    }
}
