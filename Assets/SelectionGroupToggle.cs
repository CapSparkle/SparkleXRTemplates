using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SparkleXRTemplates;

public class SelectionGroupToggle : MonoBehaviour
{
    [SerializeField]
    SelectorsManager selectorsManager;

    List<List<int>> minSelectRequirementsOnStart, onlyFirstSelectReq;

    // Start is called before the first frame update
    void Start()
    {
        minSelectRequirementsOnStart = selectorsManager.minSelectRequirements;
        onlyFirstSelectReq = new List<List<int>>() { minSelectRequirementsOnStart[0] };
    }

    public void ToggleOnAdvancedSelecting()
	{
        selectorsManager.minSelectRequirements = minSelectRequirementsOnStart;
	}

    public void ToggleOffAdvancedSelecting()
	{
        selectorsManager.minSelectRequirements = onlyFirstSelectReq;

    }
}
