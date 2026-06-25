using UnityEngine;
using UnityEngine.SceneManagement; // Biblioteca necessária para mudar de cena

public class MenuPrincipal : MonoBehaviour
{
    [Header("Configurações das Telas")]
    public GameObject telaInicio; // Arraste a pasta Tela_Inicio para cá no Inspector
    public GameObject telaMenu;   // Arraste a pasta Tela_Menu (ou Painel_Menu) para cá

    // 1. Método para o primeiro botão ("Clique aqui para começar")
    public void AbrirMenuDeOpcoes()
    {
        if (telaInicio != null && telaMenu != null)
        {
            telaInicio.SetActive(false); // Esconde o título e o botão inicial
            telaMenu.SetActive(true);    // Mostra o véu de fundo e os novos botões
        }
    }

    // 2. Método que será chamado quando o jogador clicar em "NOVO JOGO"
    public void Jogar()
    {
    // Deleta o arquivo físico para garantir que não haja trapaça de carregamento
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "savegame.json");
        if (System.IO.File.Exists(savePath))
        {
            System.IO.File.Delete(savePath);
        }

        if (GameManager.Instance != null)
        {
            // Força os valores limpos no Singleton que está na memória
            GameManager.Instance.pride = 50f;
            GameManager.Instance.prejudice = 50f;
            GameManager.Instance.savedDialogueIndex = 0;
            GameManager.Instance.relationships.Clear();

            // precisamos avisar para ela que os pontos voltaram para 50!
            GameManager.Instance.OnStatsChanged?.Invoke(50f, 50f);
        }

        // Carrega a cena do jogo
        SceneManager.LoadScene("SampleScene"); 
    }

    // 3. Método para fechar o jogo (pode usar no botão Sair, se criar um)
    /*public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("O jogo foi fechado!");
    } */
}