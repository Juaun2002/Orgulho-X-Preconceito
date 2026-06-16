# ⚡ Quick Start - Diálogos do Jogo

## O que foi feito?

✅ **Roteiro Completo em JSON**
- Todos os diálogos do seu roteiro PDF foram estruturados
- Darcy e Elizabeth com diálogos profundos
- Personagens secundários com falas resumidas
- 8 capítulos, múltiplas cenas

✅ **Sistema Automático de ScriptableObjects**
- Script que gera os diálogos automaticamente
- Menu integrado no Unity

✅ **Manager de Diálogos**
- Sistema fácil para carregar diálogos
- Suporte para precarregamento
- Organizado por capítulo/cena

✅ **Exemplos de Uso**
- Código pronto para integrar nas suas cenas

---

## ⚡ Próximos Passos (5 minutos)

### 1️⃣ Abra seu projeto no Unity

### 2️⃣ Gere os ScriptableObjects
```
Menu: Tools > Generate Dialogue ScriptableObjects from JSON
```
(Aguarde 10-30 segundos)

### 3️⃣ Verifique em Assets/DialogueData/
Você verá vários arquivos `.asset` sendo criados

### 4️⃣ Pronto!
Os diálogos estão prontos para usar

---

## 📖 Arquivos Importantes

| Arquivo | O que é |
|---------|---------|
| `DialoguesData.json` | Base de dados com todos os diálogos |
| `DialogueDataLoader.cs` | Gera os ScriptableObjects |
| `DialogueSequenceManager.cs` | Gerenciador de acesso aos diálogos |
| `DialogueSceneExample.cs` | Exemplos de como usar |
| `README.md` | Documentação completa |
| `NARRATIVE_STRUCTURE.md` | Estrutura visual da narrativa |

---

## 💻 Usar nos Seus Scripts

```csharp
// Carregar diálogo do Capítulo 1, Cena 1
DialogueSequenceManager.DialogueSequence seq = 
    DialogueSequenceManager.GetDialogueSequence(1, 1);

// Iniciar diálogo
dialogueManager.StartDialogue(seq.dialogueLines);
```

Veja `DialogueSceneExample.cs` para mais exemplos!

---

## 🎯 Estrutura dos Capítulos

- **Cap I**: Baile de Meryton (1 cena interativa)
- **Cap II**: Rumores e Verdades (1 cena interativa)
- **Cap III**: A Verdade Revelada (1 cena narrativa)
- **Cap IV**: O Escândalo (1 cena narrativa)
- **Cap VIII**: Reconciliação (1 cena interativa com final)

---

## ❓ Dúvidas?

📖 Leia `README.md` em Assets/DialogueData/

---

**Tudo pronto! Divirta-se com o jogo! 🎮**
