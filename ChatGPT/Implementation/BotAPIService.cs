using ChatGPT.Interface;
using OpenAI_API;
using OpenAI_API.Chat;

namespace ChatGPT.Implementation;

public class BotApiService : IBotApiService
{
    private const string ApiKey = "";
    private const string ApiModel = "gpt-3.5-turbo";
    
    public async Task<List<string>> Send(string message)
    {
        var response = new List<string>();
        var api = new OpenAIAPI(new APIAuthentication(ApiKey));
        
        var completionRequest = new ChatRequest {
            Messages = new List<ChatMessage> { 
                new ChatMessage { 
                    Content = message,
                    Role = ChatMessageRole.User
                } 
            },
            Model = ApiModel
        };
        
        var result = await api.Chat.CreateChatCompletionAsync(completionRequest);
        
        response.AddRange(result.Choices.Select(chatChoice => chatChoice.Message.Content));

        return response;
    }
}
