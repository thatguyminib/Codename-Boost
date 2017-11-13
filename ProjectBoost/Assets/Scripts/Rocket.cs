using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 500f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transending};
    State state = State.Alive;

	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	

	void Update () {
        //todo somewhere stop sound on death
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                state = State.Transending;
                Invoke("LoadNextScene", 1f); //todo parameterise time
                break;
            default:
                state = State.Dying;
                Invoke("ReloadLevelOnDeath", 1f); //todo parameterise time
                break;
        }
    }

    private void ReloadLevelOnDeath()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single); // todo reload current scene
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single); // todo allow more than two levels
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {

        rigidBody.freezeRotation = true; // take manual control of rotation

        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {          
            transform.Rotate(Vector3.right * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.right * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation
    }
}
