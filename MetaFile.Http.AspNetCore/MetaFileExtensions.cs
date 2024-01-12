using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

namespace MetaFile.Http.AspNetCore;

public static class MetaFileExtensions
{
    public static IResult ToResult(this IStreamFileReadOnly file, HttpResponse httpResponse, JsonSerializerOptions jsonOptions)
    {
        var dispositionType = file is IHttpFileReadOnly { InlineDisposition: true }
            ? DispositionTypeNames.Inline
            : DispositionTypeNames.Attachment;

        httpResponse.Headers.ContentDisposition = new ContentDispositionHeaderValue(dispositionType)
        {
            FileName = file.Name,
            FileNameStar = file.Name,
        }.ToString();

        if (file is IStreamFileReadOnly<object> filePlus && filePlus.Metadata != null)
            httpResponse.Headers[MetaFileHeaders.Metadata] = Uri.EscapeDataString(JsonSerializer.Serialize(filePlus.Metadata, jsonOptions));

        return Results.Stream(file.Content, file.Type);
    }

    public static IStreamFile? ToStreamFile(this HttpRequest httpRequest, Type type, JsonSerializerOptions jsonOptions)
    {
        var file = (IStreamFile)(Activator.CreateInstance(type) ?? throw new Exception($"Can not create instance of '{type}'."));

        file.Content = new StreamWrapper(httpRequest);

        if (MediaTypeHeaderValue.TryParse(httpRequest.Headers.ContentType, out var contentType))
            file.Type = contentType.MediaType;

        if (ContentDispositionHeaderValue.TryParse(httpRequest.Headers.ContentDisposition, out var contentDisposition))
            file.Name = contentDisposition.FileNameStar ?? contentDisposition.FileName;

        if (httpRequest.Headers.ContainsKey(MetaFileHeaders.Metadata))
        {
            var metadataType = type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IStreamFile<>))?.GenericTypeArguments.First();

            if (metadataType != null && httpRequest.Headers.TryGetValue(MetaFileHeaders.Metadata, out var metadataHeaderValue))
            {
                var metadata = JsonSerializer.Deserialize(Uri.UnescapeDataString(metadataHeaderValue!), metadataType, jsonOptions);
                var metadataProp = type.GetProperty(nameof(IStreamFile<int>.Metadata));
                metadataProp!.SetValue(file, metadata);
            }
        }

        return file;
    }

}
