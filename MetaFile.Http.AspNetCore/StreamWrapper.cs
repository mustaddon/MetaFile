﻿using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MetaFile.Http.AspNetCore;

internal sealed class StreamWrapper : Stream
{
    internal StreamWrapper(HttpRequest httpRequest)
    {
        _httpRequest = httpRequest;
    }

    readonly HttpRequest _httpRequest;

    protected override void Dispose(bool disposing)
    {
        _httpRequest.Body.Dispose();
        base.Dispose(disposing);
    }

    public override bool CanRead => _httpRequest.Body.CanRead;
    public override bool CanSeek => _httpRequest.Body.CanSeek;
    public override bool CanWrite => _httpRequest.Body.CanWrite;
    public override long Length => _httpRequest.ContentLength ?? -1;
    public override long Position { get => _httpRequest.Body.Position; set => _httpRequest.Body.Position = value; }
    public override void Flush() => _httpRequest.Body.Flush();
    public override Task FlushAsync(CancellationToken cancellationToken) => _httpRequest.Body.FlushAsync(cancellationToken);
    public override int Read(byte[] buffer, int offset, int count) => _httpRequest.Body.Read(buffer, offset, count);
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => _httpRequest.Body.ReadAsync(buffer, offset, count, cancellationToken);
    public override long Seek(long offset, SeekOrigin origin) => _httpRequest.Body.Seek(offset, origin);
    public override void SetLength(long value) => _httpRequest.Body.SetLength(value);
    public override void Write(byte[] buffer, int offset, int count) => _httpRequest.Body.Write(buffer, offset, count);
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => _httpRequest.Body.WriteAsync(buffer, offset, count, cancellationToken);
}