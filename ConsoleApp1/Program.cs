using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.DependencyInjection;

namespace OllamaWithSemanticKernel
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // llama2 in Ubuntu local in WSL
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
            history.AddUserMessage("嗨，你是誰？");

            // print response
            var result = await chat.GetChatMessageContentsAsync(history);
            Console.WriteLine(result[^1].Content);
        }
    }
}
