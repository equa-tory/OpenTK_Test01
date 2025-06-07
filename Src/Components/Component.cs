namespace Toryngine;

public abstract class Component
{
    public GameObject? owner = null;

    public virtual void Update() { }
}