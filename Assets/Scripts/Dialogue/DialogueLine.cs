using UnityEngine;

/// <summary>
/// ScriptableObject representing a line of dialogue.
/// </summary>
[CreateAssetMenu(fileName = "NewDialogueLine", menuName = "Dialogue/DialogueLine")]
public class DialogueLine : ScriptableObject
{
    public int dialogueID;
    public string speaker;
    [TextArea(3, 10)]
    public string text;
    public Sprite portrait;
    public AudioClip voiceLine;
    public ChoiceData[] choices;
}