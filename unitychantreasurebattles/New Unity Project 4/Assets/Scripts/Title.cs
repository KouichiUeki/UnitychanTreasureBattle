using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Title : MonoBehaviour {

    private AudioSource soundbutton;
    
    public void Start()
    {
        AudioSource audiosource = GetComponent<AudioSource>();
        soundbutton = audiosource;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown("z"))
        {
            soundbutton.PlayOneShot(soundbutton.clip);
            SceneManager.LoadScene("Main");
        }
    }
}

