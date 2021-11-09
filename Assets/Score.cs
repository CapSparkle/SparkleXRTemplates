using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
	public Action<int> onScoreChange;
    public int score { get; private set; }

	public void OnTriggerEnter(Collider other)
	{
        GoldNugget goldNugget;
	    if((goldNugget = other.GetComponent<GoldNugget>()) != null)
		{
            score += goldNugget.score;
			onScoreChange(score);
			Destroy(goldNugget);
        }
	}
}
