namespace MetaFile;

public interface IByteFile : IContentFile<byte[]>, IByteFileReadOnly
{

}

public interface IByteFile<TMetadata> : IByteFile, IContentFile<byte[], TMetadata>, IByteFileReadOnly<TMetadata>
{

}
