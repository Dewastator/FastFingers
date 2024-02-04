public interface IFallingObject
{
    public void Destroy();

    public void SetText(string text);

    public string GetText();
    public void SetWrongText(string text);

    public string GetWrongText();
}