using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainEventTrigger : EventTrigger
{

    #region Fields

    private Camera camera;
    private int cardLayerMask;
    private Plane plane;

    private GameObject draggedObject;

    private Vector3 startDragPos;
    private Vector3 lastDragPos;

    #endregion

    #region Properties

    public Vector3 NormalizedDragDirection => (lastDragPos - startDragPos).normalized;

    #endregion

    #region Mono

    private void Start()
    {
        camera = Camera.main;
        cardLayerMask = LayerMask.GetMask("card");
        // plane going through first card and towards camera
        plane = new Plane(new Vector3(0f, 0f, -1f), new Vector3(0f, 18f, 20f));
        
        draggedObject = null;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.I.IsMainMenu)
        {
            GameManager.I.NewGame();
        }
        else if (GameManager.I.IsEndGame)
        {
            GameManager.I.ShowMainMenu();
        }
    }

    public override void OnBeginDrag(PointerEventData data)
    {
        if (!GameManager.I.IsPlaying)
            return;
        
        var ray = camera.ScreenPointToRay(data.position);
        if (Physics.Raycast(ray, out var hit, 1000f, cardLayerMask))
        {
            draggedObject = hit.transform.gameObject;
            
            if (plane.Raycast(ray, out var enterDistance))
            {
                startDragPos = ray.GetPoint(enterDistance);
            }
        }
    }
    
    public override void OnDrag(PointerEventData data)
    {
        if (draggedObject == null || !GameManager.I.IsPlaying)
            return;
        
        var ray = camera.ScreenPointToRay(data.position);
        if (plane.Raycast(ray, out var enterDistance))
        {
            Vector3 hitPoint = ray.GetPoint(enterDistance);
            draggedObject.transform.position = hitPoint;
            lastDragPos = hitPoint;
        }
    }
    
    public override void OnEndDrag(PointerEventData data)
    {
        if (draggedObject != null && GameManager.I.IsPlaying)
        {
            GameManager.I.ReleaseCard();
        }
        draggedObject = null;
    }
    
    #endregion
}
