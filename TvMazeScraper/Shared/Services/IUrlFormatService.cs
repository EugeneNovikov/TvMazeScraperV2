namespace Shared.Services
{
    public interface IUrlFormatService
    {
        string FormatUrlComponent(string urlComponent, params object[] urlParameters);
    }
}