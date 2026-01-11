# Swagger Configuration Guide

## Access Swagger UI

After running the application, navigate to:
```
https://localhost:{port}/swagger
```

## Features Enabled

### 1. **Enhanced UI Features**
- ? Request duration display
- ? Filter/search endpoints
- ? Try It Out enabled by default
- ? Expanded models view
- ? Organized by controller tags

### 2. **API Documentation**
- API Title: **AgroProcessing API**
- Version: **v1**
- Description: Complete API for Agricultural Processing Management System

### 3. **Endpoint Organization**

Endpoints are grouped by functional areas:
- **Master Data - Products**
- **Master Data - Farmers**
- **Master Data - Workers**
- **Master Data - Buyers**
- **Master Data - Locations**
- **Purchases**
- **Purchase Payments**
- **Inventory**
- **Processing**
- **Sales**
- **Sales Payments**
- **Transportation**
- **Worker Payments**
- **Reports**

## How to Test Endpoints

### 1. Using Swagger UI (Recommended)

1. Navigate to `/swagger`
2. Expand any endpoint
3. Click **"Try it out"**
4. Fill in parameters/request body
5. Click **"Execute"**
6. View response

### 2. Using cURL

Export cURL commands directly from Swagger UI after testing.

### 3. Using Postman

1. In Swagger UI, find the OpenAPI spec URL: `/swagger/v1/swagger.json`
2. Import this URL into Postman
3. All endpoints will be automatically imported

## Example Requests

### Create Product
```json
POST /api/products
{
  "name": "Turmeric",
  "expectedYieldPercent": 85.5,
  "dryingDaysMin": 7,
  "dryingDaysMax": 10,
  "isActive": true
}
```

### Create Purchase Batch
```json
POST /api/purchases
{
  "farmerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "purchaseDate": "2024-01-15T10:30:00Z",
  "rawWeight": 1000.50,
  "ratePerKg": 50.00,
  "paymentSettlementType": "BeforeProcessing",
  "initialPayment": 25000.00,
  "paymentMode": "Cash"
}
```

### Create Processing Run
```json
POST /api/processing/runs
{
  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "startDate": "2024-01-15T08:00:00Z",
  "inputs": [
    {
      "purchaseBatchId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "inputWeight": 500.00
    }
  ]
}
```

### Create Sale
```json
POST /api/sales
{
  "buyerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "saleDate": "2024-01-15T10:00:00Z",
  "items": [
    {
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "quantity": 100.00,
      "rate": 120.00
    }
  ]
}
```

## Response Format

All endpoints return consistent API responses:

### Success Response
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": { /* actual response data */ },
  "errors": null
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error description",
  "data": null,
  "errors": [
    "Detailed error message 1",
    "Detailed error message 2"
  ]
}
```

## Status Codes

| Code | Meaning | When Used |
|------|---------|-----------|
| 200 | OK | Successful GET, PATCH operations |
| 201 | Created | Successful POST operation |
| 400 | Bad Request | Validation error, business rule violation |
| 404 | Not Found | Resource not found |
| 500 | Internal Server Error | Unexpected server error |

## Environment-Specific Behavior

### Development
- Swagger UI is **enabled**
- Detailed error messages
- CORS might be relaxed

### Production
- Swagger UI is **disabled** (for security)
- Generic error messages
- CORS restrictions enforced

## Customization

To customize Swagger further, edit `Program.cs`:

```csharp
builder.Services.AddSwaggerGen(c =>
{
    // Add custom configuration here
    c.SwaggerDoc("v1", new OpenApiInfo { ... });
});
```

## Security (Future Enhancement)

To add authentication to Swagger:

```csharp
c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    Description = "JWT Authorization header using the Bearer scheme",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
});

c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }
});
```

## Troubleshooting

### Swagger UI not loading
1. Ensure you're running in Development mode
2. Check the URL: `https://localhost:{port}/swagger`
3. Verify `app.UseSwagger()` and `app.UseSwaggerUI()` are called in Program.cs

### Endpoints not showing
1. Ensure controllers have `[ApiController]` attribute
2. Check route patterns: `[Route("api/[controller]")]`
3. Verify `builder.Services.AddEndpointsApiExplorer()` is registered

### Models not expanding
- Adjust `DefaultModelsExpandDepth` in SwaggerUI configuration
- Check for circular references (already handled with `ReferenceHandler.IgnoreCycles`)

## Best Practices

1. ? Always test endpoints in Swagger before deploying
2. ? Use "Try It Out" to validate request/response formats
3. ? Export Postman collection from Swagger spec for team sharing
4. ? Keep Swagger disabled in production for security
5. ? Document complex DTOs with XML comments
