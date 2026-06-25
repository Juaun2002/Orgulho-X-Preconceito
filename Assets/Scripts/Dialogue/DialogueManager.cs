using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages dialogue display and user interaction.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;
    public GameObject choicePanel;
    public GameObject choiceButtonPrefab;
    public Transform choiceButtonContainer;
    // Uma lista que vai guardar todas as falas do capítulo atual nesta cena
    public System.Collections.Generic.List<DialogueLine> chapterLines;
    public string nextSceneToLoad;

    private DialogueLine[] dialogueLines;
    private int currentLineIndex = 0;
    private Coroutine typewriterCoroutine;
    private bool isDialogueActive = false; // Controla se existe uma conversa rodando

    private void Start()
    {
        // Removemos todo o bloco que mexia nos pontos (pride/prejudice)!
        // Agora o DialogueManager foca APENAS em iniciar as falas da cena.

        if (chapterLines != null && chapterLines.Count > 0)
        {
            StartDialogue(chapterLines.ToArray());
        }
        else
        {
            Debug.LogError("🚨 ERRO: A lista 'Chapter Lines' está vazia nesta cena!");
        }
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        dialogueLines = lines;
        
        // 🛡️ TRAVA DE SEGURANÇA para o índice da fala
        if (GameManager.Instance != null)
        {
            currentLineIndex = GameManager.Instance.savedDialogueIndex; 
        }
        else
        {
            currentLineIndex = 0; // Se não tem GameManager, começa da fala 0
        }
        
        isDialogueActive = true; 
        DisplayLine();
    }

    private void Update()
    {
        // Se o diálogo estiver ativo e o jogador clicou com o botão esquerdo do mouse (Novo Sistema)
        if (isDialogueActive && UnityEngine.InputSystem.Pointer.current != null && 
            UnityEngine.InputSystem.Pointer.current.press.wasPressedThisFrame)
        {
            // Se as escolhas estiverem abertas na tela, bloqueia o clique para não pular o texto
            if (choicePanel.activeSelf) return;

            AdvanceOrSkipText();
        }
    }


    private void DisplayLine()
    {

        // Cancelar qualquer agendamento de avanço automático residual por segurança
        CancelInvoke(nameof(AdvanceDialogue));

        if (currentLineIndex >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueLines[currentLineIndex];
        speakerText.text = line.speaker;
        portraitImage.sprite = line.portrait;
        // Optionally play voice line here if you have an AudioSource

        // Stop any ongoing typewriter effect
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        // Start typewriter effect for the dialogue text
        typewriterCoroutine = StartCoroutine(TypewriterEffect(line.text));

        // Handle choices
        if (line.choices != null && line.choices.Length > 0)
        {
            ShowChoices(line.choices);
        }
        else
        {
            HideChoices();

        }
    }

    private IEnumerator TypewriterEffect(string text)
    {
        dialogueText.text = "";
        foreach (char c in text.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.04f); // Adjust speed as needed
        }
        typewriterCoroutine = null;
    }

    private void ShowChoices(ChoiceData[] choices)
    {
        // 🚨 ALARMES PARA DESCOBRIR O QUE ESTÁ VAZIO NA CENA ATUAL:
        if (choicePanel == null) { Debug.LogError("🚨 ALARME: FALTOU colocar o 'Choice Panel' no Inspector do DialogueManager nesta cena!"); return; }
        if (choiceButtonContainer == null) { Debug.LogError("🚨 ALARME: FALTOU colocar o 'Choice Button Container' no Inspector do DialogueManager nesta cena!"); return; }
        if (choiceButtonPrefab == null) { Debug.LogError("🚨 ALARME: FALTOU colocar o 'Choice Button Prefab' no Inspector do DialogueManager nesta cena!"); return; }

        choicePanel.SetActive(true);
        
        // Clear existing choice buttons
        foreach (Transform child in choiceButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Instantiate buttons for each choice
        foreach (ChoiceData choice in choices)
        {
            // Instancia o modelo de botão (Prefab) dentro do container
            GameObject btnObj = Instantiate(choiceButtonPrefab, choiceButtonContainer);
        
            // Pega o componente de texto do botão e coloca o texto correto do JSON
            TextMeshProUGUI buttonText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                // Usando 'choiceText' que bate com o seu JSON!
                buttonText.text = choice.choiceText; 
            }
            else 
            {
                Debug.LogError("🚨 ALARME: O seu Choice Button Prefab não tem um componente de texto 'TextMeshProUGUI'!");
            }

            // ADICIONADO: Configura o botão para acionar a função OnChoiceSelected ao ser clicado
            Button btnComponent = btnObj.GetComponent<Button>();
            if (btnComponent != null)
            {
                btnComponent.onClick.AddListener(() => OnChoiceSelected(choice));
            }
            else 
            {
                Debug.LogError("🚨 ALARME: O seu Choice Button Prefab não tem um componente 'Button'!");
            }
        }
    }
    /// <summary>
    /// Called by PlayerController when player clicks/presses space.
    /// Skips typewriter or advances to next line if text is already displayed.
    /// </summary>
    public void AdvanceOrSkipText()
    {
        if (typewriterCoroutine != null)
        {
            // Skip typewriter effect - show full text immediately
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
            DialogueLine line = dialogueLines[currentLineIndex];
            dialogueText.text = line.text;
        }
        else if (choicePanel.activeSelf == false)
        {
            // Text already displayed and no choices - advance to next line
            AdvanceDialogue();
        }
    }

    private void HideChoices()
    {
        choicePanel.SetActive(false);
    }

    private void OnChoiceSelected(ChoiceData choice)
        {
            // 🛡️ Alarme de segurança 1
            if (choice == null)
            {
                Debug.LogError("🚨 ERRO: Os dados da escolha (ChoiceData) vieram NULOS do botão!");
                return;
            }

        // 🛡️ Alarme de segurança 2
            if (GameManager.Instance == null)
            {
                Debug.LogError("🚨 ERRO: O GameManager.Instance está NULO! O GameManager imortal sumiu ou não foi encontrado!");
                return;
            }

            Debug.Log($"Escolha selecionada: {choice.choiceText}. Aplicando efeitos no GameManager...");
            
            HideChoices();
    
            // Apply the choice effects to GameManager
            GameManager.Instance.ApplyChoice(choice);

            // Advance to next line
            AdvanceDialogue();
        }
    private void AdvanceDialogue()
    {
        currentLineIndex++;
        DisplayLine();
    }

  private void EndDialogue()
    {
        isDialogueActive = false;
        speakerText.text = "";
        dialogueText.text = "";
        portraitImage.sprite = null;
        HideChoices();

        if (!string.IsNullOrEmpty(nextSceneToLoad))
        {
            // 🔥 ADICIONADO AQUI: Zeramos o índice ANTES de mudar de cena.
            // Assim, a próxima cena sabe que deve começar da primeira fala (0) 
            // sem precisar mexer nos pontos de orgulho/preconceito!
            if (GameManager.Instance != null)
            {
                GameManager.Instance.savedDialogueIndex = 0;
            }

            SceneManager.LoadScene(nextSceneToLoad);
        }
    }
}