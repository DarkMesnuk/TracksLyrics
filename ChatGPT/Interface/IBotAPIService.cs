namespace ChatGPT.Interface;

public interface IBotApiService
{
    Task<List<string>> Send(string message);
}