namespace Product_Catalog.Services
{
    public interface ITranslationService
    {
        Task<string> TranslateAsync(string text, string targetLanguage);
    }
}
