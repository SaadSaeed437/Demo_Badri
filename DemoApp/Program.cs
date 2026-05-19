// ============================================================
//  DemoApp — Minimal .NET Web API
//  Purpose : Demo application to show GitHub Actions workflow
//  Endpoints:
//    GET /         → Welcome page with app info
//    GET /version  → Returns current app version
//    GET /health   → Health check (pipelines use this)
// ============================================================

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Read version from version.txt (written by the pipeline on release)
// If file doesn't exist yet, fall back to "dev"
string version = "dev";
if (File.Exists("version.txt"))
    version = File.ReadAllText("version.txt").Trim();

// ── ENDPOINTS ───────────────────────────────────────────────

// Home — shows app info in browser
// Note: $$ means only {{placeholders}} are interpolated.
//       Single { } in CSS are treated as plain text — no conflicts.
app.MapGet("/", () => Results.Content($$"""
    <!DOCTYPE html>
    <html>
    <head>
        <title>DemoApp</title>
        <style>
            body     { font-family: Arial, sans-serif; max-width: 600px;
                       margin: 60px auto; padding: 0 20px; background: #f8fafc; }
            .card    { background: #fff; border: 1px solid #e2e8f0;
                       border-radius: 12px; padding: 32px; }
            h1       { color: #1e3a5f; margin-bottom: 4px; }
            .version { display: inline-block; background: #dbeafe;
                       color: #1d4ed8; padding: 4px 12px; border-radius: 20px;
                       font-family: monospace; font-size: .9rem; margin-bottom: 20px; }
            .endpoint { background: #f1f5f9; border-radius: 6px;
                        padding: 8px 14px; margin: 8px 0;
                        font-family: monospace; font-size: .85rem; }
            .label   { color: #64748b; font-size: .8rem; margin-top: 18px;
                       text-transform: uppercase; letter-spacing: .08em; }
        </style>
    </head>
    <body>
        <div class="card">
            <h1>DemoApp</h1>
            <div class="version">{{version}}</div>
            <p style="color:#475569">A minimal .NET app used to demonstrate
               GitHub Actions CI/CD pipeline.</p>
            <div class="label">Available Endpoints</div>
            <div class="endpoint">GET /version &rarr; current version</div>
            <div class="endpoint">GET /health  &rarr; health check</div>
            <div class="label">Pipeline triggers on</div>
            <div class="endpoint">push &rarr; main branch only</div>
        </div>
    </body>
    </html>
    """, "text/html"));

// Version endpoint — returns plain version string
// The pipeline stamps this automatically on release
app.MapGet("/version", () => new
{
    version,
    app     = "DemoApp",
    built   = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm") + " UTC"
});

// Health check endpoint — pipelines ping this after deploy
// Returns 200 OK if app is running, used to verify deployment succeeded
app.MapGet("/health", () => new
{
    status  = "healthy",
    version,
    time    = DateTime.UtcNow
});

app.Run();
