namespace FilterQueryDemo;

using Parol.Runtime;

public readonly struct FilterIdentifier
{
    public string Text { get; }

    public FilterIdentifier(string text)
    {
        Text = text;
    }

    public FilterIdentifier(Token token)
    {
        Text = token.Text;
    }

    public FilterIdentifier(Identifier value)
    {
        Text = value.IdentifierValue.Text;
    }

    public override string ToString() => Text;
}
