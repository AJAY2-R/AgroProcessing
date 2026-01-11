# ?? Tailwind CSS Setup for AgroProcessing

## ? What Was Added

### 1. **Configuration Files**
- ? `package.json` - Node.js dependencies
- ? `tailwind.config.js` - Tailwind configuration
- ? Updated `wwwroot/css/site.css` - Tailwind directives

### 2. **Updated Files**
- ? `Views/Shared/_Layout.cshtml` - Modern Tailwind layout

### 3. **Custom Theme**
- ?? **Primary Colors** - Green theme for agriculture
- ?? **Secondary Colors** - Yellow/Gold accents
- ?? **Responsive Design** - Mobile-first approach
- ?? **Custom Components** - Reusable Tailwind components

---

## ?? Setup Instructions

### Step 1: Install Node.js
If you don't have Node.js installed:
1. Download from https://nodejs.org/
2. Install LTS version

### Step 2: Install Dependencies
```bash
cd AgroProcessing
npm install
```

### Step 3: Build Tailwind CSS
```bash
# One-time build
npm run build:css

# Or watch for changes during development
npm run watch:css
```

### Step 4: Run Your Application
```bash
dotnet run
```

---

## ?? Usage Examples

### Basic Components

#### Buttons
```html
<!-- Primary Button -->
<button class="btn-primary-custom">
    Save Changes
</button>

<!-- Secondary Button -->
<button class="btn-secondary-custom">
    Cancel
</button>

<!-- Custom Tailwind Button -->
<button class="bg-primary-600 hover:bg-primary-700 text-white font-semibold py-2 px-4 rounded-lg">
    Click Me
</button>
```

#### Cards
```html
<!-- Custom Card Component -->
<div class="card-custom">
    <h3 class="text-xl font-semibold text-gray-900 mb-2">Card Title</h3>
    <p class="text-gray-600">Card content goes here...</p>
</div>

<!-- Standard Tailwind Card -->
<div class="bg-white rounded-lg shadow-md p-6">
    <h3 class="text-xl font-semibold mb-4">Product Details</h3>
    <p class="text-gray-600">Description...</p>
</div>
```

#### Forms
```html
<!-- Input Field -->
<input type="text" 
       class="input-custom" 
       placeholder="Enter product name">

<!-- Select Dropdown -->
<select class="input-custom">
    <option>Select option</option>
    <option>Option 1</option>
</select>

<!-- Textarea -->
<textarea class="input-custom" 
          rows="4" 
          placeholder="Enter description"></textarea>
```

#### Grid Layouts
```html
<!-- 3-Column Grid -->
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
    <div class="card-custom">Column 1</div>
    <div class="card-custom">Column 2</div>
    <div class="card-custom">Column 3</div>
</div>
```

---

## ?? Color Palette

### Primary (Green - Agriculture Theme)
```css
primary-50:  #f0fdf4  /* Very light green */
primary-100: #dcfce7
primary-200: #bbf7d0
primary-300: #86efac
primary-400: #4ade80
primary-500: #22c55e  /* Main green */
primary-600: #16a34a  /* Darker green */
primary-700: #15803d
primary-800: #166534
primary-900: #14532d
```

### Secondary (Yellow - Harvest Theme)
```css
secondary-50:  #fefce8
secondary-100: #fef9c3
secondary-200: #fef08a
secondary-300: #fde047
secondary-400: #facc15
secondary-500: #eab308  /* Main yellow */
secondary-600: #ca8a04
secondary-700: #a16207
secondary-800: #854d0e
secondary-900: #713f12
```

### Usage
```html
<div class="bg-primary-500 text-white">Primary background</div>
<div class="text-primary-600">Primary text</div>
<button class="bg-secondary-500 hover:bg-secondary-600">Secondary button</button>
```

---

## ?? Pre-built Components

### Dashboard Card
```html
<div class="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
    <div class="flex items-center justify-between mb-4">
        <h3 class="text-lg font-semibold text-gray-900">Total Sales</h3>
        <span class="text-2xl">??</span>
    </div>
    <p class="text-3xl font-bold text-primary-600">$125,430</p>
    <p class="text-sm text-gray-500 mt-2">+12.5% from last month</p>
</div>
```

### Form Group
```html
<div class="mb-4">
    <label class="block text-sm font-medium text-gray-700 mb-2">
        Product Name
    </label>
    <input type="text" 
           class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
           placeholder="Enter product name">
    <p class="mt-1 text-sm text-gray-500">This will be displayed to customers</p>
</div>
```

