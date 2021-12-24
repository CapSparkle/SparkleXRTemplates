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
        minSelectRequirementsOnStart = selectorsManager.minSelectRequirments;
        onlyFirstSelectReq = new List<List<int>>() { minSelectRequirementsOnStart[0] };
    }

    public void ToggleOnAdvancedSelecting()
	{
        selectorsManager.minSelectRequirments = minSelectRequirementsOnStart;
	}

    public void ToggleOffAdvancedSelecting()
	{
        selectorsManager.minSelectRequirments = onlyFirstSelectReq;

    }
}
