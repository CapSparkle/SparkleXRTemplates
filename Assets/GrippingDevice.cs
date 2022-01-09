
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrippingDevice : MonoBehaviour
{
	[SerializeField]
	int HP = 1;

	public Action OnDamageTaken;

    public void TakeDamage(int damage)
	{
		HP -= damage;
		OnDamageTaken();

		if (HP <= 0)
			ProccedGameOver();
	}

	void ProccedGameOver()
	{
		if(inGameFlag)
		{
			inGameFlag = false;
			print("GameOver");
			Invoke("Restart", 5f);
		}
	}

	bool inGameFlag;

	void Restart()
	{
		SceneManager.LoadScene("AstraMiner", LoadSceneMode.Single);
	}

	private void OnEnable()
	{
		inGameFlag = true;
	}
}
