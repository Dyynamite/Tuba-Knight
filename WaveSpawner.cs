using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
	public Animator transition;

	public float transitionTime = 1f;

	public void LoadNextLevel()
	{
		if (SceneManager.GetActiveScene().buildIndex == 6)
		{
			StartCoroutine(LoadLevel(0));
		}
		else
		{
			StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
		}
	}

	IEnumerator LoadLevel(int levelIndex)
	{
		transition.SetTrigger("Start");

		yield return new WaitForSeconds(transitionTime);

		SceneManager.LoadScene(levelIndex);
	}





	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform enemy;
		public int count;
		public float rate;
	}
	public Wave[] waves;
	private int nextWave = 0;

	public Transform[] spawnPoints;

	public float timeBetweenWaves = 3f;
	private float waveCountdown;

	private float searchCountdown = 1f;

	private SpawnState state = SpawnState.COUNTING;

	void Start ()
	{
		waveCountdown = timeBetweenWaves;
	}
	void Update()
	{
		if (state == SpawnState.WAITING)
		{
			if (!EnemyIsAlive())
			{
				WaveCompleted();
			}
			else
			{
				return;
			}
		}
		if (waveCountdown <= 0)
		{
			if (state != SpawnState.SPAWNING)
			{
				StartCoroutine( SpawnWave ( waves[nextWave] ) );
			}
		}
		else
		{
			waveCountdown -= Time.deltaTime;
		}
	}

	void WaveCompleted()
    {
		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

		if (nextWave + 1 > waves.Length - 1)
        {
			LoadNextLevel(); // Transition to next level // // Transition to next level // // Transition to next level // // Transition to next level //
		} else
        {
			nextWave++;
		}
	}

	bool EnemyIsAlive()
	{
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f)
		{
			searchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag ("Enemy") == null)
			{
				return false;
			}
		}
		return true;
	}
	IEnumerator SpawnWave (Wave _wave)
	{
		Debug.Log ("Spawning Wave: " + _wave.name);
		state = SpawnState.SPAWNING;
		for (int i = 0; i < _wave.count; i++)
		{
			SpawnEnemy (_wave.enemy);
			yield return new WaitForSeconds( 1f/_wave.rate );
		}
		state = SpawnState.WAITING;
		yield break;
	}
	void SpawnEnemy (Transform _enemy)
	{
		Transform _sp = spawnPoints[ Random.Range (0, spawnPoints.Length) ];
		Instantiate (_enemy, _sp.position, _sp.rotation);
	}
}
