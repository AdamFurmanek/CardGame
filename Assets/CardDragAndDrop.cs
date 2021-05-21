using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera mainCamera;
    Vector3 originalPosition;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //https://gist.github.com/SimonDarksideJ/477f5674285b63cba8e752c43950ed7c
        Ray r = mainCamera.ScreenPointToRay(Input.mousePosition); // Get the ray from mouse position
        Vector3 PO = transform.position; // Take current position of this draggable object as Plane's Origin
        Vector3 PN = -mainCamera.transform.forward; // Take current negative camera's forward as Plane's Normal
        float t = Vector3.Dot(PO - r.origin, PN) / Vector3.Dot(r.direction, PN); // plane vs. line intersection in algebric form. It find t as distance from the camera of the new point in the ray's direction.
        Vector3 P = r.origin + r.direction * t; // Find the new point.

        transform.position = P;

        //TU KONTYNUOWAÆ
        RaycastHit2D[] hit = Physics2D.GetRayIntersectionAll(r, Mathf.Infinity);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = originalPosition;
    }

}
