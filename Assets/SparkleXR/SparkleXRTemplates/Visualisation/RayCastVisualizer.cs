using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SparkleXRTemplates;

public class RayCastVisualizer : MonoBehaviour
{
    [SerializeField]
    RaycastSelector RayCasterToVisualize;

    static float trasparency = 1f;
    
    [SerializeField]
    Color color = Color.clear;

    LineRenderer lineRend;
    void Start()
    {
        lineRend = gameObject.AddComponent<LineRenderer>();
        if(color == Color.clear)
            color = new Vector4(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), trasparency);

        lineRend.material.color = color;
        
    }

    private void Update()
    {
        Vector3 normDirection = (RayCasterToVisualize.director.transform.position - RayCasterToVisualize.castSource.transform.position).normalized;
        lineRend.SetPosition(0, RayCasterToVisualize.castSource.transform.position);
        lineRend.SetPosition(1, RayCasterToVisualize.castSource.transform.position + normDirection * RayCasterToVisualize.maxDistance);
        lineRend.widthMultiplier = 0.03f;
    }

    void HighlightHovered(RaycastHit[] hits)
    {
        //TODO:...
    }

}
