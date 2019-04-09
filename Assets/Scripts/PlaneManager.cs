using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour {
    
    public float scanTime;
    private float timeSinceLastRefresh;

    private bool planesCreated;

	// Use this for initialization
	void Start () {
        timeSinceLastRefresh = Time.time;
        scanTime = 10.0f;
        planesCreated = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!planesCreated && (Time.time - timeSinceLastRefresh) > scanTime)
        {
            //planesCreated = true;
            CreatePlanes();

            timeSinceLastRefresh = Time.time;
        }
	}

    private void CreatePlanes()
    {
        // Generate planes based on the spatial map.
        SurfaceMeshesToPlanes surfaceToPlanes = SurfaceMeshesToPlanes.Instance;
        if (surfaceToPlanes != null && surfaceToPlanes.enabled)
        {
            surfaceToPlanes.MakePlanes();
        }
    }
}
