using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARAnchorManager))]
public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject[] copyTarget = new GameObject[3];
    private GameObject Dog;
    private GameObject targetGoal_Now;
    public GameObject targetGoal;
    public Text text;

    private GameObject selectedTarget;

    private GameObject spwandObject;
    private GameObject selectObject;
    private ARRaycastManager _arRaycastManager;
    private ARAnchorManager _arAnchorManager;
    private List<ARAnchor> placedAnchors = new List<ARAnchor>(); //앵커를 추적하기 위한 목록
    private Vector2 touchPosition;

    private float rotateY = 0;
    private float scaleFactor = 1;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
        _arAnchorManager = GetComponent<ARAnchorManager>();
    }

    void Start()
    {
        selectedTarget = copyTarget[0];
    }

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition)) return;
        if (EventSystem.current.IsPointerOverGameObject() == true) return;
        if (EventSystem.current.IsPointerOverGameObject(0) == true) return;

        if (_arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if (modeChg.mode == 1)
            {
                Quaternion e = Quaternion.Euler(0, 0, 0);
                if (targetGoal_Now == null)
                {
                    ARAnchor anchor = _arAnchorManager.AddAnchor(hitPose);
                    targetGoal_Now = Instantiate(targetGoal, hitPose.position, e);
                    targetGoal_Now.transform.parent = anchor.transform;
                }
                else
                {
                    targetGoal_Now.transform.position = hitPose.position;
                }

                text.text = ":" + targetGoal_Now.transform.position.ToString() + " " + Dog.transform.position.ToString();
                Dog.GetComponent<DogMove>().FindTargetGoal(targetGoal);
                return;
            }

            if (spwandObject == null)
            {
                ARAnchor anchor = _arAnchorManager.AddAnchor(hitPose);
                Quaternion e = Quaternion.Euler(0, 0, 0);
                spwandObject = Instantiate(selectedTarget, hitPose.position, e);
                spwandObject.transform.parent = anchor.transform;
                Dog = GameObject.FindGameObjectWithTag("Dog");
            }
            else
            {
                spwandObject.transform.position = hitPose.position;
            }
        }
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    public void changeObject(int i)
    {
        selectedTarget = copyTarget[i];
        if (spwandObject)
        {
            if (spwandObject.CompareTag("Dog"))
                Destroy(spwandObject);
        }
        spwandObject = null;
    }

    public void changeRotateY(Slider s)
    {
        rotateY = s.value;
        if (spwandObject) spwandObject.transform.eulerAngles = new Vector3(0, rotateY, 0);
    }

    public void changeScaleFactor(Slider s)
    {
        scaleFactor = s.value;
        if (spwandObject) spwandObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
}
