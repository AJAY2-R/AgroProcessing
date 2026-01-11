# Swagger UI Quick Reference

## ?? Accessing Swagger

**URL:** `https://localhost:{YOUR_PORT}/swagger`

Example: `https://localhost:7244/swagger`

---

## ?? What You'll See

### Main Interface

```
???????????????????????????????????????????????????????????????????
?  AgroProcessing API v1                                          ?
?  Complete API for Agricultural Processing Management System    ?
?                                                                 ?
?  [Explore] [Schemas ?]                           [Filter: ___] ?
???????????????????????????????????????????????????????????????????
?                                                                 ?
?  ? Master Data - Buyers                                        ?
?     GET   /api/buyers                Get all buyers            ?
?     GET   /api/buyers/{id}           Get buyer by ID           ?
?     POST  /api/buyers                Create new buyer          ?
?                                                                 ?
?  ? Master Data - Farmers                                       ?
?     GET   /api/farmers               Get all farmers           ?
?     GET   /api/farmers/{id}          Get farmer by ID          ?
?     POST  /api/farmers               Create new farmer         ?
?                                                                 ?
?  ? Master Data - Products                                      ?
?     GET   /api/products              Get all active products   ?
?     GET   /api/products/{id}         Get product by ID         ?
?     POST  /api/products              Create new product        ?
?     PATCH /api/products/{id}/toggle  Toggle product status     ?
?                                                                 ?
?  ? Purchases                                                   ?
?     POST  /api/purchases             Create purchase batch     ?
?     GET   /api/purchases/{id}        Get purchase by ID        ?
?     GET   /api/purchases/pending     Get pending purchases     ?
?                                                                 ?
?  ? Processing                                                  ?
?     POST  /api/processing/runs       Create processing run     ?
?     GET   /api/processing/runs/{id}  Get processing run        ?
?     POST  /api/processing/runs/complete Complete run           ?
?                                                                 ?
?  ? Sales                                                       ?
?     POST  /api/sales                 Create new sale           ?
?     GET   /api/sales/{id}            Get sale by ID            ?
?     GET   /api/sales/open            Get open sales            ?
?                                                                 ?
?  ? Reports                                                     ?
?     GET   /api/reports/batch-profit/{batchId}                 ?
?     GET   /api/reports/yield-analysis/{productId}             ?
?     GET   /api/reports/outstanding-payments                   ?
?                                                                 ?
???????????????????????????????????????????????????????????????????
```

---

## ?? Expanded Endpoint View

When you click on an endpoint (e.g., `POST /api/purchases`):

```
???????????????????????????????????????????????????????????????????
?  POST /api/purchases                                            ?
?  Create new purchase batch                                      ?
?                                                                 ?
?  [Try it out]                                                  ?
???????????????????????????????????????????????????????????????????
?  Request body                                            *required?
?                                                                 ?
?  {                                                              ?
?    "farmerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",       ?
?    "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",      ?
?    "purchaseDate": "2024-01-15T10:30:00Z",                    ?
?    "rawWeight": 1000.50,                                       ?
?    "ratePerKg": 50.00,                                         ?
?    "paymentSettlementType": "BeforeProcessing",               ?
?    "initialPayment": 25000.00,                                 ?
?    "paymentMode": "Cash"                                       ?
?  }                                                              ?
?                                                                 ?
?  [Execute]  [Clear]                                            ?
???????????????????????????????????????????????????????????????????
?  Responses                                                      ?
?                                                                 ?
?  Code  Description                                             ?
?  201   Buyer created successfully                              ?
?  400   Invalid input                                            ?
?  500   Internal server error                                   ?
???????????????????????????????????????????????????????????????????
```

---

## ?? After Clicking "Execute"

