
public abstract class PersistableData
{
    public abstract void Save(GameDataWriter writer);

    public static T Load<T>(GameDataReader reader) where T : PersistableData, new()
    {
        T data = new T();
        data.Init(reader);

        return data;
    }

    protected abstract void Init(GameDataReader reader);
}