### Alert Messages
```html
<!-- Success Alert -->
<div class="bg-green-50 border border-green-200 text-green-800 px-4 py-3 rounded-lg">
    <strong class="font-semibold">Success!</strong> Your changes have been saved.
</div>

<!-- Error Alert -->
<div class="bg-red-50 border border-red-200 text-red-800 px-4 py-3 rounded-lg">
    <strong class="font-semibold">Error!</strong> Something went wrong.
</div>

<!-- Info Alert -->
<div class="bg-blue-50 border border-blue-200 text-blue-800 px-4 py-3 rounded-lg">
    <strong class="font-semibold">Info!</strong> Please review the information.
</div>
```

### Table
```html
<div class="overflow-x-auto">
    <table class="min-w-full divide-y divide-gray-200">
        <thead class="bg-gray-50">
            <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Product
                </th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Quantity
                </th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Status
                </th>
            </tr>
        </thead>
        <tbody class="bg-white divide-y divide-gray-200">
            <tr class="hover:bg-gray-50">
                <td class="px-6 py-4 whitespace-nowrap">Turmeric</td>
                <td class="px-6 py-4 whitespace-nowrap">1000 kg</td>
                <td class="px-6 py-4 whitespace-nowrap">
                    <span class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-green-100 text-green-800">
                        Active
                    </span>
                </td>
            </tr>
        </tbody>
    </table>
</div>
```

---

## ?? Development Workflow

### During Development
```bash
# Terminal 1: Watch Tailwind CSS (auto-rebuild on changes)
npm run watch:css

# Terminal 2: Run .NET application
dotnet watch run
```

### Before Deployment
```bash
# Build minified CSS for production
npm run build:css

# Verify build
dotnet build
```

---

## ?? Responsive Design

Tailwind uses mobile-first breakpoints:

```html
<!-- Responsive Grid Example -->
<div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
    <!-- Automatically adjusts based on screen size -->
</div>

<!-- Responsive Padding -->
<div class="px-4 sm:px-6 lg:px-8">
    <!-- More padding on larger screens -->
</div>

<!-- Responsive Text -->
<h1 class="text-2xl sm:text-3xl lg:text-4xl font-bold">
    Responsive Heading
</h1>
```

### Breakpoints
- `sm:` - 640px and up (small tablets)
- `md:` - 768px and up (tablets)
- `lg:` - 1024px and up (laptops)
- `xl:` - 1280px and up (desktops)
- `2xl:` - 1536px and up (large desktops)

---

## ?? Integration with Existing Project

### Option 1: Full Tailwind (Recommended)
Remove Bootstrap and use only Tailwind:
```html
<!-- In _Layout.cshtml, comment out Bootstrap -->
<!-- <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" /> -->
```

### Option 2: Hybrid Approach
Keep Bootstrap for specific components:
```html
<!-- Use both, but Tailwind takes precedence -->
<link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
<link rel="stylesheet" href="~/css/output.css" asp-append-version="true" />
```

---

## ?? Plugins Included

### @tailwindcss/forms
Better form styling out of the box:
```html
<input type="text" class="rounded-md border-gray-300 focus:border-primary-500 focus:ring-primary-500">
```

### @tailwindcss/typography
Beautiful typography for content:
```html
<article class="prose lg:prose-xl">
    <h1>Heading</h1>
    <p>Automatically styled content...</p>
</article>
```

---

## ? Checklist

Before going to production:

- [ ] Run `npm run build:css` for minified CSS
- [ ] Test responsive design on mobile devices
- [ ] Verify all pages use new Tailwind classes
- [ ] Remove unused Bootstrap references (optional)
- [ ] Add `wwwroot/css/output.css` to git
- [ ] Add `node_modules/` to `.gitignore`

---

## ?? Resources

- **Tailwind Docs**: https://tailwindcss.com/docs
- **Component Examples**: https://tailwindui.com/components
- **Color Palette**: https://tailwindcss.com/docs/customizing-colors
- **Cheat Sheet**: https://nerdcave.com/tailwind-cheat-sheet

---

## ?? You're Ready!

Your AgroProcessing project now has:
- ? Modern Tailwind CSS setup
- ? Custom agricultural theme (green/yellow)
- ? Responsive mobile-first design
- ? Pre-built component classes
- ? Professional navigation and layout

**Next Steps:**
1. Run `npm install`
2. Run `npm run watch:css`
3. Start building beautiful pages! ??
