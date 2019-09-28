using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    private AudioSource soundbutton;
    // Use this for initialization
    void Start () {
        AudioSource audiosource = GetComponent<AudioSource>();
        soundbutton = audiosource;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("z"))
        {
            soundbutton.PlayOneShot(soundbutton.clip);
            SceneManager.LoadScene("Title");
        }
    }
}
