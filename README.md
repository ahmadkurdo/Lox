# Lox

This repository hosts the implementation of Lox, a dynamically typed toy language developed in C#.

## Data Types

Lox encompasses various data types:

### Booleans

There are two Boolean values, each with a corresponding literal in Lox:

```c#
true; // Represents true.
false; // Represents false.
```

### Numbers

Lox employs double-precision floating-point numbers for all numeric values:

```c#
1234; // An integer.
12.34; // A decimal number.
```

### Strings

Lox supports strings, enclosed in double quotes:

```c#
"I am a string";
""; // Represents the empty string.
"123"; // This is a string, not a number.
```

### Nil

In Lox, a distinct data type signifies the absence of a value, referred to as "nil":

```c#
nil; // Represents a value with no content.
```

## Expressions

The grammar of Lox's expressions is the following:
```c#
expression → equality
equality → comparison (( "!=" | "==" ) comparison)*
comparison → term (( "<" | ">" | "<=" | ">=" ) term)*
term → factor (( "+" | "-" ) factor)*
factor → unary (( "*" | "/" ) unary)*
unary → ( "!" | "-") unary | primary
primary → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")"
```

### Arithmetic

Lox features basic arithmetic operators, including subtraction, addition, multiplication, and division:

```c#
add + me;
subtract - me;
multiply * me;
divide / me;
```

All these operators exclusively work with numbers, with the exception of the '+' operator, which can also concatenate two strings.

### Comparison and Equality

In Lox, several operators return a Boolean result. You can compare numbers using Comparison Operators:

```c#
less < than;
lessThan <= orEqual;
greater > than;
greaterThan >= orEqual;
```

You can also test two values of any kind for equality or inequality:

```c#
1 == 2; // false.
"cat" != "dog"; // true.
```

Even different types can be compared:

```c#
314 == "pi"; // false.
```

### Logical Operators

Lox supports logical operators, including the '!' (not) operator, the 'and' operator, and the 'or' operator:

```c#
false or false; // false.
true or false; // true.
```

## Statements

While expressions produce values, statements produce effects. Statements change the program's state, read input, or produce output.

```c#
print "Hello, world!";
```

A `print` statement evaluates a single expression and displays the result to the user.

## Variables

In Lox, you declare variables using `var` statements. If you omit the initializer, the variable's value defaults to `nil`:

```c#
var imAVariable = "here is my value";
var iAmNil;
```

Once declared, you can access and assign a variable using its name:

```c#
var breakfast = "bagels";
print breakfast; // "bagels".
breakfast = "beignets";
print breakfast; // "beignets".
```

## Control Flow

Lox includes three statements from C: `if`, `while`, and `for`.

An `if` statement executes one of two statements based on some condition:

```c#
if (condition) {
  print "yes";
} else {
  print "no";
}
```

A `while` loop executes the body repeatedly as long as the condition expression evaluates to true:

```c#
var a = 1;
while (a < 10) {
  print a;
  a = a + 1;
}
```

Lastly, there are `for` loops:

```c#
for (var a = 1; a < 10; a = a + 1) {
  print a;
}
```

## Functions

A function call expression in Lox looks as follows:

```c#
makeBreakfast(bacon, eggs, toast);
```

To define a function in Lox, you use the `fun` keyword:

```c#
fun printSum(a, b) {
  print a + b;
}
```

Functions are first-class in Lox, meaning they can be treated as values:

```c#
fun printSum(a, b) {
  print a + b;
}

fun identity(a) {
  return a;
}

print identity(addPair)(1, 2); // Prints "3".
```

You can also declare local functions inside another function:

```c#
fun outerFunction() {
  fun localFunction() {
    print "I'm local!";
  }
  localFunction();
}
```

## Classes

In Lox, you declare a class and its methods like so:

```c#
class Breakfast {
  cook() {
    print "Eggs a-fryin'!";
  }
  serve(who) {
    print "Enjoy your breakfast, " + who + ".";
  }
}
```

To create instances in Lox, you call the class as if it were a function:

```c#
var breakfast = Breakfast();
print breakfast; // "Breakfast instance".
```

You can freely add properties to objects:

```c#
breakfast.meat = "sausage";
breakfast.bread = "sourdough";
```

To access a field or method on the current object from within a method, you use `this`:

```c#
class Breakfast {
  serve(who) {
    print "Enjoy your " + this.meat + " and " +
      this.bread + ", " + who + ".";
  }
  // ...
}
```

To ensure that objects are in a valid state when created, you can define an initializer:

```c#
class Breakfast {
  init(meat, bread) {
    this.meat = meat;
    this.bread = bread;
  }
  // ...
}
var baconAndToast = Breakfast("bacon", "toast");
baconAndToast.serve("Dear Reader");
// "Enjoy your bacon and toast, Dear Reader."
```

## Inheritance

Lox supports single inheritance, where a derived class inherits from a base class using the `<` operator:

```c#
class Brunch < Breakfast {
  drink() {
    print "How about a Bloody Mary?";
  }
}
```

Methods defined in the superclass are available to its subclasses:

```c#
var benedict = Brunch("ham", "English muffin");
benedict.serve("Noble Reader");
```
