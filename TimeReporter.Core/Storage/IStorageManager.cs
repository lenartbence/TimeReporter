using System.Collections.Generic;

namespace TimeReporter.Core.Storage
{
    public interface IStorageManager<TData, TParameter>
    {
        void Save(IEnumerable<TData> data);

        IEnumerable<TData> Load(TParameter parameter);
    }

    public interface IStorageManager<TData>
    {
        void Save(IEnumerable<TData> data);

        IEnumerable<TData> Load();
    }
}
