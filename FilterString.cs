namespace FilterQueryDemo;

using Parol.Runtime;

public readonly struct FilterString
{
    public string Text { get; }

    public FilterString(string text)
    {
        Text = text;
    }

    public FilterString(Token token)
    {
        Text = token.Text;
    }

    public FilterString(StringLit value)
    {
        Text = value.StringLitValue.Text;
    }

    public override string ToString() => Text;
}
