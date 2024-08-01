using ChatGPT.Model;

namespace ChatGPT.Interface;

public interface IProductService
{
    Task<ResponseModel> SendMessage(string message);
}