using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace OEIKnapper;

public class Bundle<T> : ObservableCollection<T> where T : IBundleItem
{
    public T this[Guid id] 
    {
        get => this.First(i => i.ID == id);
        set {
            if (this.Any(i => i.ID == id))
            {
                this[this.IndexOf(this.First(i => i.ID == id))] = value;
            }
            else
            {
                Add(value);
            }
            Console.WriteLine("Updated " + id + " to " + JsonConvert.SerializeObject(value));
        }
    }
    public T this[string name]
    {
        get => this.First(i => i.Tag == name);
        set
        {
            if (this.Any(i => i.Tag == name))
            {
                this[this.IndexOf(this.First(i => i.Tag == name))] = value;
            }
            else
            {
                Add(value);
            }
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