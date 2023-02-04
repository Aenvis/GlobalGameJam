using System;
using System.Collections.Generic;
using UnityEngine;

public class Marker
{
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }

    public Marker(Vector3 pos, Quaternion rot)
    {
        Position = pos;
        Rotation = rot;
    }
}

public class MarkerManager : MonoBehaviour
{
    private List<Marker> _markers = new List<Marker>();

    private void FixedUpdate()
    {
        UpdateMarkers();
    }

    public List<Marker> GetMarkers() => _markers;
    
    public void UpdateMarkers()
    {
        _markers.Add(new Marker(transform.position, transform.rotation));
    }

    public void ClearMarkers()
    {
        _markers.Clear();
        _markers.Add(new Marker(transform.position, transform.rotation));
    }
}
