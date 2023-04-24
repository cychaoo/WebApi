using Microsoft.Extensions.Configuration;
using System;

namespace WebApiMaxine.Extensions
{
    public static class CustomConfigProviderExtensions
    {
        public static IConfigurationBuilder AddEncryptConfProvider(this IConfigurationBuilder builder
        , string path, string key, string ivKey, bool reloadOnChange)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (!File.Exists(path))
                throw new FileNotFoundException($"{path} not found");

            var source = new EncryptJsonConfSource
            {
                FileProvider = null,
                Optional = false,
                Path = path,
                ReloadOnChange = reloadOnChange,
                Key = key,
                IV = ivKey
            };

            return builder.Add(source);
        }
    }
}
