using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroFIeldStarter : MonoBehaviour
{
    public AsteroidFieldCreator asteroidFieldCreator;
    void Start()
    {
        asteroidFieldCreator.StartGenerator();
    }
}
