using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement; // 🔥 ADICIONADO para podermos ler o nome da cena atual!
using System.IO;

/// <summary>
/// Singleton GameManager to hold game state: pride, prejudice, and relationships.
/// Provides events and persistence hooks for UI and other systems.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Range(0f, 100f)]
    public float pride = 50f;
    [Range(0f, 100f)]
    public float prejudice = 50f;

    public int savedDialogueIndex = 0;

    // Relationships: NPC name -> relationship value (0-100)
    public Dictionary<string, float> relationships = new Dictionary<string, float>();

    // Events for UI / other systems to subscribe
    public Action<float, float> OnStatsChanged;
    public Action<string, float> OnRelationshipChanged;

    public void ResetarParaNovoJogo()
    {
        pride = 50f;
        prejudice = 50f;
        savedDialogueIndex = 0;
        
        if (relationships != null)
        {
            relationships.Clear();
        }

        // Se você ainda usar aquele evento para atualizar as barrinhas da UI:
        OnStatsChanged?.Invoke(50f, 50f);
        
        Debug.Log("🔄 GameManager: Status resetados para um Novo Jogo!");
    }

    private void Awake()
    {
        // Lógica do Singleton para torná-lo imortal
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 🔥 O SEGREDO ESTÁ AQUI:
            // Assim que o GameManager nasce, ele checa o arquivo.
            VerificarOuCarregarSave();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void VerificarOuCarregarSave()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "savegame.json");

        if (File.Exists(savePath))
        {
            // 💾 CORRIGIDO: Se o arquivo existe, nós resgatamos as informações salvas!
            try
            {
                string jsonText = File.ReadAllText(savePath);
                DadosDoSave dados = JsonUtility.FromJson<DadosDoSave>(jsonText);

                this.pride = dados.pride;
                this.prejudice = dados.prejudice;
                this.savedDialogueIndex = dados.savedDialogueIndex;
                
                // Atualiza a UI com os dados carregados do save antigo
                OnStatsChanged?.Invoke(pride, prejudice);

                Debug.Log($"💾 GameManager: Progresso carregado com sucesso! Orgulho: {pride}, Preconceito: {prejudice}");
            }
            catch (Exception ex)
            {
                Debug.LogError("🚨 Erro ao tentar ler o arquivo de save antigo: " + ex.Message);
            }
        }
        else
        {
            // 🔥 SE O ARQUIVO NÃO EXISTE (Novo Jogo):
            // Forçamos os valores iniciais perfeitos aqui na raiz!
            pride = 50f;
            prejudice = 50f;
            savedDialogueIndex = 0;
            if (relationships != null) relationships.Clear();

            Debug.Log("🔄 GameManager: Nenhum arquivo encontrado (Novo Jogo). Status definidos para 50/50!");
        }
    }

    /// <summary>
    /// Apply the effects of a choice made in dialogue.
    /// ChoiceData.prideChange and prejudiceChange are expected in range [-1,1].
    /// This method converts them to 0-100 scale and clamps values.
    /// </summary>
    /// <param name="choice">The choice data containing attribute and relationship changes.</param>
    public void ApplyChoice(ChoiceData choice)
    {
        if (choice == null) return;

        // Convert [-1,1] deltas to percentage scale (-100..100)
        pride = Mathf.Clamp(pride + choice.prideChange * 100f, 0f, 100f);
        prejudice = Mathf.Clamp(prejudice + choice.prejudiceChange * 100f, 0f, 100f);

        // Apply relationship changes; default relationship is 50 if not present
        foreach (var relChange in choice.relationshipChanges)
        {
            if (relChange == null || string.IsNullOrEmpty(relChange.npcName))
                continue;

            float old = relationships.ContainsKey(relChange.npcName) ? relationships[relChange.npcName] : 50f;
            float updated = Mathf.Clamp(old + relChange.changeAmount * 100f, 0f, 100f);
            relationships[relChange.npcName] = updated;

            // Notify listeners about relationship change
            OnRelationshipChanged?.Invoke(relChange.npcName, updated);
        }

        // Notify listeners about stats change
        OnStatsChanged?.Invoke(pride, prejudice);

        // 🔥 MODIFICADO: Agora salvamos os dados injetando o nome da cena atual!
        try
        {
            SalvarProgressoAtual();
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Erro ao tentar salvar o jogo: " + ex.Message);
        }
    }

    // 🔥 NOVA FUNÇÃO AUXILIAR: Junta tudo o que precisamos salvar e escreve no JSON
    private void SalvarProgressoAtual()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "savegame.json");

        DadosDoSave novosDados = new DadosDoSave();
        novosDados.pride = this.pride;
        novosDados.prejudice = this.prejudice;
        novosDados.savedDialogueIndex = this.savedDialogueIndex;
        
        // 🎬 Captura o nome exato da cena em que o jogador está pisando agora!
        novosDados.cenaSalva = SceneManager.GetActiveScene().name;

        string json = JsonUtility.ToJson(novosDados, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"📝 Jogo Salvo automaticamente na cena: {novosDados.cenaSalva}");
    }

    [System.Serializable]
    public class DadosDoSave
    {
        public float pride;
        public float prejudice;
        public int savedDialogueIndex;
        public string cenaSalva; 
    }
}