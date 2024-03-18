# C# SemanticKernel 連接 Ollama 使用 Gemma 開源模型

在 C# 裡使用 SemanticKernel 連接本地 Ollama 使用 Gemma 開源模型作 ChatBot 範例

- Gemma是Google的DeepMind 和AI 研究部門的其他團隊共同開發的第一個使用與Gemini模型相同的研究和技術建構的開源LLM。這個系列的模型目前有兩種尺寸，2B和7B，並且提供了聊天的基本版和指令版
- [Ollama](https://link.zhihu.com/?target=https%3A//github.com/ollama/ollama)支援在macos、linux、windows上執行各種開源LLM。Ollama 將模型權重、配置和資料捆綁到一個套件中，定義成Modelfile。它優化了設定和配置細節，包括GPU 使用情況
- Semantic Kernel 一個微軟開源的AI 軟體開發工具（SDK），基於此SDK 可以輕鬆建立安全自動化的智慧體（Agent）。目前支援C#、Python 以及Java 三種語言。Semantic Kernel 透過Connectors 和Plugins 將LLM 的強大能力和真實業務連接起來，來自動化業務流程並幫助使用者實現更多目標

範例程式: https://github.com/antonylu0826/OllamaWithSemanticKernel

![image](https://github.com/antonylu0826/OllamaWithSemanticKernel/assets/92000976/9e927af4-335c-4ab1-a4fe-2bb6ab06351f)

semantic kernel 對於非OpenAI的模型，需要透過自訂LLM的方式來連接。所以這裡實作了CustomChatCompletionService 物件

主程式:

```docker
var ollamaChat = new CustomChatCompletionService();
ollamaChat.ModelUrl = "http://localhost:11434/v1/chat/completions";
ollamaChat.ModelName = "gemma:2b";

// semantic kernel builder
var builder = Kernel.CreateBuilder();
builder.Services.AddKeyedSingleton<IChatCompletionService>("ollamaChat", ollamaChat);
var kernel = builder.Build();

// init chat
var chat = kernel.GetRequiredService<IChatCompletionService>();
var history = new ChatHistory();
history.AddSystemMessage("您是一位有用的助手，可以使用有趣的風格和表情符號來回應。 你的名字是悟空。使用繁體中文對談");

// Start the conversation
Console.Write("User > ");
string? userInput;
while ((userInput = Console.ReadLine()) != null)
{
    // Add user input
    history.AddUserMessage(userInput);

    // Get the response from the AI
    var result = await chat.GetChatMessageContentsAsync(
        history
    );

    // Print the results
    Console.WriteLine("Assistant > " + result[^1].Content);

    // Add the message from the agent to the chat history
    history.AddMessage(result[^1].Role, result[^1].Content ?? string.Empty);

    // Get user input again
    Console.Write("User > ");
}
```
