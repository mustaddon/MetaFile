using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MetaFile.Http;

public static class MetaFileExtensions
{
    public static async Task<T?> ReadAsStreamFile<T>(this HttpResponseMessage response, JsonSerializerOptions? jsonOptions = null, CancellationToken cancellationToken = default)
        where T : IStreamFile
    {
        var file = await ReadAsStreamFile(response, typeof(T), jsonOptions, cancellationToken);
        return (T?)file;
    }

    public static async Task<IStreamFile?> ReadAsStreamFile(this HttpResponseMessage response, Type type, JsonSerializerOptions? jsonOptions = null, CancellationToken cancellationToken = default)
    {
        var file = (IStreamFile)(Activator.CreateInstance(type) ?? throw new ArgumentException($"Can not create instance of '{type}'."));

        file.Name = response.Content.Headers.ContentDisposition?.FileNameStar ?? response.Content.Headers.ContentDisposition?.FileName;
        file.Type = response.Content.Headers.ContentType?.MediaType;
        file.Content = new StreamWrapper(await response.Content.ReadAsStreamAsync(cancellationToken), () => response.Dispose());

        if (file is IHttpFile httpFile)
            httpFile.InlineDisposition = string.Equals(DispositionTypeNames.Inline, response.Content.Headers.ContentDisposition?.DispositionType, StringComparison.InvariantCultureIgnoreCase);

        if (response.Headers.TryGetValues(MetaFileHeaders.Metadata, out var metadataValues) && metadataValues.Any())
        {
            var metadataType = type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMetaFile<>))?.GenericTypeArguments.First();

            if (metadataType != null)
            {
                var metadata = JsonSerializer.Deserialize(Uri.UnescapeDataString(metadataValues.First()), metadataType, jsonOptions);
                var metadataProp = type.GetProperty(nameof(IMetaFile<int>.Metadata));
                metadataProp!.SetValue(file, metadata);
            }
        }

        return file;
    }

    public static Task<HttpResponseMessage> PostAsStreamFile(this HttpClient client, string uri, IStreamFileReadOnly file, JsonSerializerOptions? jsonOptions = null, CancellationToken cancellationToken = default)
    {
        var content = new StreamContent(file.Content);

        if (file.Type != null)
            content.Headers.ContentType = new MediaTypeHeaderValue(file.Type);

        content.Headers.ContentDisposition = new ContentDispositionHeaderValue(DispositionTypeNames.Attachment)
        {
            FileName = file.Name,
            FileNameStar = file.Name,
        };

        if (file is IStreamFileReadOnly<object> filePlus && filePlus.Metadata != null)
            content.Headers.TryAddWithoutValidation(MetaFileHeaders.Metadata, Uri.EscapeDataString(JsonSerializer.Serialize(filePlus.Metadata, jsonOptions)));

        return client.PostAsync(uri, content, cancellationToken);
    }


#if NETSTANDARD2_0
#pragma warning disable IDE0060 // Remove unused parameter
    public static Task<Stream> ReadAsStreamAsync(this HttpContent httpContent, CancellationToken cancellationToken) => httpContent.ReadAsStreamAsync();
    public static Task<string> ReadAsStringAsync(this HttpContent httpContent, CancellationToken cancellationToken) => httpContent.ReadAsStringAsync();
#pragma warning restore IDE0060 // Remove unused parameter
#endif

}
