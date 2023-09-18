# Lox

This repository houses the implementation of Lox, a dynamically typed toy language written in C#.

## Data Types

Lox comprises the following data types:

### Booleans

There exist two Boolean values, each accompanied by a corresponding literal in Lox:

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

Lox supports strings, which are enclosed in double quotes:

```c#
"I am a string";
""; // Represents the empty string.
"123"; // This is a string, not a number.
```

### Nil

In Lox, a distinct data type signifies the absence of a value, which we refer to as "nil":

```c#
nil; // Represents a value with no content.
```

## Expressions

If built-in data types and their literals are atoms, then expressions must be the molecules.

### Arithmetic

Lox features basic arithmetic operators, including subtraction, addition, multiplication, and division:

```c#
add + me;
subtract - me;
multiply * me;
divide / me;
```

All these operators work exclusively with numbers, and attempting to use other types will result in an error. The exception is the '+' operator, which can also concatenate two strings.

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
