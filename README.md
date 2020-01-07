[![Build and Deployment](https://github.com/menoret-allan/mazinator/workflows/Build%20and%20Deploy/badge.svg)](https://github.com/menoret-allan/mazinator/actions)

# Maze generator (mostly to play with property testing on an existing algorythm that need changes)

The blazor app is deploy on [my personal website](https://maze.allanmenoret.com/)

[WIKI for maze generation](https://en.wikipedia.org/wiki/Maze_generation_algorithm)

I play around to generate a perfect maze with a random algo.

Then I created some "property tests" to verify that mazes were correct.

Then I implemented a new algo. Since the maze is still perfect I could apply the property tests to it (which was the whole point of this little experiment 🙂).

## Project setup
```
dotnet restore
```

### Build
```
dotnet build
```

### Tests
```
dotnet test
```

### Run console with the "speed test"
```
dotnet run -p ./Maze.Console/Maze.Console.csproj
```

### Run the blazor app with the view
```
dotnet run -p ./Maze.Front/Maze.Front.csproj
```

run [there](http://localhost:56037/) localhost port 56037

