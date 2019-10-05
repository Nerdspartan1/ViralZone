﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
	public float PerlinRate;
	public float PerlinAccelerationFactor;
	private Rigidbody2D _rigidbody;
	private Vector2 _perlinDirection;
	private Animator _animator;

	public int VirionsDrop = 5;
	public GameObject VirionPrefab;
	private VirionManager _virionManager;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_perlinDirection = Random.insideUnitCircle.normalized;
		_animator = GetComponent<Animator>();
		_virionManager = GameManager.instance.GetComponent<VirionManager>();
	}

	void Update()
    {
		Vector2 perlinPosition = _perlinDirection * PerlinRate * Time.time;
		Vector2 acceleration = PerlinAccelerationFactor  * new Vector2(Mathf.PerlinNoise(perlinPosition.x, perlinPosition.y) - 0.5f, Mathf.PerlinNoise(perlinPosition.x + 10,perlinPosition.y) -0.5f );
		_rigidbody.velocity += acceleration * Time.deltaTime;
		

	}

	public void StartInfecting()
	{
		_animator.SetBool("panicked", true);
	}

	public void StopInfecting()
	{
		_animator.SetBool("panicked", false);
	}

	public void Pop(bool infected)
	{

		_animator.SetTrigger("pop");
		Destroy(gameObject,1.0f);
		if (infected)
		{
			EnemiesManager.numberOfCellsDestroyed++;
			for (int i = 0; i < VirionsDrop; i++)
			{
				var v = Instantiate(VirionPrefab, transform.position + Random.onUnitSphere, Quaternion.identity, GameManager.instance.Game.transform);
				_virionManager.virions.Add(v.GetComponent<Virion>());
			}
		}
		EnemiesManager.numberOfCells--;
	}
}
