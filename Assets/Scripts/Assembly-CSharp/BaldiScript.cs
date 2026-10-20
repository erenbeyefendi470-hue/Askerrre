using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaldiScript : MonoBehaviour
{
	public bool db;

	public float baseTime;

	public float speed;

	public float timeToMove;

	public float baldiAnger;

	public float baldiTempAnger;

	public float baldiWait;

	public float baldiSpeedScale;

	private float moveFrames;

	private float currentPriority;

	public bool antiHearing;

	public float antiHearingTime;

	public float vibrationDistance;

	public float angerRate;

	public float angerRateRate;

	public float angerFrequency;

	public float timeToAnger;

	public bool endless;

	public Transform player;

	public Transform wanderTarget;

	public AILocationSelectorScript wanderer;

	private AudioSource baldiAudio;

	public AudioClip slap;

	public Animator baldiAnimator;

	public float coolDown;

	private Vector3 previous;

	private bool rumble;

	private NavMeshAgent agent;

	public bool canChase;

	public AudioSource music;

	public float waitTime;

	public Image flash;

	public Animator cameraAnimator;

	public Animator hudAnimator;

	public AudioClip chase;

	public AudioSource localSource;

	private void Start()
	{
		cameraAnimator.enabled = false;
		hudAnimator.enabled = false;
		StartCoroutine(wait());
		baldiAudio = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
		baseTime = 1f;
		baldiWait = 0.055f;
		baldiSpeedScale = 0.2f;
		vibrationDistance = 50f;
		timeToMove = baseTime;
		Wander();
		if (PlayerPrefs.GetInt("Rumble") == 1)
		{
			rumble = true;
		}
	}

	private IEnumerator wait()
	{
		yield return new WaitForSeconds(waitTime);
		canChase = true;
		music.Play();
		PlayerScript component = player.gameObject.GetComponent<PlayerScript>();
		component.walkSpeed = 55f;
		component.runSpeed = 75f;
		cameraAnimator.enabled = true;
		cameraAnimator.Play("CameraWobble", 0, 0f);
		hudAnimator.enabled = true;
		hudAnimator.Play("HUDPulse", 0, 0f);
		localSource.PlayOneShot(chase);
		component.gc.camera.fieldOfView = 120f;
		RenderSettings.fog = false;
		float alpha = 1f;
		for (int i = 0; i < 20; i++)
		{
			flash.color = new Color(1f, 1f, 1f, alpha);
			alpha -= 0.05f;
			yield return new WaitForSeconds(0.05f);
		}
	}

	private void Update()
	{
		if (timeToMove > 0f)
		{
			timeToMove -= 1f * Time.deltaTime;
		}
		else
		{
			Move();
		}
		if (coolDown > 0f)
		{
			coolDown -= 1f * Time.deltaTime;
		}
		if (antiHearingTime > 0f)
		{
			antiHearingTime -= Time.deltaTime;
		}
		else
		{
			antiHearing = false;
		}
	}

	private void FixedUpdate()
	{
		if (moveFrames > 0f)
		{
			moveFrames--;
			agent.speed = speed;
		}
		else
		{
			agent.speed = 0f;
		}
		Vector3 direction = player.position - base.transform.position;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out var hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & (hitInfo.transform.tag == "Player"))
		{
			db = true;
			TargetPlayer();
		}
		else
		{
			db = false;
		}
	}

	private void Move()
	{
		if (canChase)
		{
			if (base.transform.position == previous && coolDown < 0f)
			{
				Wander();
			}
			moveFrames = 10f;
			timeToMove = baldiWait;
			previous = base.transform.position;
			baldiAudio.PlayOneShot(slap);
		}
	}

	private void Wander()
	{
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
		currentPriority = 0f;
	}

	public void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
		currentPriority = 0f;
	}

	public void GetAngry(float value)
	{
	}

	public void GetTempAngry(float value)
	{
	}

	public void Hear(Vector3 soundLocation, float priority)
	{
		if (!antiHearing && priority >= currentPriority)
		{
			agent.SetDestination(soundLocation);
			currentPriority = priority;
		}
	}

	public void ActivateAntiHearing(float t)
	{
		Wander();
		antiHearing = true;
		antiHearingTime = t;
	}
}
