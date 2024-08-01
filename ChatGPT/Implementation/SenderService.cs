using ChatGPT.Interface;
using ChatGPT.Model;

namespace ChatGPT.Implementation;

public class SenderService : IProductService
{
    private readonly IBotApiService _botApiService;

    public SenderService(IBotApiService botApiService)
    {
        _botApiService = botApiService;
    }

    public async Task<ResponseModel> SendMessage(string message)
    {
        if(string.IsNullOrEmpty(message))
        {
            return new ResponseModel
            {
                Success = false,
                Contents = new List<string>()
            };
        }
        
        var generate = await _botApiService.Send(message);

        return new ResponseModel
        {
            Success = generate.Count != 0,
            Contents = generate
        };
    }
}
