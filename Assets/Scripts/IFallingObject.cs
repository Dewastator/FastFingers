
public interface IFallingObject
{
    public void Destroy(bool value);

    public void SetText(string text);

    public string GetText();
    public void SetWrongText(string text);

    public string GetWrongText();

    public bool Enabled();

    public bool IsAlreadyDead();

    public void Enable(bool enable);
}