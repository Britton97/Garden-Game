using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextBoxTyper : MonoBehaviour
{
    [SerializeField] private DataStringList_SO dialogue;
    [SerializeField] private float typingSpeed = 0.05f; // Speed at which text is typed out
    [SerializeField] private TextMeshProUGUI dialogueText; // Reference to the UI Text component

    private Coroutine typingCoroutine;
    private bool isTyping;
    private int currentDialogueIndex = 0;
    [SerializeField] private UnityEvent onDialogueEnd;

    public void NextTextState()
    {
        if (isTyping)
        {
            // If already typing, display all text immediately
            StopCoroutine(typingCoroutine);
            dialogueText.maxVisibleCharacters = dialogue.data[currentDialogueIndex].Length;
            isTyping = false;
        }
        else
        {
            // If the message is fully displayed, go to the next message
            if (dialogueText.maxVisibleCharacters == dialogue.data[currentDialogueIndex].Length)
            {
                currentDialogueIndex++;
                if (currentDialogueIndex >= dialogue.data.Count)
                {
                    currentDialogueIndex = dialogue.data.Count - 1; // Stop at the last message
                    onDialogueEnd.Invoke();
                    return; // Exit the method to prevent starting the coroutine again
                }
            }
            // Start typing the next message
            typingCoroutine = StartCoroutine(TypeText());
        }
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        dialogueText.text = dialogue.data[currentDialogueIndex];
        dialogueText.maxVisibleCharacters = 0;

        int visibleCharacters = 0;
        for (int i = 0; i < dialogue.data[currentDialogueIndex].Length; i++)
        {
            if (dialogue.data[currentDialogueIndex][i] == '<')
            {
                // Skip over the tag
                while (i < dialogue.data[currentDialogueIndex].Length && dialogue.data[currentDialogueIndex][i] != '>')
                {
                    i++;
                }
                // Include the closing '>'
                if (i < dialogue.data[currentDialogueIndex].Length)
                {
                    i++;
                }
            }
            else
            {
                visibleCharacters++;
                dialogueText.maxVisibleCharacters = visibleCharacters;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        dialogueText.maxVisibleCharacters = dialogue.data[currentDialogueIndex].Length; // Ensure the final text is displayed
        isTyping = false;
    }
}