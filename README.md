# DemoApp — GitHub Actions Demo

A minimal .NET 8 Web API built to demonstrate how a GitHub Actions
CI/CD pipeline works when code is merged to the **main** branch.

---

## What This App Does

| Endpoint     | Returns                          |
|--------------|----------------------------------|
| `GET /`      | Simple HTML page with app info   |
| `GET /version` | Current version from version.txt |
| `GET /health`  | Health check — used by pipelines |

---

## Run Locally

```bash
cd DemoApp
dotnet run
```

Then open: http://localhost:5000

---

## Pipeline — How It Works

The workflow file is at: `.github/workflows/main-pipeline.yml`

### Trigger
The pipeline runs **only** when code is pushed to `main`.

```
Push to ABC-123      → nothing happens
Push to development  → nothing happens
Merge to main        → PIPELINE RUNS
```

### What the Pipeline Does

```
Job 1: Build & Test
  ├── Checkout code
  ├── Setup .NET 8
  ├── Restore packages
  ├── Build (Release)
  └── Run tests

Job 2: Release  (only if "release: vX.Y.Z" in commit message)
  ├── Detect version from commit message
  ├── Write version to version.txt  ← app displays this
  ├── Auto-generate CHANGELOG.md
  ├── Commit & push back to main
  ├── Create Git tag  e.g. v1.2.0
  ├── Create GitHub Release
  └── Publish app to ./publish
```

### How to Trigger a Release

When merging development → main, include this in the commit message:

```
release: v1.2.0
```

The pipeline detects it, stamps the version, updates the changelog,
creates a tag, and creates a GitHub Release automatically.

### Commit Message Convention

| Prefix    | Changelog Section |
|-----------|-------------------|
| `feat:`   | ### Added         |
| `fix:`    | ### Fixed         |
| `change:` | ### Changed       |
| `chore:`  | ### Maintenance   |

---

## Folder Structure

```
DemoApp/
├── .github/
│   └── workflows/
│       └── main-pipeline.yml   ← the pipeline
├── DemoApp/
│   ├── Program.cs              ← the app
│   ├── DemoApp.csproj
│   ├── version.txt             ← pipeline writes here
│   └── Properties/
│       └── launchSettings.json
├── CHANGELOG.md                ← pipeline updates here
└── README.md
```