```
???????????????????????????????????????????????????????????????????
?  Server response                                                ?
?                                                                 ?
?  Code: 201                                                      ?
?  Duration: 245 ms                                               ?
?                                                                 ?
?  Response body                                                  ?
?  {                                                              ?
?    "success": true,                                             ?
?    "message": "Purchase batch created successfully",           ?
?    "data": {                                                    ?
?      "purchaseBatchId": "a1b2c3d4-...",                        ?
?      "farmerId": "3fa85f64-...",                               ?
?      "productId": "3fa85f64-...",                              ?
?      "purchaseDate": "2024-01-15T10:30:00Z",                  ?
?      "rawWeight": 1000.50,                                      ?
?      "ratePerKg": 50.00,                                        ?
?      "totalAmount": 50025.00,                                   ?
?      "paymentSettlementType": "BeforeProcessing",              ?
?      "status": "Stored"                                         ?
?    },                                                           ?
?    "errors": null                                               ?
?  }                                                              ?
?                                                                 ?
?  Response headers                                               ?
?  content-type: application/json; charset=utf-8                 ?
?  date: Mon, 15 Jan 2024 10:30:45 GMT                          ?
?  location: /api/purchases/a1b2c3d4-...                        ?
?                                                                 ?
?  [Download] [Copy] [cURL]                                      ?
???????????????????????????????????????????????????????????????????
```

---

## ?? Quick Testing Workflow

### 1?? **Create Master Data First**

```
1. Create Product      ? POST /api/products
2. Create Farmer       ? POST /api/farmers
3. Create Location     ? POST /api/locations
4. Create Worker       ? POST /api/workers
5. Create Buyer        ? POST /api/buyers
```

### 2?? **Purchase Flow**

```
1. Create Purchase     ? POST /api/purchases
2. Record Payment      ? POST /api/purchasepayments
3. Check Outstanding   ? GET /api/purchases/{id}/outstanding
```

### 3?? **Processing Flow**

```
1. Create Run          ? POST /api/processing/runs
2. Add Stage           ? POST /api/processing/runs/{id}/stages
3. Assign Workers      ? POST /api/processing/stages/{id}/workers
4. Complete Run        ? POST /api/processing/runs/complete
```

### 4?? **Sales Flow**

```
1. Check Inventory     ? GET /api/inventory/finished/product/{id}
2. Create Sale         ? POST /api/sales
3. Record Payment      ? POST /api/salespayments
```

### 5?? **Reports**

```
1. Batch Profit        ? GET /api/reports/batch-profit/{batchId}
2. Yield Analysis      ? GET /api/reports/yield-analysis/{productId}
3. Outstanding         ? GET /api/reports/outstanding-payments
```

---

## ?? Pro Tips

### Copy as cURL
After executing, click **cURL** button to get command like:
```bash
curl -X 'POST' \
  'https://localhost:7244/api/purchases' \
  -H 'Content-Type: application/json' \
  -d '{
  "farmerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  ...
}'
```

### Filter Endpoints
Use the filter box to search:
- Type "purchase" ? Shows all purchase-related endpoints
- Type "GET" ? Shows only GET endpoints
- Type "reports" ? Shows reporting endpoints

### Schema Inspection
Click **Schemas** dropdown at top to see:
- Request DTOs
- Response models
- Domain entities

### Model Examples
Swagger auto-generates example values:
- GUIDs: `3fa85f64-5717-4562-b3fc-2c963f66afa6`
- Dates: `2024-01-15T10:30:00Z`
- Numbers: 0
- Strings: "string"

You can edit these directly!

---

## ?? UI Features Enabled

? **Display Request Duration** - See API performance  
? **Enable Filter** - Search endpoints easily  
? **Try It Out by Default** - Ready to test immediately  
? **Expanded Models** - See full DTO structure  
? **Organized by Tags** - Grouped by functional area  

---

## ?? Security Note

?? **Swagger is ONLY available in Development mode**

In Production:
- Swagger UI is disabled
- API endpoints still work
- Use Postman or other tools

---

## ?? Export for Team

### For Postman Users
1. Copy Swagger JSON URL: `https://localhost:7244/swagger/v1/swagger.json`
2. Postman ? Import ? Link
3. Paste URL
4. All endpoints imported!

### For API Documentation
Share this URL with your team:
```
https://localhost:7244/swagger
```

---

## ? What Makes This Special

Compared to default Swagger:
- ? Organized by business domains
- ? Consistent response format
- ? Better error messages
- ? Request duration tracking
- ? Enhanced filtering
- ? Try-it-out enabled by default
- ? Professional API documentation

**Happy Testing! ??**
