using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SparkleXRTemplates;

public class SphereCastVisualizer : MonoBehaviour
{
    [SerializeField]
    SpherecastSelector SphereCasterToVisualize;

    [SerializeField]
    GameObject CylinderModel;
    [SerializeField]
    GameObject SphereModel;
    [SerializeField]
    GameObject Wrapper;

    GameObject VisualizatorWrapper;
    GameObject CylinderVisualizer;
    GameObject[] SphereModels = new GameObject[2] { null, null};

    static float trasparency = 0.05f;
    
    [SerializeField]
    Color color = Color.clear;

    Vector3 normDirection;
    void Start()
    {
        SphereCasterToVisualize.spherecastHitInfo += HighlightHovered;
        VisualizatorWrapper = Instantiate(Wrapper);
        SphereModels[0] = Instantiate(SphereModel, VisualizatorWrapper.transform);
        SphereModels[1] = Instantiate(SphereModel, VisualizatorWrapper.transform);
        SphereModels[0].transform.localPosition = new Vector3(0f, 0f, SphereCasterToVisualize.maxDistance / 2f);
        SphereModels[1].transform.localPosition = new Vector3(0f, 0f, - SphereCasterToVisualize.maxDistance / 2f);
        SphereModels[0].transform.localScale = new Vector3(SphereCasterToVisualize.radius * 2, SphereCasterToVisualize.radius * 2, SphereCasterToVisualize.radius * 2);
        SphereModels[1].transform.localScale = new Vector3(SphereCasterToVisualize.radius * 2, SphereCasterToVisualize.radius * 2, SphereCasterToVisualize.radius * 2);

        CylinderVisualizer = Instantiate(CylinderModel, VisualizatorWrapper.transform);
        CylinderVisualizer.transform.localPosition = Vector3.zero;
        CylinderVisualizer.transform.localRotation = Quaternion.identity * Quaternion.Euler(90f, 0f, 0f);
        CylinderVisualizer.transform.localScale = new Vector3(SphereCasterToVisualize.radius * 2,
            SphereCasterToVisualize.maxDistance / 2f,
            SphereCasterToVisualize.radius * 2);

      
        color = new Vector4(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), trasparency);

        CylinderVisualizer.GetComponent<MeshRenderer>().material.color = color;
        SphereModels[0].GetComponent<MeshRenderer>().material.color = color;
        SphereModels[1].GetComponent<MeshRenderer>().material.color = color;
    }

    private void Update()
    {
        //print("Say hi and welcome to chillies");
        Vector3 normDirection = (SphereCasterToVisualize.director.transform.position - SphereCasterToVisualize.castSource.transform.position).normalized;
        VisualizatorWrapper.transform.position = SphereCasterToVisualize.castSource.transform.position + normDirection * (SphereCasterToVisualize.maxDistance / 2f);
        VisualizatorWrapper.transform.LookAt(SphereCasterToVisualize.castSource.transform.position + normDirection * SphereCasterToVisualize.maxDistance, Vector3.up);
    }

    void HighlightHovered(RaycastHit[] hits)
    {
        //TODO:...
    }

}
