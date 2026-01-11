# ?? Tailwind CSS - Quick Start

## ? Setup Complete!

Tailwind CSS has been successfully added to your AgroProcessing project.

---

## ?? Get Started in 3 Steps

### 1. Install Dependencies
```bash
cd AgroProcessing
npm install
```

### 2. Build Tailwind CSS
```bash
# One-time build
npm run build:css

# OR watch for changes (recommended during development)
npm run watch:css
```

### 3. Run Your Application
```bash
dotnet run
```

Open: `https://localhost:{PORT}/Home/Dashboard` to see the example page!

---

## ?? Files Added/Modified

### ? New Files
- `package.json` - Node.js dependencies
- `tailwind.config.js` - Tailwind configuration
- `TAILWIND_SETUP.md` - Complete documentation
- `Views/Home/Dashboard.cshtml` - Example Tailwind page
- `.gitignore` - Excludes node_modules

### ? Modified Files
- `wwwroot/css/site.css` - Added Tailwind directives
- `Views/Shared/_Layout.cshtml` - Modern Tailwind layout

---

## ?? Features

### Custom Theme
- ?? **Green Primary** - Agriculture theme
- ?? **Yellow Secondary** - Harvest accents
- ?? **Responsive** - Mobile-first design
- ?? **Custom Components** - Pre-built classes

### Included Plugins
- ? `@tailwindcss/forms` - Better form styling
- ? `@tailwindcss/typography` - Beautiful content typography

---

## ?? Quick Examples

### Button
```html
<button class="bg-primary-600 hover:bg-primary-700 text-white font-semibold py-2 px-4 rounded-lg">
    Click Me
</button>
```

### Card
```html
<div class="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
    <h3 class="text-xl font-semibold mb-2">Card Title</h3>
    <p class="text-gray-600">Card content...</p>
</div>
```

### Form Input
```html
<input type="text" 
       class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500" 
       placeholder="Enter text">
```

### Grid Layout
```html
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
    <div>Column 1</div>
    <div>Column 2</div>
    <div>Column 3</div>
</div>
```

---

## ?? Responsive Classes

```html
<!-- Mobile: 1 column, Tablet: 2 columns, Desktop: 4 columns -->
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
    <!-- Content -->
</div>

<!-- Hide on mobile, show on desktop -->
<div class="hidden lg:block">
    Desktop only content
</div>

<!-- Different padding on different screen sizes -->
<div class="px-4 md:px-6 lg:px-8">
    Responsive padding
</div>
```

---

## ?? Pre-built Custom Classes

These are defined in `site.css` for reuse:

```html
<!-- Custom Button -->
<button class="btn-primary-custom">Primary Button</button>
<button class="btn-secondary-custom">Secondary Button</button>

<!-- Custom Card -->
<div class="card-custom">
    Card with hover effect
</div>

<!-- Custom Input -->
<input type="text" class="input-custom" placeholder="Styled input">
```

---

## ?? Development Workflow

### Terminal 1: Watch Tailwind
```bash
npm run watch:css
```
This automatically rebuilds CSS when you change files.

### Terminal 2: Run .NET App
```bash
dotnet watch run
```
This auto-restarts when you change code.

### Result
Any Tailwind class changes are immediately reflected in your browser!

---

## ?? Example Dashboard

We created a sample dashboard at:
```
Views/Home/Dashboard.cshtml
```

Features:
- ? Stats cards with icons
- ? Recent purchases list
- ? Quick action buttons
- ? Inventory table
- ? Fully responsive
- ? Hover effects

---

## ?? Color Reference

### Primary (Green)
```html
<div class="bg-primary-500">Main green</div>
<div class="bg-primary-600">Darker green</div>
<div class="text-primary-700">Green text</div>
```

### Secondary (Yellow)
```html
<div class="bg-secondary-500">Main yellow</div>
<div class="bg-secondary-600">Darker yellow</div>
<div class="text-secondary-700">Yellow text</div>
```

### Utility Colors
```html
<div class="bg-green-100">Light green background</div>
<div class="text-red-600">Red text</div>
<div class="border-blue-500">Blue border</div>
```

---

## ?? Resources

### Official Docs
- **Tailwind CSS**: https://tailwindcss.com/docs
- **Components**: https://tailwindui.com/components
- **Cheat Sheet**: https://nerdcave.com/tailwind-cheat-sheet

### Your Project Docs
- `TAILWIND_SETUP.md` - Complete setup guide
- `Views/Home/Dashboard.cshtml` - Example page
- `tailwind.config.js` - Configuration

---

## ? Next Steps

1. **Install dependencies**: `npm install`
2. **Build CSS**: `npm run watch:css`
3. **Run app**: `dotnet run`
4. **Browse**: `/Home/Dashboard`
5. **Start building!** ??

---

## ?? Troubleshooting

### CSS Not Loading?
```bash
# Make sure you ran:
npm install
npm run build:css
```

### Classes Not Working?
Check `tailwind.config.js` content paths:
```javascript
content: [
    './Views/**/*.cshtml',
    './Pages/**/*.cshtml',
    './wwwroot/js/**/*.js'
]
```

### Node.js Not Found?
Download from: https://nodejs.org/

---

## ?? You're Ready!

Your AgroProcessing project now has:
- ? Modern Tailwind CSS
- ? Custom agricultural theme
- ? Responsive design
- ? Example dashboard
- ? Pre-built components

**Start Building Beautiful UIs! ??**
