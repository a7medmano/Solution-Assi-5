Search Task
Access Modifiers 

protected internal

بيسمح بالوصول للمتغير من نفس الـ Assembly، أو من كلاس وارث حتى لو في Assembly تاني.


protected internal int number = 10;

class Parent
{
    protected internal int number = 10;
}

class Child : Parent
{
    public void Show()
    {
        Console.WriteLine(number);
    }
}


private protected

بيسمح بالوصول من كلاس وارث،  بشرط يكون في نفس الـ Assembly.

private protected int number = 20;

class Parent
{
    private protected int number = 20;
}

class Child : Parent
{
    public void Show()
    {
        Console.WriteLine(number);
    }
}
