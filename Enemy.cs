using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Animator animator;

	public int maxHealth = 2;
	int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;

		animator.SetTrigger("Hurt");
		
		if(currentHealth <= 0)
		{
			ScoreManager.instance.AddPoint();
			DestroyObjectDelayed();
		}
	}

	void DestroyObjectDelayed()
	{
		animator.SetBool("IsDead", true);
		GetComponent<Collider2D>().enabled = false;
		Destroy(gameObject, 5);
		this.enabled = false;
	}
}