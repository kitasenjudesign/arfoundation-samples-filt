using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
public class ReferencePointCreator : MonoBehaviour
{
    public void RemoveAllReferencePoints()
    {
        foreach (var referencePoint in m_ReferencePoints)
        {
            m_ReferencePointManager.RemoveAnchor(referencePoint);
        }
        m_ReferencePoints.Clear();
    }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_ReferencePointManager = GetComponent<ARAnchorManager>();
        m_ReferencePoints = new List<ARAnchor>();
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;

        if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.FeaturePoint))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;
            var referencePoint = m_ReferencePointManager.AddAnchor(hitPose);
            if (referencePoint == null)
            {
                Logger.Log("Error creating reference point");
            }
            else
            {
                m_ReferencePoints.Add(referencePoint);
            }
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    List<ARAnchor> m_ReferencePoints;

    ARRaycastManager m_RaycastManager;

    ARAnchorManager m_ReferencePointManager;
}
