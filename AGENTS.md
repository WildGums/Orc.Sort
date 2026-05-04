# Orc.Sort

Orc.Sort is a library containing various sorting algorithms for .NET applications. It provides implementations of well-known and specialized sorting algorithms, including NSort, TimSort, TopologicalSort, and TemplateSort.

Orc.Sort consists of the following components:

- **NSort** — Classic sorting algorithms: BubbleSort, HeapSort, QuickSort, MergeSort, ShellSort, InsertionSort, SelectionSort, ShakerSort, and more.
- **TimSort** — A stable, adaptive, iterative merge sort algorithm.
- **TopologicalSort** — Sorts nodes in a directed acyclic graph (DAG) respecting dependencies.
- **TemplateSort** — Sorts items based on a specified order template.

---

## Critical Rules (Read First)

These rules are **non-negotiable**. Violating them causes broken builds, crashes, or downstream breakage.

### 1. Never Edit Generated Files

Files matching `*.generated.cs` are auto-generated.

- **NEVER** manually edit these files

### 2. ABI / API Stability

This project maintains stable ABI / API. Breaking changes break downstream apps.

| Allowed | Never |
|---------|-------|
| Add new overloads | Modify existing signatures |
| Add new methods | Remove public APIs |
| Add new classes | Change return types |

### 3. Tests Are Mandatory

**Building alone is NOT sufficient.** Run tests before claiming completion (see [Commands](#commands)).

### 4. Branch Protection (COMPLIANCE REQUIRED)

**Direct commits to protected branches are a policy violation.**

| Repository | Protected Branches |
|------------|-------------------|
| Orc.Sort | `master` |
| Orc.Sort | `develop` |

**Required workflow:**

1. **Create a feature branch FIRST** — Use naming convention: `feature/issue-NNNN-description`
2. **Make all commits on the feature branch** — Never commit directly to protected branches
3. **Submit a Pull Request** — Changes must be reviewed by a human before merging

```bash
# CORRECT — Always create a feature branch first
git checkout -b feature/issue-1234-fix-description

# NEVER DO THIS — Policy violation
git checkout develop && git commit  # FORBIDDEN

# NEVER DO THIS — Policy violation
git checkout master && git commit  # FORBIDDEN
```

The repository has protected branches that must be respected.

---

## Commands

Single source of truth for all commands:

| Task | Command |
|------|---------|
| **Build** | `dotnet cake --target=build` |
| **Test** | `dotnet cake --target=test` |
| **Build and test** | `dotnet cake --target=buildandtest` |

---

## Architecture & Directories

### Component Overview

```
Orc.Sort
├── NSort          — Classic sorting algorithm implementations
├── NSort.Generic  — Generic versions of NSort algorithms
├── TimSort        — TimSort algorithm implementation
├── TopologicalSort — Topological/dependency-aware sorting
├── TemplateSort   — Template-based sorting
├── Extensions     — IEnumerable extension methods for sorting
└── Interfaces     — Core interfaces (ISorter, ISwap)
```

### Directory Guide

| Directory | Editable? | Notes |
|-----------|-----------|-------|
| `*.generated.cs` | No | Leave as-is |
| `deployment` | No | Deployment / build scripts |
| `src/Orc.Sort` | Yes | Main library source code |
| `src/Orc.Sort.Tests` | Yes | Unit tests |

---

## Writing Code

### Anti-Patterns (Never Do This)

| Anti-Pattern | Why |
|-------------|-----|
| Modifying method signatures | ABI breaking |
| Manual edits to `*.generated.cs` | Overwritten on regenerate |
| Using default parameters in public APIs | ABI breaking |
| **Skipping failing tests** | **Unacceptable — tests must pass** |

---

## Testing & Debugging

### Running Tests

```bash
dotnet cake --target=test
```

### Tests MUST Pass

> **NON-NEGOTIABLE:** Tests must PASS before claiming completion.
>
> - Do NOT skip failing tests
> - Do NOT claim completion if tests fail
> - Do NOT use `SkipException` to work around failures

### Writing Tests

1. Use NUnit to write tests
2. Create a Facts or Tests class for a feature
3. Combine Pascal / Snake case for test methods (e.g. `TopologicalSort_SortsNodesRespectingDependencies`)

```csharp
[Test]
public void TopologicalSort_SortsNodesRespectingDependencies()
{
    var sort = new TopologicalSort<string>();
    sort.Add("B", "A"); // B depends on A
    sort.Add("C", "B"); // C depends on B

    var result = sort.Sort();

    Assert.That(result, Is.EqualTo(new[] { "A", "B", "C" }));
}
```

**Philosophy:** Tests FAIL when wrong, never skip (except missing hardware).

### Public API Tests

The project uses `PublicApiGenerator` to track the public API surface. After changing the public API, you must update the approved API snapshot:

```
src/Orc.Sort.Tests/PublicApiFacts.Orc_Sort_HasNoBreakingChanges_Async.verified.txt
```

Run the test to regenerate the snapshot if the public API has intentionally changed.

### Debugging Methodology

1. **Establish baseline** — What's the known-good state?
2. **One change at a time** — Verify each change before proceeding
3. **Track changes in a table** — Log what you changed and the result
4. **Platform differences are signals** — If X works and Y fails, the difference IS the answer
5. **Revert if worse** — Don't pile fixes on top of failures

---

## Further Reading

| Topic | Document |
|-------|----------|
| Contributing guidelines | [CONTRIBUTING.md](CONTRIBUTING.md) |
| License | [LICENSE](LICENSE) |
