using System;

namespace ConfigurationProvider
{
    public interface IConfigurationParser<out T>
    {
        T Parse();
    }
}
