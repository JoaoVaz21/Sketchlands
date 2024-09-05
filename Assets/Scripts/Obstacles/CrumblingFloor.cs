using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingFloot : MonoBehaviour
{
    [SerializeField] private float timeToCrumble = 2f;
    [SerializeField] private Animator animator;
    private bool _startedCrumbling = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator Crumble()
    {
        yield return new WaitForSeconds(timeToCrumble);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_startedCrumbling)
        {
            _startedCrumbling=true;
            this.animator.SetBool("Shaking", _startedCrumbling);
            StartCoroutine(Crumble());
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _startedCrumbling)
        {
            _startedCrumbling = false;
            this.animator.SetBool("Shaking", _startedCrumbling);
            StopAllCoroutines();
        }
    }
}
