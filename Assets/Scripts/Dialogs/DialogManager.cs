using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; set; }
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void ActivateDialogBox(string dialogText)
    {
        text.text = dialogText;
        dialogBox.SetActive(true);
    }

    public void DeactivateDialogBox()
    {
        dialogBox.SetActive(false);
        text.text = "";
    }
}