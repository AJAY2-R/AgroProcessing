# ?? Quick Start - Swagger API Testing

## Step 1: Run the Application (30 seconds)

```bash
cd AgroProcessing
dotnet run
```

Wait for:
```
Now listening on: https://localhost:7244
```

---

## Step 2: Open Swagger UI (10 seconds)

Open your browser:
```
https://localhost:7244/swagger
```

You'll see the AgroProcessing API documentation!

---

## Step 3: Test Your First Endpoint (2 minutes)

### Create a Product

1. **Find the endpoint:**
   - Scroll to "Master Data - Products"
   - Click `POST /api/products`

2. **Try it out:**
   - Click the **"Try it out"** button

3. **Edit the request:**
   ```json
   {
     "name": "Organic Turmeric",
     "expectedYieldPercent": 85.5,
     "dryingDaysMin": 7,
     "dryingDaysMax": 10,
     "isActive": true
   }
   ```

4. **Execute:**
   - Click **"Execute"** button

5. **View response:**
   ```json
   {
     "success": true,
     "message": "Product created successfully",
     "data": {
       "productId": "12345678-1234-1234-1234-123456789012",
       "name": "Organic Turmeric",
       "expectedYieldPercent": 85.5,
       "dryingDaysMin": 7,
       "dryingDaysMax": 10,
       "isActive": true
     }
   }
   ```

6. **Copy the productId** for next steps!

---

## Step 4: Create a Complete Workflow (5 minutes)

### A. Create Farmer
```
POST /api/farmers
{
  "name": "John Smith",
  "phone": "+1-555-0123"
}
```
**Copy the `farmerId`**

### B. Create Location
```
POST /api/locations
{
  "name": "Main Warehouse",
  "locationType": "Raw"
}
```

### C. Create Purchase Batch
```
POST /api/purchases
{
  "farmerId": "{paste-farmerId-here}",
  "productId": "{paste-productId-here}",
  "purchaseDate": "2024-01-15T10:00:00Z",
  "rawWeight": 1000.00,
  "ratePerKg": 50.00,
  "paymentSettlementType": "AfterYield",
  "initialPayment": 0
}
```
**Copy the `purchaseBatchId`**

### D. View Pending Purchases
```
GET /api/purchases/pending
```

You'll see your purchase in the list!

### E. Check Raw Inventory
```
GET /api/inventory/raw/product/{your-productId}
```

---

## Step 5: Export to Postman (Optional, 1 minute)

1. Copy this URL:
   ```
   https://localhost:7244/swagger/v1/swagger.json
   ```

2. Open Postman ? Import ? Link

3. Paste the URL

4. All 70+ endpoints imported!

---

## ?? Common Testing Patterns

### Pattern 1: Master Data Setup (Do This First)
```
1. POST /api/products       ? Create products
2. POST /api/farmers        ? Create farmers
3. POST /api/workers        ? Create workers
4. POST /api/buyers         ? Create buyers
5. POST /api/locations      ? Create locations
```

### Pattern 2: Purchase Flow
```
1. POST /api/purchases                    ? Create purchase
2. GET  /api/purchases/{id}/outstanding   ? Check payment
3. POST /api/purchasepayments             ? Record payment
4. GET  /api/inventory/raw/product/{id}   ? Verify inventory
```

### Pattern 3: Processing Flow
```
1. POST /api/processing/runs                  ? Start processing
2. POST /api/processing/runs/{id}/stages      ? Add stage
3. POST /api/processing/stages/{id}/workers   ? Assign workers
4. POST /api/processing/runs/complete         ? Complete run
5. GET  /api/inventory/finished/product/{id}  ? Check output
```

### Pattern 4: Sales Flow
```
1. GET  /api/inventory/finished/product/{id}/available ? Check stock
2. POST /api/sales                                      ? Create sale
3. POST /api/salespayments                              ? Record payment
4. GET  /api/sales/buyer/{id}/outstanding               ? Check dues
```

### Pattern 5: Reporting
```
1. GET /api/reports/batch-profit/{batchId}
2. GET /api/reports/yield-analysis/{productId}
3. GET /api/reports/outstanding-payments
4. GET /api/reports/inventory-valuation
```

---

## ?? Tips for Efficient Testing

### 1. Use the Filter Box
Type keywords to find endpoints quickly:
- `purchase` ? Shows all purchase endpoints
- `GET` ? Shows only GET endpoints
- `reports` ? Shows all reports

### 2. Keep IDs Handy
Open a text file and save IDs as you create records:
```
ProductId: 12345678-1234-1234-1234-123456789012
FarmerId:  23456789-2345-2345-2345-234567890123
BatchId:   34567890-3456-3456-3456-345678901234
```

### 3. Use Realistic Dates
```
Today:       "2024-01-15T10:00:00Z"
Yesterday:   "2024-01-14T10:00:00Z"
Next week:   "2024-01-22T10:00:00Z"
```

### 4. Test Error Cases
Try invalid inputs:
- Negative weights
- Missing required fields
- Invalid GUIDs
- Past due dates

### 5. Watch Request Duration
Look at the duration display:
- < 100ms ? Excellent
- 100-500ms ? Good
- > 500ms ? May need optimization

---

## ?? Troubleshooting

### Problem: Swagger UI Not Loading
**Solution:**
```bash
# Check if app is running
netstat -an | findstr "7244"

# Restart the app
Ctrl+C
dotnet run
```

### Problem: "404 Not Found"
**Cause:** Wrong URL or app not running  
**Fix:** Check URL is `https://localhost:7244/swagger` (not /swagger/index.html)

### Problem: "Invalid GUID format"
**Cause:** Copied GUID incorrectly  
**Fix:** Use GUID from response, format: `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx`

### Problem: "Farmer not found"
**Cause:** Using wrong farmerId  
**Fix:** Create farmer first, then use returned ID

### Problem: SSL Certificate Error
**Cause:** Development certificate not trusted  
**Fix:** 
```bash
dotnet dev-certs https --trust
```

---

## ?? Next Steps

1. ? Test all Master Data endpoints
2. ? Create a complete purchase-to-sale workflow
3. ? Generate reports
4. ? Export to Postman
5. ? Share with your team

---

## ?? More Resources

| Document | Purpose |
|----------|---------|
| `SWAGGER_GUIDE.md` | Complete Swagger documentation |
| `SWAGGER_UI_GUIDE.md` | Visual UI reference |
| `CURL_COMMANDS.md` | cURL command examples |
| `API_DOCUMENTATION.md` | Full API reference |
| `ARCHITECTURE_WITH_SWAGGER.md` | System architecture |

---

## ?? You're Ready!

Your AgroProcessing API is fully configured with Swagger.

**Start testing now:** `https://localhost:7244/swagger`

Happy testing! ??
