using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] private string text;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DialogManager.Instance.ActivateDialogBox(text);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DialogManager.Instance.DeactivateDialogBox();
        }
    }
}