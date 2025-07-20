### ## Code Style and Structure

* Write concise, idiomatic C# code with correct and relevant examples.
* Follow .NET conventions and best practices.
* Use object-oriented and functional patterns where appropriate.
* Prefer LINQ and lambda expressions for collection operations.
* Use descriptive, self-explanatory names (e.g., `IsUserSignedIn`, `CalculateTotal`).
* Avoid deeply nested code structures. Favor early returns, continues, or breaks to reduce nesting depth and enhance readability. Extract methods and encapsulate complex conditions clearly.
* Use regions sparingly, only for large classes or files to improve readability. 
---

### ## Naming Conventions

* **PascalCase**: class names, method names, public members.
* **camelCase**: local variables and private fields.
* **UPPERCASE**: constants.
* Prefix interfaces with `I` (e.g., `IUserService`).
* Use meaningful names for methods, properties, and parameters.
* Avoid abbreviations unless they are well-known (e.g., `GetUserById` instead of `GetUsrById`).
* Use suffixes like `Async` for asynchronous methods (e.g., `GetUserAsync`).
---

### ## C# and .NET Usage

* Use C# 10+ features (e.g., `record` types, null-coalescing assignment).
* Use **pattern matching** when applicable (e.g., `if (value is 1)` instead of `if (value == 1)`).
* Use **global usings** and **file-scoped namespaces**.

---

### ## Syntax and Formatting

* Follow [C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
* Use modern syntax features (e.g., null-conditional operators, string interpolation).
* Use `var` when the type is obvious and improves readability.
* For single-statement conditionals, omit braces **only if** it improves clarity:

  ```csharp
  if (isValid)
      return true;
  ```

---

### ## Testing

* Write **unit tests** with **xUnit**.
* Use **NSubstitute** for mocking dependencies.
* Use **AwesomeAssertions** for expressive assertions.
* Use Arrange, Act, Assert comments in a unit test.
* Use 'dotnet test <SolutionName>' for runs each test project in turn 
* Don't use --no-build if the solution has been changed since the last build. 
* Use the next naming rule for unit tests <NameOfTestedMethod>_When<Scenario>_Should<ExpectedResult>
* Use the name of a tested class with the suffix Should for test classes (e.g., `UserServiceShould`).
* Inherit folder structure from tested class (e.g., `Services/UserServiceShould.cs`). 
---

### ## Git
* Follow [Conventional Commits] https://www.conventionalcommits.org/en/v1.0.0/
* Do not be too verbose in commit messages, but provide enough context.
---
