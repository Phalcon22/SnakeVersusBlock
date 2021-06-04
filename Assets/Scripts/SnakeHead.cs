using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : SnakePart
{
    float startSnakeX;
    float startMouseX;

    float targetX;

    Vector3 topLeft;
    Vector3 topRight;
    Vector3 bottomLeft;
    Vector3 bottomRight;

    Rigidbody rb;

    public void Init()
    {
        rb = GetComponent<Rigidbody>();
        Collider collider = GetComponent<BoxCollider>();
        float sizeX = collider.bounds.extents.x - 0.00f;
        float sizeZ = collider.bounds.extents.z - 0.01f;
        topLeft = new Vector3(sizeX, 0, sizeZ);
        topRight = new Vector3(-sizeX, 0, sizeZ);
        bottomLeft = new Vector3(sizeX, 0, -sizeZ);
        bottomRight = new Vector3(-sizeX, 0, -sizeZ);

        targetX = rb.position.x;
    }

    public void UpdateMouse()
    {
        if (Input.GetMouseButtonDown(0))
            OnButtonDown();
        else if (Input.GetMouseButton(0))
            OnButtonHoldDown();
    }

    public bool Move()
    {
        Vector3 translation = new Vector3(GetXTranslation(), 0, GetYTranslation());
        GetComponent<Rigidbody>().velocity = translation * 30;

        return translation.z > 0;
    }

    float GetMouseX()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y - rb.position.y;
        return Camera.main.ScreenToWorldPoint(mousePos).x;
    }

    void OnButtonDown()
    {
        startMouseX = GetMouseX();
        startSnakeX = rb.position.x;
        OnButtonHoldDown();
    }

    void OnButtonHoldDown()
    {
        float endMouseX = GetMouseX();
        targetX = startSnakeX + (endMouseX - startMouseX);
    }

    float GetXTranslation()
    {
        Vector3 pos = rb.position;
        
        float direction = (pos.x < targetX) ? 1 : -1;
        float translation = direction * Snake.horizontalSpeed;

        if (direction < 0 && pos.x + translation < targetX || direction > 0 && pos.x + translation > targetX)
        {
           translation = targetX - pos.x;
        }

        if (left.Count > 0 && translation < 0 || right.Count > 0 && translation > 0)
            translation = 0;

        if (left.Count != 0 && targetX - pos.x  < 0 || right.Count != 0 && targetX - pos.x > 0)
        {
            targetX = rb.position.x;
            return 0;
        }

        return translation;
    }

    float GetYTranslation()
    {
        if (front.Count > 0)
            return 0;

        return Snake.verticalSpeed;
    }

/// <summary>
/// Collision
/// </summary>

    List<Collider> front = new List<Collider>();
    List<Collider> left = new List<Collider>();
    List<Collider> right = new List<Collider>();

    bool OnSide(Collider collision, Vector3 pos, Vector3 dir, string layer)
    {
        RaycastHit hit;
        return Physics.Raycast(rb.position + pos, dir, out hit, 100, LayerMask.GetMask(layer)) && hit.collider == collision;
    }

    bool OnLeft(Collider collision, string layer)
    {
        return OnSide(collision, topLeft, Vector3.left, layer) || OnSide(collision, bottomLeft, Vector3.left, layer)
            || OnSide(collision, Vector3.zero, Vector3.left, layer);
    }

    bool OnRight(Collider collision, string layer)
    {
        return OnSide(collision, topRight, Vector3.right, layer) || OnSide(collision, bottomRight, Vector3.right, layer)
            || OnSide(collision, Vector3.zero, Vector3.right, layer);
    }

    void OnCollisionEnter(Collision collision)
    {
       /* Collider collider = collision.collider;
        string layer = LayerMask.LayerToName(collision.gameObject.layer);

        if (layer == "Wall" || layer == "Block")
        {
            if (OnLeft(collider, layer))
                left.Add(collider);
            else if (OnRight(collider, layer))
                right.Add(collider);
            else if (collider.transform.position.z > rb.position.z)
                front.Add(collider);
        }*/
    }

    void OnCollisionExit(Collision collision)
    {
        Collider collider = collision.collider;
        string layer = LayerMask.LayerToName(collision.gameObject.layer);
        
        if (layer == "Wall" || layer == "Block")
        {
            left.Remove(collider);
            right.Remove(collider);
            front.Remove(collider);
        }
    }
}
