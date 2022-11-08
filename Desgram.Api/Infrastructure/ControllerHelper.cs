using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Infrastructure
{
    public static class ControllerHelper
    {
        public static string NameOfController<T>()where T:ControllerBase
        {
            return typeof(T).Name.Replace("Controller", "");
        }
    }
}
