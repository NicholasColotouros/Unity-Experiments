using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour 
{
	public Transform Player;
	public float FOV;
	public float PatrolSpeed;
	public Transform CurrentWayPoint;

	private NavMeshAgent Agent;
	private AudioSource Detected;
	private Vector3 LastWayPoint;

	private bool ClipPlayed;

	// Use this for initialization
	void Start () 
	{
		Agent = gameObject.GetComponent<NavMeshAgent> ();
		Detected = gameObject.GetComponent<AudioSource> ();
		ClipPlayed = false;

		Vector3 LastWayPoint = CurrentWayPoint.transform.position;
		Transform[] destinations = CurrentWayPoint.GetComponent<WayPointNode> ().AdjacentWayPoints;
		CurrentWayPoint = destinations [Random.Range (0, destinations.Length)];
		Agent.destination = CurrentWayPoint.position;

		// Move the FOV markers to correspond to the guard's FOV
		float rotationAngle = FOV / 2;
		gameObject.transform.FindChild("LeftPivot").Rotate( new Vector3(0, rotationAngle, 0));
		gameObject.transform.FindChild("RightPivot").Rotate( new Vector3(0, -1 * rotationAngle, 0));
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
			Vector3 LastWayPoint = CurrentWayPoint.transform.position;

			// Pick the next waypoint randomly. If the previous waypoint is selected, pick the next index
			Transform[] destinations = CurrentWayPoint.GetComponent<WayPointNode> ().AdjacentWayPoints;

			int index = Random.Range (0, destinations.Length);
			CurrentWayPoint = destinations [index];

			if(CurrentWayPoint.position == LastWayPoint)
			{
				CurrentWayPoint = destinations [ (index + 1) % destinations.Length];
			}

			Agent.destination = CurrentWayPoint.position;
		}
	}
}
