using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Web.Http.Filters;

public class CompressionHelper
{
    public enum CompressionType
    {
        Deflate,
        Gzip
    }

    public static HttpActionExecutedContext GetCompressedHttpActionExecutedContext(HttpActionExecutedContext context, CompressionType compressionType)
    {
        var content = new byte[0];

        if (context.Response.Content != null)
        {
            var bytes = context.Response.Content.ReadAsByteArrayAsync().Result;

            if (bytes != null)
            {
                content = CompressionHelper.Compress(bytes, compressionType, CompressionLevel.Fastest);
            }
        }

        context.Response.Content = new ByteArrayContent(content);
        context.Response.Content.Headers.Remove("Content-Type");
        context.Response.Content.Headers.Add("Content-encoding", compressionType.ToString().ToLower());
        context.Response.Content.Headers.Add("Content-Type", "application/json");

        return context;
    }

    public static byte[] Compress(byte[] str, CompressionType compressionType, CompressionLevel compressionLevel)
    {
        using (var output = new MemoryStream())
        {
            var compressor = (Stream)null;

            if (compressionType == CompressionType.Deflate)
            {
                compressor = new DeflateStream(output, compressionLevel);
            }
            else if (compressionType == CompressionType.Gzip)
            {
                compressor = new GZipStream(output, compressionLevel);
            }

            using (compressor)
            {
                compressor.Write(str, 0, str.Length);
            }

            return output.ToArray();
        }
    }
}