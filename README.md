# FoodLens - Smart Recipe &amp; Meal Planner

A .NET MAUI mobile application for discovering recipes, planning meals, and leveraging mobile hardware features.

## Features

- **Recipe Browsing & Search** - Browse and search recipes by keyword, category, difficulty, and cooking time
- **Camera Food Recognition** - Take photos to identify ingredients using computer vision
- **Meal Plan Calendar** - Plan your meals with a calendar-based interface
- **Nearby Restaurants** - Find restaurants near your location using geolocation
- **Text-to-Speech** - Listen to recipe cooking steps read aloud
- **Favorites** - Save and manage your favorite recipes
- **Dark Mode** - Full dark mode support with WCAG 2.0 accessibility

## Tech Stack

- .NET 9.0 MAUI
- CommunityToolkit.Mvvm (MVVM architecture)
- SQLite-net-pcl (local data persistence)
- C# / XAML

## Mobile Hardware Features

1. Camera + Computer Vision (food recognition)
2. Geolocation + Geocoding (nearby restaurants)
3. Text-to-Speech (recipe step narration)
4. Haptic Feedback + Vibration (interaction feedback)
5. Accelerometer (shake to get random recipe)
6. Compass (restaurant direction)

## Accessibility (WCAG 2.0)

- Semantic properties on all interactive controls
- High color contrast (>= 4.5:1)
- Dark mode support via AppThemeBinding
- Scalable font sizes
- Minimum 48x48dp touch targets
- Logical navigation order

## Requirements

- .NET 9.0 SDK
- Visual Studio 2022 17.8+
- Android API 21+ / Windows 10 1809+

## How to Run

1. Clone the repository
2. Open `FoodLens.sln` in Visual Studio
3. Select target platform (Android / Windows)
4. Build and run

## Project Structure

```
FoodLens/
├── Models/                    # Data models (Recipe, Category, Ingredient, MealPlan, User)
├── Views/                     # XAML pages
│   ├── SplashPage.xaml
│   ├── HomePage.xaml
│   ├── CategoryListPage.xaml
│   ├── RecipeDetailPage.xaml
│   ├── SearchPage.xaml
│   ├── FavoritesPage.xaml
│   ├── MealPlanPage.xaml
│   ├── SettingsPage.xaml
│   ├── LoginPage.xaml
│   ├── RegisterPage.xaml
│   └── AboutPage.xaml
├── ViewModels/                # MVVM ViewModels
├── Services/                  # Service layer (data, camera, location, speech, vision)
├── Helpers/                   # Validators, Converters, Behaviors
├── Controls/                  # Custom reusable controls
├── Data/                      # Seed data (recipes.json)
├── Resources/                 # Styles, Fonts, Images
├── App.xaml / AppShell.xaml
├── MauiProgram.cs             # DI configuration
└── README.md
```
