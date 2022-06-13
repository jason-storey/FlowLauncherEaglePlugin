using Eagle;
using Eagle.Models;

Api api = new Api();

var result = "";
do
{
    say("what?");
    result = read();
    switch (result)
    {
        case "status":
            var info = await api.Status(CancellationToken.None);
            say(info.status);
            break;
    }
} while (IsNot(result, "quit"));

bool IsNot(string a,string b) => !Is(a, b);
bool Is(string a,string b) => a.Equals(b, StringComparison.OrdinalIgnoreCase);
string read() => Console.ReadLine();
void say(string message) => Console.WriteLine(message);