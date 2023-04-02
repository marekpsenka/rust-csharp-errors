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
unfortunately. Documenting modes of failure is far inferior to explicitly including it in 
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