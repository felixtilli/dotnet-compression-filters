using System.Web.Http.Filters;

public class DeflateCompressionAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(HttpActionExecutedContext context)
    {
        base.OnActionExecuted(CompressionHelper.GetCompressedHttpActionExecutedContext(context, CompressionHelper.CompressionType.Deflate));
    }
}