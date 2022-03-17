using System.Collections.Generic;

namespace TimeReporter.Core.Storage
{
    public interface IStorageManager<TData, TParameter> : IStorageReader<TData, TParameter>
    {
        void Save(IEnumerable<TData> data);
    }

    public interface IStorageManager<TData> : IStorageReader<TData>
    {
        void Save(IEnumerable<TData> data);
    }
}
