# Sistema de Diálogos - Orgulho & Preconceito

## 📋 Visão Geral

O sistema de diálogos foi criado baseado no roteiro do jogo. Todos os diálogos dos personagens principais (**Darcy** e **Elizabeth**) foram adaptados com profundidade, enquanto os personagens secundários têm falas resumidas e objetivas.

## 📁 Estrutura de Arquivos

```
Assets/
├── Scripts/
│   ├── Data/
│   │   ├── DialogueLine.cs           # Classe base para linhas de diálogo
│   │   ├── ChoiceData.cs             # Classe para opções de diálogo
│   │   ├── DialoguesData.json        # 📄 Todos os diálogos em JSON
│   │   └── DialogueDataLoader.cs     # 🛠️ Gerador de ScriptableObjects
│   ├── Dialogue/
│   │   ├── DialogueManager.cs        # Manager de UI de diálogos
│   │   └── DialogueSequenceManager.cs # Manager de organização de sequências
└── DialogueData/
    └── (Gerado automaticamente)      # ScriptableObjects dos diálogos
```

## 🚀 Como Usar

### 1️⃣ Gerar ScriptableObjects do JSON

1. Abra o **Unity Editor**
2. No menu superior, clique em: **Tools** > **Generate Dialogue ScriptableObjects from JSON**
3. Aguarde a conclusão (pode levar alguns segundos)
4. Os diálogos serão criados em `Assets/DialogueData/`

### 2️⃣ Acessar Diálogos no Código

```csharp
// Carregar sequência de diálogos de um capítulo e cena específicos
DialogueSequenceManager.DialogueSequence sequence = 
    DialogueSequenceManager.GetDialogueSequence(chapterNumber: 1, sceneNumber: 1);

if (sequence != null)
{
    // Iniciar o diálogo
    DialogueManager dialogueManager = GetComponent<DialogueManager>();
    dialogueManager.StartDialogue(sequence.dialogueLines);
}
```

### 3️⃣ Precarregar Diálogos de um Capítulo

```csharp
// Precarrega todos os diálogos de um capítulo na memória
DialogueSequenceManager.PreloadChapter(chapterNumber: 2);
```

## 📖 Conteúdo dos Diálogos

### Capítulo I - O Baile de Meryton ✨ INTERATIVO
- **Cena 1**: Primeiro Encontro (2 escolhas - como Darcy se comporta)

### Capítulo II - Rumores e Verdades ✨ INTERATIVO
- **Cena 1**: O Encontro com Wickham (2 escolhas - como Elizabeth reage)

### Capítulo III - A Verdade Revelada 📖 NARRATIVA
- **Cena 1**: Pemberley e a Carta (apenas narrativa)

### Capítulo IV - O Escândalo 📖 NARRATIVA
- **Cena 1**: A Fuga (apenas narrativa)

### Capítulo VIII - Reconciliação ✨ INTERATIVO
- **Cena 1**: A Segunda Proposta (2 escolhas - como Elizabeth responde)

## ⚙️ Modificações de Stats

Cada escolha afeta os seguintes valores:

- **Pride (Orgulho)**: 0-10 (Darcy)
- **Prejudice (Preconceito)**: 0-10 (Elizabeth)
- **Relationships**: Relacionamentos com NPCs específicos

### Exemplos de Modificações:

```json
{
  "choiceText": "[CANÔNICA] Talvez você tenha razão...",
  "prideChange": -0.3,
  "prejudiceChange": 0,
  "relationshipChanges": [
    {
      "npcName": "Elizabeth",
      "changeAmount": 0.4
    }
  ]
}
```

## 🎨 Personalizações

### Adicionar Sprites de Personagens

Para adicionar portraits dos personagens:

1. Coloque as imagens em `Assets/Resources/Portraits/`
2. Edite os `.asset` gerados e atribua os Sprites no campo `portrait`

### Adicionar Áudio

Para adicionar vozes:

1. Coloque os áudios em `Assets/Resources/Audio/Voices/`
2. Edite os `.asset` gerados e atribua os AudioClips no campo `voice`

## 📝 Estrutura do JSON

O arquivo `DialoguesData.json` segue esta estrutura:

```json
{
  "chapters": [
    {
      "chapterNumber": 1,
      "chapterName": "O Baile de Meryton",
      "scenes": [
        {
          "sceneNumber": 1,
          "sceneName": "Chegada ao Baile",
          "dialogueLines": [
            {
              "speaker": "Darcy",
              "text": "Há muitas pessoas que não conheço.",
              "portrait": null,
              "choices": [...]
            }
          ]
        }
      ]
    }
  ]
}
```

## 🔧 Troubleshooting

### ❌ Erro: "JSON file not found"
- Certifique-se de que `DialoguesData.json` existe em `Assets/Scripts/Data/`

### ❌ Nenhum diálogo foi criado
- Verifique se o Unity console mostra mensagens de erro
- Tente deletar a pasta `Assets/DialogueData/` e regenerar

### ❌ Sprites/Áudio não aparecem
- Edite manualmente cada `.asset` em `DialogueData/` para adicionar as referências

## 💡 Dicas

- Use `DialogueSequenceManager.ClearCache()` entre cenas para liberar memória
- Precarregue capítulos inteiros antes de começar para melhor performance
- Os diálogos de personagens secundários são resumidos propositalmente
- Darcy e Elizabeth têm diálogos mais complexos e com mais escolhas

## 📄 Ficheiro JSON

O arquivo original `DialoguesData.json` contém toda a estrutura narrativa e pode ser editado diretamente se necessário regenerar os ScriptableObjects.

---

**Criado com base no roteiro: "Orgulho e Preconceito - Roteiro Canônico"**
