using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : SnakePart
{
    float startSnakeX;
    float startMouseX;

    float targetX;

    BoxCollider col;

    string[] obstacleLayers = { "Wall", "Block" };

    public List<Vector3> posHistory = new List<Vector3>();
    public List<List<float>> deltasHistory = new List<List<float>>();

    public void Init()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
    }

    public void UpdateMouse()
    {
        if (Input.GetMouseButtonDown(0))
            OnButtonDown();
        else if (Input.GetMouseButton(0))
            OnButtonHoldDown();
    }

    void Translate(Vector3 translation)
    {
        rb.MovePosition(rb.position + translation);
    }

    public Vector3 GetPos()
    {
        return rb.position;
    }

    public bool Move()
    {
        Vector3 translation = new Vector3(GetXTranslation(), 0, GetYTranslation());
        Vector3 h = new Vector3(translation.x, 0, 0);
        Vector3 v = new Vector3(0, 0, translation.z);

        Translate(h);
        var horizontalColliders = Physics.OverlapBox(col.bounds.center, col.bounds.extents, Quaternion.identity, LayerMask.GetMask(obstacleLayers));

        if (horizontalColliders.Length > 0)
        {
            targetX = rb.position.x;
            Translate(-h);
            translation.x = 0;
        }

        Translate(v);
        var verticalColliders = Physics.OverlapBox(col.bounds.center, col.bounds.extents, Quaternion.identity, LayerMask.GetMask(obstacleLayers));

        if (verticalColliders.Length > 0)
        {
            Translate(-v);
            translation.z = 0;
        }

        posHistory.Add(rb.position);
        List<float> f = new List<float>();
        for (int i = 0; i < Snake.followers; i++)
            f.Add(Time.deltaTime);
        deltasHistory.Add(f);

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
        float translation = direction * Snake.horizontalSpeed * Time.deltaTime;

        if (direction < 0 && pos.x + translation < targetX || direction > 0 && pos.x + translation > targetX)
        {
           translation = targetX - pos.x;
        }

        return translation;
    }

    float GetYTranslation()
    {
        return Snake.verticalSpeed * Time.deltaTime;
    }
}
