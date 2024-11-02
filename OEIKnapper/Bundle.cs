using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace OEIKnapper;

public class Bundle<T> : ObservableCollection<T> where T : IBundleItem
{
    public T this[Guid id] 
    {
        get => this.First(i => i.ID == id);
        set => this[this.IndexOf(this.First(i => i.ID == id))] = value;
    }
    public T this[string name]
    {
        get => this.First(i => i.Tag == name);
        set
        {
            this[this.IndexOf(this.First(i => i.Tag == name))] = value;
            Console.WriteLine("Updated " + name + " to " + JsonConvert.SerializeObject(value));
        }
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }
}

public interface IBundleItem
{
    public Guid ID { get; set; }
    public string Tag { get; set; }
}