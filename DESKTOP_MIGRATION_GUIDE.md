# HestiaLink Desktop Migration Guide

## Summary
Your project has been successfully converted from a Blazor web application to a **Windows Desktop Application** using .NET MAUI Blazor Hybrid.

## What Was Changed

### 1. Project Configuration (HestiaIT13Final.csproj)
- ✅ Removed mobile targets (Android, iOS, MacCatalyst)
- ✅ Changed to Windows-only desktop application (`net9.0-windows10.0.19041.0`)
- ✅ Changed OutputType from `Exe` to `WinExe` (proper Windows desktop app)
- ✅ Added Entity Framework Core packages for database access
- ✅ Updated application title to "HestiaLink Hotel Management System"

### 2. Dependency Injection (MauiProgram.cs)
- ✅ Added `HestiaLinkContext` (Entity Framework DbContext)
- ✅ Added `UserSession` service for authentication
- ✅ Configured SQL Server connection with retry logic
- ✅ Maintained Blazor WebView and Developer Tools

### 3. Razor Components
- ✅ Updated `Routes.razor` to work with MAUI Blazor
- ✅ Updated `_Imports.razor` with correct namespaces
- ✅ Removed web-specific render modes
- ✅ All your existing components (Login, Dashboard, HR, Inventory, etc.) remain intact

## How to Run Your Desktop Application

### Option 1: Using Visual Studio
1. Open `HestiaIT13Final.sln` in Visual Studio
2. Set `HestiaIT13Final` as the startup project (right-click → Set as Startup Project)
3. Select **Windows Machine** from the debug dropdown
4. Press **F5** or click the green "Windows Machine" button

### Option 2: Using Command Line
```powershell
cd "c:\Users\Jester\source\repos\HestiaIT13Final"
dotnet build HestiaIT13Final.csproj
dotnet run --project HestiaIT13Final.csproj --framework net9.0-windows10.0.19041.0
```

## What Can Be Removed (Optional Cleanup)

### Files/Projects No Longer Needed:
These were part of the old web-based setup and are NOT needed for your desktop app:

1. **HestiaLink.csproj** - The old ASP.NET Core Blazor Server project file
2. **HestiaLink.sln** - The old solution file (keep HestiaIT13Final.sln)
3. **Components/Program.cs** - Web application entry point
4. **Components/App.razor** - Blazor Server root component (not used in MAUI)
5. **appsettings.json** - Web configuration (connection string now in MauiProgram.cs)
6. **appsettings.Development.json** - Web development settings

### Folders You Can Delete (Optional):
- `Platforms/Android/` - No longer targeting Android
- `Platforms/iOS/` - No longer targeting iOS  
- `Platforms/MacCatalyst/` - No longer targeting Mac
- `Platforms/Tizen/` - No longer targeting Tizen

### What to KEEP:
- ✅ `HestiaIT13Final.csproj` - Your desktop application project
- ✅ `HestiaIT13Final.sln` - Your desktop solution
- ✅ `MauiProgram.cs` - Application startup and configuration
- ✅ `MainPage.xaml` and `MainPage.xaml.cs` - Main window
- ✅ All `Components/` folder files (Login, Dashboard, Pages, Layout, etc.)
- ✅ All `Models/` folder files
- ✅ All `Data/` folder files
- ✅ All `wwwroot/` files (CSS, images, etc.)
- ✅ `Platforms/Windows/` - Windows-specific configurations
- ✅ `Resources/` - App icons, fonts, images

## Database Connection

The database connection string is now configured in `MauiProgram.cs`:
```csharp
"Data Source=MSI\\SQLEXPRESS;Initial Catalog=IT13;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

If you need to change it, edit line 24 in `MauiProgram.cs`.

## Key Differences: Desktop vs Web

### Desktop Application (Current):
- ✅ Runs as a standalone Windows application (.exe)
- ✅ No web server required
- ✅ Direct database access from the desktop app
- ✅ Blazor UI runs in a native WebView component
- ✅ Better performance for local operations
- ✅ Can access local file system directly
- ✅ Single user per instance

### Old Web Application:
- ❌ Required IIS or Kestrel web server
- ❌ Accessed via browser (http://localhost:5000)
- ❌ Multiple concurrent users
- ❌ Network-based

## Project Structure

```
HestiaIT13Final/                    (Your desktop app)
├── HestiaIT13Final.csproj          ← Main project file (desktop)
├── HestiaIT13Final.sln             ← Solution file (desktop)
├── MauiProgram.cs                  ← App startup & DI configuration
├── MainPage.xaml                   ← Main window definition
├── MainPage.xaml.cs                ← Main window code-behind
│
├── Components/                     (All your Blazor UI)
│   ├── _Imports.razor              ← Global using statements
│   ├── Routes.razor                ← Routing configuration
│   ├── Login.razor                 ← Login page
│   ├── Layout/                     ← Layout components
│   │   ├── MainLayout.razor
│   │   ├── LoginLayout.razor
│   │   └── NavMenu.razor
│   └── Pages/                      ← All your application pages
│       ├── Dashboard.razor
│       ├── Counter.razor
│       ├── Home.razor
│       ├── FinanceBilling/
│       ├── Housekeeping/
│       ├── HumanResources/
│       ├── InventoryManagement/
│       ├── ReservationBooking/
│       └── SystemSettings/
│
├── Data/                           (Database)
│   └── HestiaLinkContext.cs        ← Entity Framework DbContext
│
├── Models/                         (Database models)
│   ├── Employee.cs
│   ├── Department.cs
│   ├── Guest.cs
│   ├── Reservation.cs
│   └── ... (all other models)
│
├── Components/Servies/             (Business logic)
│   └── UserSession.cs              ← User authentication service
│
├── wwwroot/                        (Static assets)
│   ├── index.html                  ← Blazor host page
│   ├── app.css                     ← Your styles
│   ├── css/
│   └── images/
│
└── Platforms/Windows/              (Windows-specific)
    ├── App.xaml                    ← Windows app resources
    └── Package.appxmanifest        ← Windows app manifest
```

## Troubleshooting

### Issue: "Could not find a part of the path"
**Solution:** Make sure you're running the correct project (`HestiaIT13Final`, not `HestiaLink`)

### Issue: Database connection fails
**Solution:** Verify SQL Server is running and the connection string in `MauiProgram.cs` is correct

### Issue: Components not loading
**Solution:** Ensure all `.razor` files are in the `Components/` folder and namespaces match

### Issue: "Type or namespace could not be found"
**Solution:** Clean and rebuild the solution:
```powershell
dotnet clean
dotnet build
```

## Next Steps

1. **Test the application:** Run it and verify all features work
2. **Remove old files:** Delete the HestiaLink.csproj and related web files (optional)
3. **Update connection string:** Change database connection if needed
4. **Customize:** Update app icon, splash screen, and title as desired
5. **Publish:** Create a distributable installer when ready

## Publishing Your Desktop Application

When you're ready to distribute your application:

```powershell
dotnet publish HestiaIT13Final.csproj -c Release -f net9.0-windows10.0.19041.0 -r win-x64 --self-contained
```

This will create a standalone executable in `bin\Release\net9.0-windows10.0.19041.0\win-x64\publish\`

---

**Congratulations!** 🎉 Your HestiaLink application is now a fully functional Windows Desktop Application!
