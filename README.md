# FoodLens — Smart Recipe & Meal Planner

<p align="center">
  <strong>Discover recipes. Plan meals. Cook smarter.</strong>
</p>

FoodLens is a cross-platform mobile application built with **.NET 9 MAUI** that helps users discover recipes, manage meal plans, and leverage on-device hardware features like camera food recognition, text-to-speech narration, and shake-to-randomize.

---

## Table of Contents

- [Features](#features)
- [Screenshots](#screenshots)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Architecture](#architecture)
- [Pages](#pages)
- [Hardware Integration](#hardware-integration)
- [Accessibility](#accessibility)
- [Theming & Design](#theming--design)
- [Database](#database)
- [License](#license)

---

## Features

| Category | Feature | Description |
|----------|---------|-------------|
| **Recipes** | Browse & Search | Browse featured recipes or search by keyword, category, difficulty |
| | Create & Edit | Full recipe editor with ingredients, instructions, and image URL |
| | Favorites | Save favorite recipes for quick access |
| | Voice Input | Use speech-to-text for hands-free recipe creation |
| **Meal Planning** | Calendar View | Date-based meal planner with Breakfast/Lunch/Dinner/Snack slots |
| | Add Recipes | Browse and assign recipes to specific meal slots |
| **Camera** | Food Recognition | Take a photo and identify ingredients via Roboflow AI vision |
| **Audio** | Text-to-Speech | Listen to cooking steps read aloud step-by-step |
| | Speech-to-Text | Dictate recipe fields (title, description, ingredients, instructions) |
| **Motion** | Shake to Random | Shake device to discover a random recipe |
| **Location** | Geolocation | Get your current location and view it on an embedded map |
| | Reverse Geocoding | Display your street address using OpenStreetMap Nominatim |
| **UX** | Dark Mode | Full light/dark theme support with automatic system detection |
| | Responsive Layout | Orientation-aware adaptive grid — switches between single/double column on tablet rotation |
| | Pull-to-Refresh | Refresh content on all listing pages |
| **Auth** | Login / Register | Local user accounts with SHA-256 password hashing |

---

## Screenshots

> *Screenshots coming soon — the app features a warm orange (#FF6B35) and green (#4CAF50) design language with card-based layouts, gradient headers, and hero images.*

---

## Tech Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET MAUI | 9.0 | Cross-platform UI framework |
| CommunityToolkit.Mvvm | 8.4.2 | MVVM source generators (ObservableObject, RelayCommand) |
| sqlite-net-pcl | 1.9.172 | Local SQLite database |
| C# / XAML | 13 / MAUI | Primary languages |
| Roboflow API | — | Computer vision for food recognition (fallback) |
| ONNX Runtime | 1.21+ | On-device food classification (food101 model) |
| OpenStreetMap Nominatim | — | Reverse geocoding |
| Leaflet.js | — | Embedded map rendering |

---

## Project Structure

```
FoodLens/
├── Models/                         # Data models
│   ├── Recipe.cs                   # Recipe with ingredients, instructions, metadata
│   ├── Ingredient.cs               # Ingredient with name, quantity, unit
│   ├── Category.cs                 # Recipe category (Breakfast, Lunch, etc.)
│   ├── MealPlan.cs                 # Meal plan entry (date + meal type + recipe)
│   └── User.cs                     # User account (display name, email, password hash)
│
├── Views/                          # XAML pages (12 pages)
│   ├── SplashPage.xaml             # Splash screen with gradient animation
│   ├── LoginPage.xaml              # Login with gradient header
│   ├── RegisterPage.xaml           # Registration form
│   ├── HomePage.xaml               # Featured recipes, greeting, shake-to-random
│   ├── SearchPage.xaml             # Keyword search + category filter chips
│   ├── CategoryListPage.xaml       # Grid of category cards
│   ├── RecipeDetailPage.xaml       # Full recipe view with hero image
│   ├── RecipeEditPage.xaml         # Recipe creation/editing form
│   ├── FavoritesPage.xaml          # Saved favorite recipes
│   ├── MealPlanPage.xaml           # Calendar-based meal planner
│   ├── SettingsPage.xaml           # Account, appearance, location settings
│   └── AboutPage.xaml              # App info with feature grid
│
├── ViewModels/                     # MVVM ViewModels (11 ViewModels)
│   ├── BaseViewModel.cs            # Base class: IsBusy, Title, navigation helpers
│   ├── HomePageViewModel.cs        # Featured recipes, user greeting, shake
│   ├── SearchViewModel.cs          # Search + category filtering with debounce
│   ├── CategoryListViewModel.cs    # Category loading and navigation
│   ├── RecipeDetailViewModel.cs    # Recipe detail, TTS, camera scan
│   ├── RecipeEditViewModel.cs      # Create/edit recipe, voice input
│   ├── FavoritesViewModel.cs       # Favorite recipes management
│   ├── MealPlanViewModel.cs        # Meal plan CRUD operations
│   ├── SettingsViewModel.cs        # User settings, location, appearance
│   ├── LoginViewModel.cs           # Login authentication
│   └── RegisterViewModel.cs        # Registration validation
│
├── Services/                       # Service layer (7 interfaces + implementations)
│   ├── IDataService / DataService              # SQLite data access
│   ├── ILocationService / LocationService      # GPS, geocoding, Leaflet map
│   ├── ICameraService / CameraService          # Photo capture via MediaPicker
│   ├── IVisionService / VisionService          # Roboflow food recognition API
│   ├── ISpeechService / SpeechService          # Text-to-speech narration
│   ├── ISpeechRecognitionService / SpeechRecognitionService  # Speech-to-text
│   └── IAccelerometerService / AccelerometerService          # Shake detection
│
├── Controls/                       # Custom reusable controls
│   ├── RecipeCardView.xaml         # Recipe card with image, title, stats, favorite
│   ├── CategoryChipView.xaml       # Category card with icon and name
│   └── RatingStarView.xaml         # 5-star rating display
│
├── Helpers/                        # Utilities
│   ├── PlatformHelper.cs           # Orientation-aware layout detection (IsWideLayout + LayoutChanged event)
│   └── Converters/                 # XAML value converters
│       ├── StringNotEmptyConverter.cs
│       ├── StringEmptyConverter.cs
│       └── BoolToInverseConverter.cs
│
├── Resources/
│   ├── Styles/
│   │   ├── Colors.xaml             # Color palette (Primary, Secondary, etc.)
│   │   └── Styles.xaml             # Global styles (buttons, labels, frames)
│   ├── Images/                     # App images and icons (SVG)
│   ├── Fonts/                      # Custom fonts
│   ├── AppIcon/                    # App icon SVG assets
│   └── Splash/                     # Splash screen SVG
│
├── Platforms/                       # Platform-specific code
│   ├── Android/                    # Android: MainActivity, MainApplication
│   ├── Windows/                    # Windows: App.xaml.cs
│   ├── iOS/                        # iOS: AppDelegate, Program
│   └── MacCatalyst/                # MacCatalyst: AppDelegate, Program
│
├── Data/
│   └── recipes.json                # Seed data (reference only)
│
├── Resources/Raw/
│   ├── food101_model.onnx          # ONNX food classification model (NOT in repo — see "ONNX Local Model Setup")
│   └── food101_labels.txt          # 101 food class labels
│
├── App.xaml / App.xaml.cs           # Application entry, resource dictionary
├── AppShell.xaml / AppShell.xaml.cs # Shell navigation (tabs + flyout)
├── MauiProgram.cs                   # DI container configuration
└── FoodLens.csproj                  # Project configuration
```

---

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022 v17.8+](https://visualstudio.microsoft.com/) with MAUI workload
  - Or VS Code with the .NET MAUI extension

### Platform Requirements

| Platform | Minimum Version |
|----------|----------------|
| Android | API 21 (Android 5.0 Lollipop) |
| Windows | 10.0.17763 (October 2018 Update) |
| iOS | 15.0 |
| macOS (Mac Catalyst) | 15.0 |

### Build & Run

```bash
# Clone the repository
git clone <repository-url>
cd FoodLens

# Restore dependencies
dotnet restore

# Build for Windows
dotnet build -f net9.0-windows10.0.19041.0

# Build for Android
dotnet build -f net9.0-android

# Run on Windows
dotnet run -f net9.0-windows10.0.19041.0

# Run on Android (connected device or emulator)
dotnet run -f net9.0-android
```

Or open `FoodLens.sln` in Visual Studio, select your target platform, and press **F5**.

---

## Architecture

FoodLens follows the **MVVM (Model-View-ViewModel)** pattern:

```
┌─────────────┐     Data Binding     ┌──────────────────┐     async/await     ┌──────────────┐
│   Views      │ ◄══════════════════► │   ViewModels      │ ═════════════════► │   Services    │
│   (XAML)     │     Commands         │   (C# Observable)  │                    │   (C# async)  │
└─────────────┘                       └──────────────────┘                     └──────────────┘
                                                                                      │
                                                                              ┌───────┴───────┐
                                                                              │    Models      │
                                                                              │   (SQLite)     │
                                                                              └───────────────┘
```

### Key Patterns

- **CommunityToolkit.Mvvm** — Source-generated `[ObservableProperty]` and `[RelayCommand]`
- **Dependency Injection** — All services and ViewModels registered in `MauiProgram.cs`
- **Shell Navigation** — URI-based navigation with query parameters
- **Bindable Properties** — Custom controls use `BindableProperty` for XAML data binding

---

## Pages

| Page | Route | Description |
|------|-------|-------------|
| **SplashPage** | — | Gradient splash with loading indicator |
| **LoginPage** | `login` | Email/password login with gradient header |
| **RegisterPage** | `register` | Account creation with 4-field form |
| **HomePage** | `home` | Featured recipe grid, user greeting, shake hint |
| **SearchPage** | `search` | Search bar + category filter chips + results grid |
| **CategoryListPage** | `categories` | 2-column grid of category cards |
| **RecipeDetailPage** | `recipedetail` | Hero image, stats, ingredients, instructions, actions |
| **RecipeEditPage** | `recipeedit` | Full recipe editor with voice input on each field |
| **FavoritesPage** | `favorites` | Saved recipes grid with empty state |
| **MealPlanPage** | `mealplan` | Date picker + meal plan cards + add-recipe modal |
| **SettingsPage** | `settings` | Account, dark mode, font size, location with map |
| **AboutPage** | `about` | App info, feature grid, description |

### Navigation Structure

```
AppShell
├── TabBar (bottom tabs)
│   ├── Home        (home icon)
│   ├── Search      (search icon)
│   ├── Favorites   (heart icon)
│   └── Meal Plan   (calendar icon)
└── FlyoutMenu (hamburger)
    ├── Home
    ├── Categories
    ├── Settings
    ├── Login / Logout
    └── About
```

---

## Hardware Integration

| # | Feature | Service | API Used |
|---|---------|---------|----------|
| 1 | **Camera** | `ICameraService` | `MediaPicker.CapturePhotoAsync()` |
| 2 | **Food Recognition** | `IVisionService` | Local ONNX model (food101) — runs entirely on-device |
| 3 | **Geolocation** | `ILocationService` | `Geolocation.GetLocationAsync()` |
| 4 | **Reverse Geocoding** | `ILocationService` | OpenStreetMap Nominatim API + `Geocoding` |
| 5 | **Map Display** | `ILocationService` | Leaflet.js in `WebView` |
| 6 | **Text-to-Speech** | `ISpeechService` | `TextToSpeech.SpeakAsync()` |
| 7 | **Speech-to-Text** | `ISpeechRecognitionService` | Android `RecognizerIntent` |
| 8 | **Accelerometer** | `IAccelerometerService` | `Accelerometer.Start()` + shake detection |
| 9 | **Haptic Feedback** | ViewModels | `HapticFeedback.Perform()` |
| 10 | **Vibration** | ViewModels | `Vibration.Vibrate()` |

---

## Accessibility

FoodLens targets **WCAG 2.0 AA** compliance:

- `AutomationId` on all interactive controls for UI testing
- `SemanticProperties.Hint` and `.Description` on buttons, entries, and pickers
- Minimum **44x48dp** touch targets on all tappable elements
- High color contrast ratios (>= 4.5:1 for text)
- Full **dark mode** support via `AppThemeBinding`
- **Scalable font sizes** with a settings control and `DynamicResource`
- Logical tab/reading order throughout the app

---

## Theming & Design

### Color Palette

| Role | Color | Hex |
|------|-------|-----|
| Primary (Orange) | ![](https://via.placeholder.com/15/FF6B35/FF6B35) | `#FF6B35` |
| Secondary (Green) | ![](https://via.placeholder.com/15/4CAF50/4CAF50) | `#4CAF50` |
| Light Background | ![](https://via.placeholder.com/15/FFF8F0/FFF8F0) | `#FFF8F0` |
| Dark Background | ![](https://via.placeholder.com/15/1A1A2E/1A1A2E) | `#1A1A2E` |
| Star Filled | ![](https://via.placeholder.com/15/FFD700/FFD700) | `#FFD700` |
| Danger | ![](https://via.placeholder.com/15/EF5350/EF5350) | `#EF5350` |

### Design System

- **Corner radius**: 12px (cards, buttons), 16px (images), 20px (pills)
- **Spacing scale**: 8 / 12 / 16 / 24px
- **Font sizes**: 12 / 14 / 16 / 20 / 24 / 32 / 40px
- **Card style**: Rounded frames with shadow, gradient placeholders for missing images
- **Responsive**: Orientation-aware layout — single column in portrait, 2-column grid in landscape on tablets; always 2-column on Windows desktop (`PlatformHelper.IsWideLayout` + `DeviceDisplay.MainDisplayInfoChanged`)

---

## ONNX Local Model Setup

FoodLens supports **on-device food recognition** using a local ONNX model (food101). Due to GitHub's **100 MB file size limit**, the model file (`food101_model.onnx`, ~328 MB) is **not included** in this repository.

### Download the Model

1. Obtain the `food101_model.onnx` file (Food-101 classification model, input: 224x224 RGB, output: 101-class logits)
2. Place it in the project at the following location:

```
FoodLens/Resources/Raw/food101_model.onnx
```

### How It Works

At runtime, `VisionService` loads the model in this order:

1. Checks `FileSystem.AppDataDirectory` for a previously extracted copy of the model
2. If not found, extracts from the app bundle (`Resources/Raw/food101_model.onnx`) to app data directory
3. Creates an `InferenceSession` using `Microsoft.ML.OnnxRuntime`

### Build with the Model

After placing the model file, build normally:

```bash
dotnet build
```

The file will be included as a `MauiAsset` and packaged into the app automatically.

> **Note:** If the model file is missing, the camera food recognition feature will be unavailable. All other features (browsing, search, meal planning, etc.) work without the model.

---

## Database

FoodLens uses **SQLite** via `sqlite-net-pcl` for local data persistence. The database file is stored at:

| Platform | Path |
|----------|------|
| Windows | `%LOCALAPPDATA%\Packages\...\LocalState\foodlens.db3` |
| Android | `/data/data/com.companyname.foodlens/files/foodlens.db3` |
| iOS | `Library/foodlens.db3` |

### Tables

| Table | Columns |
|-------|---------|
| `Recipe` | Id, Title, Description, ImageUrl, Category, CookTimeMinutes, PrepTimeMinutes, Servings, Difficulty, Rating, Calories, IsFavorite, CreatedAt, InstructionsJson, IngredientsJson |
| `MealPlan` | Id, Date, MealType, RecipeId |
| `User` | Id, DisplayName, Email, PasswordHash |
| `Ingredient` | Id, Name, Quantity, Unit |

### Seed Data

The app ships with **6 pre-loaded recipes** across 5 categories:

1. Classic Pasta Carbonara (Dinner)
2. Avocado Toast with Poached Egg (Breakfast)
3. Chicken Stir Fry (Lunch)
4. Chocolate Lava Cake (Dessert)
5. Fresh Berry Smoothie (Drinks)
6. Grilled Salmon with Lemon (Dinner)

---

## License

This project is for educational purposes.
