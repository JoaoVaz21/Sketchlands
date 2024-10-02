using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class DrawableObject : MonoBehaviour
{
    public Action CollidedWhileCreating;
    [SerializeField] private float massPerVertice = 0.1f;
    [SerializeField] private float maxMass = 20f;
    [SerializeField] private float maxMassToBecomeUnpushable = 30;

    private Mesh _mesh;

    private Rigidbody2D _rb;

    private PolygonCollider2D _polygonCollider;
    private List<Vector3> _mousepoints = new List<Vector3>();
    
    public int InkUsed;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
    }

    // Initialize the drawing by making a mesh and make it's first vertices the initial position
    public void InitializeDrawing(Vector3 initialPosition)
    {
        _rb.bodyType = RigidbodyType2D.Static;
        _mesh = new Mesh();
        var material = GetComponent<MeshRenderer>().material;
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];
        vertices[0] = initialPosition;
        vertices[1] = initialPosition;
        vertices[2] = initialPosition;
        vertices[3] = initialPosition;

        uv[0] = Vector2.zero;
        uv[1] = Vector2.zero;
        uv[2] = Vector2.zero;
        uv[3] = Vector2.zero;

        triangles[0] = 0;
        triangles[1] = 3;
        triangles[2] = 1;
           
        triangles[3] = 1;
        triangles[4] = 3;
        triangles[5] = 2;

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
        _mesh.MarkDynamic();
        GetComponent<MeshFilter>().mesh = _mesh;
        GetComponent<MeshRenderer>().material = material;
    }

    private void CheckCollisionWhileGenerating()
    {
        var upVertices = new List<Vector3>();
        var downVertices = new List<Vector3>();
        for (var i = 0; i < _mesh.vertices.Length; i++)
        {
            if (i % 2 == 0)
            {
                upVertices.Add(_mesh.vertices[i]);
            }
            else
            {
                downVertices.Add(_mesh.vertices[i]);
            }
        }
        downVertices.Reverse();
        upVertices.AddRange(downVertices);




        _polygonCollider.points = MathHelper.Vector3ToVector2(upVertices.ToArray());
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false; // Ignore triggers if necessary

        // Create an array to store the results
        Collider2D[] results = new Collider2D[10]; // Adjust the size as needed

        // Check for collisions
        int collisionCount = _polygonCollider.OverlapCollider(contactFilter, results);

        if (collisionCount > 0)
        {
            CollidedWhileCreating?.Invoke();
        }
    }
    public void CompleteMesh()
    {
        var upVertices = new List<Vector3>();
        var downVertices = new List<Vector3>();
        for (var i = 0; i < _mesh.vertices.Length; i++)
        {
            if (i % 2 == 0)
            {
                upVertices.Add(_mesh.vertices[i]);
            }
            else
            {
                downVertices.Add(_mesh.vertices[i]);
            }
        }
        downVertices.Reverse();
        upVertices.AddRange(downVertices);
        
        
        
        
        _polygonCollider.points = MathHelper.Vector3ToVector2(upVertices.ToArray());
        var renderer = GetComponent<MeshRenderer>();
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();
        var mass = GetDrawnVerticesCount() * massPerVertice;
        _rb.mass = mass>maxMass?maxMass:mass;
        _rb.bodyType =RigidbodyType2D.Kinematic;
        if (_rb.mass > maxMassToBecomeUnpushable)
        {
            _rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
    //update the next point of the drawing using a position, a vector and a line thickness to calculate the meshes vertices
    public void AddVerticesToMesh(Vector3 position, Vector3 forwardVector,float lineThickness)
    {
        _mousepoints.Add(position);
        if (_mousepoints.Count < 2) return;

        Vector3[] vertices = new Vector3[_mousepoints.Count * 2];
        Vector2[] uv = new Vector2[_mousepoints.Count * 2];
        int[] triangles = new int[(_mousepoints.Count - 1) * 6];

        for (int i = 0; i < _mousepoints.Count; i++)
        {
            Vector3 forward = Vector3.zero;
            if (i < _mousepoints.Count - 1)
            {
                forward = (_mousepoints[i + 1] - _mousepoints[i]).normalized;
            }
            else if (i > 0)
            {
                forward = (_mousepoints[i] - _mousepoints[i - 1]).normalized;
            }

            Vector3 right = new Vector3(-forward.y, forward.x, 0) * lineThickness / 2;
            vertices[i * 2] = _mousepoints[i] - right;
            vertices[i * 2 + 1] = _mousepoints[i] + right;

            float distance = (i > 0) ? Vector3.Distance(_mousepoints[i], _mousepoints[i - 1]) : 0f;
            float uvOffset = (i > 0) ? uv[(i - 1) * 2].y + distance / lineThickness : 0f;

            uv[i * 2] = new Vector2( uvOffset,0);
            uv[i * 2 + 1] = new Vector2(uvOffset,1);

            if (i < _mousepoints.Count - 1)
            {
                triangles[i * 6] = i * 2;
                triangles[i * 6 + 1] = i * 2 + 2;
                triangles[i * 6 + 2] = i * 2 + 1;
                triangles[i * 6 + 3] = i * 2 + 1;
                triangles[i * 6 + 4] = i * 2 + 2;
                triangles[i * 6 + 5] = i * 2 + 3;
            }
        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
        this.CheckCollisionWhileGenerating();
    }

    public int GetDrawnVerticesCount()
    {
        return _mesh.vertices.Length - 4;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _rb.velocity = Vector2.zero;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
