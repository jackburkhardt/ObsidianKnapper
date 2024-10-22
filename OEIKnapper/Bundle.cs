﻿namespace OEIKnapper;

public class Bundle<T> : List<T> where T : IBundleItem
{
    public T this[Guid id] => this.First(i => i.ID == id);
    public T this[string name] => this.First(i => i.Filename == name);
}

public interface IBundleItem
{
    public Guid ID { get; set; }
    public string Filename { get; set; }
}