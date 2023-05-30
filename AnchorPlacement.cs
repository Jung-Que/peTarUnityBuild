using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AnchorPlacement : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    private ARRaycastManager raycastManager;
    private ARAnchorManager anchorManager; // ARAnchorManager 추가
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>(); // ARAnchorManager 할당
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    ARAnchor anchor = anchorManager.AddAnchor(hitPose); // 인스턴스화된 ARAnchorManager 개체의 AddAnchor 메서드 사용
                    GameObject cube = Instantiate(cubePrefab, hitPose.position, hitPose.rotation);
                    cube.transform.parent = anchor.transform;
                }
            }
        }
    }
}
