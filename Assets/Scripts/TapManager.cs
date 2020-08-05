using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine;

public class TapManager : MonoBehaviour
{

    [SerializeField]
    public GameObject objectToPlace;
    private ARRaycastManager aRRaycastManager;
    private Pose pose;
    private ARAnchorManager aRAnchorManager;
    private EventSystem eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRAnchorManager = GetComponent<ARAnchorManager>();
        eventSystem = FindObjectOfType<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if((eventSystem == null || !eventSystem.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) && (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
           PlaceAnchornObject();
        }
    }
    
    private void PlaceObject()
    {
       
        if(isValidPositionPhyRaycast())
        {
            Instantiate(objectToPlace,pose.position,pose.rotation);
        }
    }
    private void PlaceAnchor()
    {
        if(isValidPositionPhyRaycast())
        {
            ARAnchor anchor = aRAnchorManager.AddAnchor(pose);
        } 
    }
    private void PlaceAnchornObject()
    {
        if(isValidPositionPhyRaycast())
        {
            ARAnchor anchor = aRAnchorManager.AddAnchor(pose);
            GameObject placedObject = Instantiate(objectToPlace,anchor.transform.position,anchor.transform.rotation);
            placedObject.transform.parent = anchor.gameObject.transform;

        }
    }
    private bool isValidPositionARRaycast()
    {
        bool result = false;
        var rayOrigin = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        List<ARRaycastHit> rayCastHits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(rayOrigin, rayCastHits,TrackableType.Planes);

        if(rayCastHits.Count > 0)
        {
            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x,0,cameraForward.z).normalized;
            result = true;
            pose = rayCastHits[0].pose;
            pose.rotation = Quaternion.LookRotation(cameraBearing);
        }
        return result;
    }
     private bool isValidPositionPhyRaycast()
    {
        bool result = false;
        int hitMask = 1 << 10;
        var rayOrigin = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit[] rayCastHits = new RaycastHit[10];
        var physicsRayCastHits =  Physics.RaycastNonAlloc(rayOrigin,rayCastHits,Mathf.Infinity,hitMask);

        if(rayCastHits[0].collider != null)
        {
            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x,0,cameraForward.z).normalized;
            result = true;
            pose = new Pose(rayCastHits[0].point,Quaternion.LookRotation(cameraBearing));
        }
        return result;
    }

    void OnEnabled()
    {

    }

}
