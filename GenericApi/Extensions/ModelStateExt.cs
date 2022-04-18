using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace GenericApi.Extensions
{
    public static class ModelStateExt
    {
        public static List<string> ErrorMessages(this ModelStateDictionary dictionary)
        {
            return dictionary.SelectMany(e=> e.Value!.Errors).Select(e=> e.ErrorMessage).ToList();
        }
    }
}
