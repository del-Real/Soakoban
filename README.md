# Soakoban

Sokoban solver developed with [Raylib C#](https://github.com/raylib-cs/raylib-cs)

![](./demo.gif)


## Dependencies

Before building, ensure you have the following installed:

- **.NET SDK 8.0** or later – [Download here](https://dotnet.microsoft.com/en-us/download)
- **Raylib C# bindings** – Included via [raylib-cs](https://github.com/raylib-cs/raylib-cs)
  

## Build

```console
$ git clone https://github.com/del-Real/Soakoban.git
$ cd Soakoban/src
$ dotnet build
```

## Run

```console
$ cd ../build/bin/net8.0
$ ./sokoban '###########\n####  @#  #\n#### #    #\n####  $#  #\n# $.  .## #\n#   ###  $#\n#   ###  .#\n###########' A\* 100
```



