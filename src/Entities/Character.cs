
namespace CompositionLearning.src.Entities;

public class Character
{
    public string Name { get; set; }
    public string Type { get; set; }

    public Character(string name, string type)
    {
        Name = name; Type = type;
    }
}
