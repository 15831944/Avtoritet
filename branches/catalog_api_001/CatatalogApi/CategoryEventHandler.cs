using System;
using System.IO;
using CatalogApi.Exceptions;
using CatalogApi.Settings;
using CodeTools.Extensions;

namespace CatalogApi
{
    // TODO: Найти использование в коде, заменить на кастомную реализацию.
    public class CategoryEventHandler : ICategoryEventHandler
    {
        private readonly string fileName;

        public CategoryEventHandler(string category)
        {
            fileName = "{0}_errors.txt".FormatString(category);
        }

        public void ProcessException(Exception ex)
        {
            var file = Path.Combine(ResourceManager.Root, fileName);
            System.IO.File.AppendAllText(file
                , "{1} Exception: {0}'{2}'{0}".FormatString(
                    Environment.NewLine
                    , string.Format("{0:d/M/yyyy HH:mm:ss}", DateTime.Now)
                    , ex)
                );
        }
    }
}