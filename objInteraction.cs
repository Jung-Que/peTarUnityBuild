using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
public class objInteraction : MonoBehaviour
{
    private GameObject spwandObject;
    public GameObject yesno;
    private GameObject[] tools;
    public Text text;

    private ARRaycastManager _arRaycastManager;
    // Start is called before the first frame update

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Start()
    {
        text.text = "Name: Detroy?";
    }

    // Update is called once per frame
    void Update()
    {
        if(modeChg.mode == 0) return;
        //if(!TryGetTouchPosition(out Vector2 touchPosition)) return;

        Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit raycastHit;

         if (Physics.Raycast(raycast, out raycastHit))
        {
            if (EventSystem.current.IsPointerOverGameObject() == true) return; // ui 클릭시 작동 막기
            if (EventSystem.current.IsPointerOverGameObject(0) == true) return;
            Vector3 hitPose = raycastHit.point;

            if(raycastHit.collider.tag == "Tool")
            {
                spwandObject = raycastHit.collider.gameObject;
                yesno.SetActive(true);
                text.text = spwandObject.name + ": Detroy?";
            }
            else if(raycastHit.collider.tag == "Dog")
            {
                FollowMe dogMoveComponent = raycastHit.collider.GetComponent<FollowMe>();
                if (dogMoveComponent != null)
                {
                   dogMoveComponent.TriggerSneeze();
                }
            }
        }
    }
    public void destroyObject()
    {
        if(spwandObject) Destroy(spwandObject); // 있으면 삭제
        yesno.SetActive(false);
        spwandObject = null;
    }
    public void interactionTool(GameObject tools)
    {
                spwandObject = tools;
                yesno.SetActive(true);
                text.text = spwandObject.name + ": Detroy?";

    }
    public void cancled()
    {
        yesno.SetActive(false);
    }
    public void detroyAll()
    {
        tools = GameObject.FindGameObjectsWithTag("Tool");
        for(int i=0; i<tools.Length; i++)
            {
                Destroy(tools[i]);
            }
        tools = GameObject.FindGameObjectsWithTag("Dog");
        for(int i=0; i<tools.Length; i++)
            {
                Destroy(tools[i]);
            }     
    }

    //    bool TryGetTouchPosition(out Vector2 touchPosition)
    // {
    //     //if(Input.touchCount > 0)
    //     if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //     {
    //         touchPosition = Input.GetTouch(0).position;
    //         return true;
    //     }

    //     touchPosition = default;
    //     return false;
    // }
}
