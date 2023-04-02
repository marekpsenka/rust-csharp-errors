# Improving C# error handling by studying Rust

At the end of 2022, me and my colleagues were provided an opportunity to study Rust by applying it
to solve problems that might come up in our daily line of work.

## Rust makes possibility of failure explicit

As we made our way through the basics of Rust, the topic of error handling came up quite early
in our journey. To me, the fact that we had to think about error handling early on, proves
an important characteristic of the language: the goal is to make possible failure visible
and handling is intended as an opt-out concept.
In higher-level languages of the C family, you can get very far without touching on the subject.

I will illustrate this on something which new learners of a programming language encounter very soon. Let's see an example of how Rust makes the possibility of failure visible. I wrote this simple
test that uses  [`Stdin::read_line()`](https://doc.rust-lang.org/std/io/struct.Stdin.html#method.read_line) to read a line from STDIN to a string

```Rust
#[test]
fn build_me_and_get_a_warning() {
    let mut s = String::new();
    std::io::stdin().read_line(&mut s);
    println!("{s}");
}
```

First of all, the signature

```Rust
read_line(&self, buf: &mut String) -> Result<usize>
```

immediately tells you through the `Result` return type that the operation may fail. Though I am
sure that most programmers do not read method signatures up front when focused on solving
a particular problem. This is where the power of automation comes in.

When I compile it, `rustc` gives me a clear warning that i forgot to use the `Result` type
associated with
```
warning: unused `Result` that must be used
  --> src\lib.rs:18:9
   |
18 |         std::io::stdin().read_line(&mut s);
   |         ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
   |
   = note: `#[warn(unused_must_use)]` on by default
   = note: this `Result` may be an `Err` variant, which should be handled
```

The functionally equivalent code in C# compiles without any trouble

```C#
[Test]
public void BuildingThisGivesYouNoWarning()
{
    var line = Console.ReadLine();
}
```

Reading a line from the console may fail in C#, but you can only learn that from the docstring
unfortunately. Documenting modes of failure is far inferior to explicitly including them in 
the return value.

```
Exceptions:
    T:System.IO.IOException:
    An I/O error occurred.

    T:System.OutOfMemoryException:
    There is insufficient memory to allocate a buffer for the returned string.

    T:System.ArgumentOutOfRangeException:
    The number of characters in the next line of characters is greater than Int32.MaxValue.
```

## The many pain points of exceptions

## Exceptions can easily break encapsulation

One problem I encounter very often in practice originates in the fact that it is very easy to break
encapsulation when using exceptions incorrectly. Consider the interface:

```C#
public interface INewsService
{
    Task<List<string>> GetLatestNews();
}
```

The point of creating interfaces is to decouple pieces of code by having them depend on a common
abstraction instead of letting them know the implementation of the other. The intent behind 
`INewsService` in a larger application will be to inject a particular implementation into some
higher application logic that performs complex tasks.

Suppose that a developer called Bob implements the `INewsService` interface like this:

```C#
    public async Task<List<string>> GetLatestNews()
    {
        var response = await _httpClient.GetAsync("http://someWrongAdress831");

        // Here the implementation would convert response into a list
        // of the latest news. We return a made up list for the sake of 
        // the example.
    }
```

Another developer called Sam is responsible for creating a news reader for the user interface.
The body of the test method below represents what Sam might write working only with the interface:

```C#
public void SadNewsReader()
{
    // Picture the NewsReader resolving `BadHttpNewsService` via DI
    INewsService newsService = new BadHttpNewsService();

    Assert.ThrowsAsync<HttpRequestException>(
        async () => {
            var latestNews = await newsService.GetLatestNews();
            foreach (var piece in latestNews)
                Console.WriteLine(piece);
        }
    );
}
```

TODO: Rename BadHttpNewsService -> BadNewsService

When Sam obtains the implementation of `INewsService` via `BadHttpNewsService` Bob,
they will be unpleasantly surprised. The `GetLatestNews` method will throw a `HttpRequestException`.
The problem is that this reveals an implementation detail of `BadHttpNewsService`, betraying
the purpose of having an interface.

Sam, being under pressure to deliver fast, resorts to handling the `HttpRequestException`
like this:

```C#
try
{
    var latestNews = newsService.GetLatestNews();
}
catch (HttpRequestException) // No not like this !!!
{
        // This is where encapsulation would be broken. The role of the interface is to
        // hide implementation details!
}
```

## Trouble with granularity of handling


