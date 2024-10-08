using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBell : MonoBehaviour
{
    [SerializeField] Animator bellAnimator;
    [SerializeField] AudioSource bellAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CharacterMovementController>().SetCantMove(true);
            DialogManager.Instance.ActivateDialogBox("FInally, the magic bell! Time to ring it and end this nonsense!");
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2f);
        bellAnimator.SetTrigger("Ring");
        bellAudioSource.Play();
        yield return new WaitForSeconds(1.5f);
        //The game doesn't have an end for now. Just send to a scene explaining what this is and thanking for playing it.
        SceneManager.LoadScene(4);

    }
}
