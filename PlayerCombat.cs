using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{

	public Animator animator;

	public Transform attackPoint;
	public LayerMask enemyLayers;

	public float attackRange = 0.5f;
	public int attackDamage = 1;

	public float attackRate = 2f;
	float nextAttackTime = 0f;

	// Update is called once per frame
	void Update()
    {
	if(Time.time >= nextAttackTime)
	{
		if (Input. GetMouseButtonDown(0))
		{
			Attack();
			nextAttackTime = Time.time + 1f / attackRate;
		}
	}
    }

	void Attack()
	{
		animator.SetTrigger("Attack");
		
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
		
		foreach(Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
		}
	}

void OnDrawGizmosSelected()
	{
		if(attackPoint == null)
			return;

		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}

	private void OnCollisionEnter2D(Collision2D other)
    {
		if (other.gameObject.tag == "Enemy")
        {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
