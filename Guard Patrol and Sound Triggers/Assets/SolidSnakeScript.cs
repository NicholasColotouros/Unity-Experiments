using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Used to trigger the game over when the guard spots and kills solid snake
public class SolidSnakeScript : MonoBehaviour 
{
	void OnDestroy()
	{
		GameObject textObject = GameObject.Find ("Text");
		textObject.GetComponent<Text> ().text = "GAME OVER";
		textObject.GetComponent<AudioSource> ().Play ();
	}
}
