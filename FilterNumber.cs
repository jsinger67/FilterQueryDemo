namespace FilterQueryDemo;

using Parol.Runtime;

public readonly struct FilterNumber
{
    public string Text { get; }

    public FilterNumber(string text)
    {
        Text = text;
    }

    public FilterNumber(Token token)
    {
        Text = token.Text;
    }

    public FilterNumber(NumberLit value)
    {
        Text = value.NumberLitValue.Text;
    }

    public override string ToString() => Text;
}
