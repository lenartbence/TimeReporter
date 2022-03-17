using System.Collections.Generic;

namespace TimeReporter.Core.Storage
{
    public interface IStorageReader<TData, TParameter>
    {
        IEnumerable<TData> Load(TParameter parameter);
    }

    public interface IStorageReader<TData>
    {
        IEnumerable<TData> Load();
    }
}
