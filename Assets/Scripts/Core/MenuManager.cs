using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Importante para gerenciar botões

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public Button continuarBotao; // Arraste o botão "Continuar" aqui no Inspector

    private void Start()
    {
        // Se o botão de continuar foi associado no Inspector
        if (continuarBotao != null)
        {
            // O botão CONTINUAR só fica clicável se o arquivo de save já existir no PC do jogador!
            continuarBotao.interactable = System.IO.File.Exists(System.IO.Path.Combine(Application.persistentDataPath, "savegame.json"));
        }
    }

    public void NovoJogo()
    {
        // Reinicia os valores padrões do GameManager se necessário antes de carregar o jogo
        GameManager.Instance.pride = 0;
        GameManager.Instance.prejudice = 0;
        GameManager.Instance.relationships.Clear();

        // Substitua pelo nome exato da sua cena onde a história começa de verdade
        SceneManager.LoadScene("SampleScene"); 
    }

    public void ContinuarJogo()
    {
        // Tenta carregar os dados salvos no JSON
        bool sucesso = SaveSystem.LoadGame();

        if (sucesso)
        {
            Debug.Log("Save carregado com sucesso! Iniciando a cena...");
            // Carrega a cena do jogo com os pontos antigos já restaurados no GameManager
            SceneManager.LoadScene("SampleScene"); 
        }
        else
        {
            Debug.LogError("Erro crítico: Arquivo de save não encontrado.");
        }
    }

    // Função que o botão de Salvar vai chamar na SampleScene
    public void BotaoSalvarJogo(int currentLine)
    {
        // Chama a linha mágica que converte os pontos atuais do GameManager em JSON
        SaveSystem.SaveGame(currentLine); // Você pode passar o índice atual do diálogo se quiser salvar isso também
        
        Debug.Log("Jogo salvo manualmente pelo jogador!");
        
        // (Opcional) Se você tiver algum textinho de "Jogo Salvo!" piscando na tela, 
       
    }

    public void IrParaCreditos() => SceneManager.LoadScene("Final");
    public void VoltarParaMenu() => SceneManager.LoadScene("MainMenu");
} 