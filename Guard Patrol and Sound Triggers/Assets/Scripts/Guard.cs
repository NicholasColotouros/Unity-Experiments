using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour 
{
	public Transform Player;
	public float FOV;
	public float PatrolSpeed;
	public Transform[] PatrolWayPoints;

	private NavMeshAgent Agent;
	private AudioSource Detected;
	private int WayPointIndex;

	private bool ClipPlayed;

	// Use this for initialization
	void Start () 
	{
		Agent = gameObject.GetComponent<NavMeshAgent> ();
		Detected = gameObject.GetComponent<AudioSource> ();
		ClipPlayed = false;

		WayPointIndex = 0;
		Agent.destination = PatrolWayPoints [WayPointIndex].position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!ClipPlayed) checkPlayerVisible ();
		patrol ();
	}

	private void checkPlayerVisible ()
	{
		Vector3 playerPos = Player.position;
		Vector3 guardPos = gameObject.transform.position;

		Vector3 playerDirection = playerPos - guardPos;
		Vector3 forward = gameObject.transform.forward;

		float angle = Vector3.Angle (forward, playerDirection);

		// FOV / 2 because that many degrees to the left and to the right
		if(angle <= FOV / 2)
		{
			RaycastHit hit;
			if(Physics.Raycast(guardPos, playerDirection, out hit))
			{
				if(hit.transform.tag == "Player")
				{
					if(! ClipPlayed)
					{
						Detected.Play();
						ClipPlayed = true;

						// pew pew
						Destroy(hit.transform.gameObject);
					}
				}
			}
		}
	}

	private void patrol()
	{
		Agent.speed = PatrolSpeed;
		if( Agent.destination.x == gameObject.transform.position.x && Agent.destination.z == gameObject.transform.position.z)
		{
			WayPointIndex++;
			if( WayPointIndex == PatrolWayPoints.Length ) WayPointIndex = 0;
		
			Agent.destination = PatrolWayPoints[WayPointIndex].position;
		}
	}
}
