dotnet build -c Release 
dotnet pack .\MetaFile\ -c Release -o ..\_publish
dotnet pack .\MetaFile.Http\ -c Release -o ..\_publish
dotnet pack .\MetaFile.Http.AspNetCore\ -c Release -o ..\_publish
