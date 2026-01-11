# API Testing Commands (cURL)

Replace `localhost:7244` with your actual port number.

## ?? Master Data Setup

### Create Product (Turmeric)
```bash
curl -X POST "https://localhost:7244/api/products" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Turmeric",
    "expectedYieldPercent": 85.5,
    "dryingDaysMin": 7,
    "dryingDaysMax": 10,
    "isActive": true
  }'
```

### Create Farmer
```bash
curl -X POST "https://localhost:7244/api/farmers" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Doe",
    "phone": "+1234567890"
  }'
```

### Create Location (Raw Storage)
```bash
curl -X POST "https://localhost:7244/api/locations" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Warehouse A",
    "locationType": "Raw"
  }'
```

### Create Worker
```bash
curl -X POST "https://localhost:7244/api/workers" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Worker One",
    "skillType": "Cleaning",
    "defaultRate": 500.00,
    "isActive": true
  }'
```

### Create Buyer
```bash
curl -X POST "https://localhost:7244/api/buyers" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "ABC Trading Co",
    "phone": "+1234567890",
    "creditLimit": 500000.00
  }'
```

### Get All Products
```bash
curl -X GET "https://localhost:7244/api/products"
```

---

## ?? Purchase Operations

### Create Purchase Batch
```bash
curl -X POST "https://localhost:7244/api/purchases" \
  -H "Content-Type: application/json" \
  -d '{
    "farmerId": "YOUR_FARMER_ID",
    "productId": "YOUR_PRODUCT_ID",
    "purchaseDate": "2024-01-15T10:30:00Z",
    "rawWeight": 1000.50,
    "ratePerKg": 50.00,
    "paymentSettlementType": "BeforeProcessing",
    "initialPayment": 25000.00,
    "paymentMode": "Cash"
  }'
```

### Record Purchase Payment
```bash
curl -X POST "https://localhost:7244/api/purchasepayments" \
  -H "Content-Type: application/json" \
  -d '{
    "purchaseBatchId": "YOUR_BATCH_ID",
    "amountPaid": 25025.00,
    "paymentDate": "2024-01-16T10:00:00Z",
    "dueDate": "2024-01-16T10:00:00Z",
    "paymentMode": "Bank Transfer",
    "remarks": "Final payment"
  }'
```

### Get Pending Purchase Batches
```bash
curl -X GET "https://localhost:7244/api/purchases/pending"
```

### Check Outstanding Amount
```bash
curl -X GET "https://localhost:7244/api/purchases/YOUR_BATCH_ID/outstanding"
```

---

## ?? Processing Operations

### Create Processing Run
```bash
curl -X POST "https://localhost:7244/api/processing/runs" \
  -H "Content-Type: application/json" \
  -d '{
    "productId": "YOUR_PRODUCT_ID",
    "startDate": "2024-01-15T08:00:00Z",
    "inputs": [
      {
        "purchaseBatchId": "YOUR_BATCH_ID",
        "inputWeight": 500.00
      }
    ]
  }'
```

### Add Processing Stage
```bash
curl -X POST "https://localhost:7244/api/processing/runs/YOUR_RUN_ID/stages" \
  -H "Content-Type: application/json" \
  -d '{
    "workTypeId": "YOUR_WORKTYPE_ID",
    "startDate": "2024-01-15T08:30:00Z"
  }'
```

### Assign Worker to Stage
```bash
curl -X POST "https://localhost:7244/api/processing/stages/YOUR_STAGE_ID/workers" \
  -H "Content-Type: application/json" \
  -d '{
    "workerId": "YOUR_WORKER_ID",
    "workedDays": 5
  }'
```

### Complete Processing Stage
```bash
curl -X PATCH "https://localhost:7244/api/processing/stages/YOUR_STAGE_ID/complete" \
  -H "Content-Type: application/json" \
  -d '"2024-01-20T17:00:00Z"'
```

### Complete Processing Run
```bash
curl -X POST "https://localhost:7244/api/processing/runs/complete" \
  -H "Content-Type: application/json" \
  -d '{
    "processingRunId": "YOUR_RUN_ID",
    "endDate": "2024-01-20T17:00:00Z",
    "totalOutputWeight": 427.50,
    "additionalCosts": [
      {
        "costType": "Electricity",
        "amount": 5000.00
      },
      {
        "costType": "Materials",
        "amount": 2000.00
      }
    ]
  }'
```

### Get Active Processing Runs
```bash
curl -X GET "https://localhost:7244/api/processing/runs/active"
```

---

## ?? Inventory Queries

### Get Raw Inventory by Product
```bash
curl -X GET "https://localhost:7244/api/inventory/raw/product/YOUR_PRODUCT_ID"
```

### Get Finished Inventory by Product
```bash
curl -X GET "https://localhost:7244/api/inventory/finished/product/YOUR_PRODUCT_ID"
```

### Get Available Finished Stock for Sale
```bash
curl -X GET "https://localhost:7244/api/inventory/finished/product/YOUR_PRODUCT_ID/available"
```

---

## ?? Sales Operations

### Create Sale
```bash
curl -X POST "https://localhost:7244/api/sales" \
  -H "Content-Type: application/json" \
  -d '{
    "buyerId": "YOUR_BUYER_ID",
    "saleDate": "2024-01-22T10:00:00Z",
    "items": [
      {
        "productId": "YOUR_PRODUCT_ID",
        "quantity": 100.00,
        "rate": 120.00
      }
    ]
  }'
```

### Get Open Sales
```bash
curl -X GET "https://localhost:7244/api/sales/open"
```

### Get Buyer Outstanding
```bash
curl -X GET "https://localhost:7244/api/sales/buyer/YOUR_BUYER_ID/outstanding"
```

### Record Sales Payment
```bash
curl -X POST "https://localhost:7244/api/salespayments" \
  -H "Content-Type: application/json" \
  -d '{
    "saleId": "YOUR_SALE_ID",
    "amountPaid": 6000.00,
    "paymentDate": "2024-01-23T10:00:00Z",
    "dueDate": "2024-01-23T10:00:00Z",
    "paymentMode": "Cash"
  }'
```

### Close Sale
```bash
curl -X PATCH "https://localhost:7244/api/sales/YOUR_SALE_ID/close"
```

---

## ?? Transportation

### Record Transportation
```bash
curl -X POST "https://localhost:7244/api/transportation" \
  -H "Content-Type: application/json" \
  -d '{
    "relatedType": "Purchase",
    "relatedId": "YOUR_BATCH_ID",
    "fromLocationId": "LOCATION_A_ID",
    "toLocationId": "LOCATION_B_ID",
    "cost": 3000.00
  }'
```

### Get Unpaid Transportations
```bash
curl -X GET "https://localhost:7244/api/transportation/unpaid"
```

---

## ?? Worker Payments

### Create Worker Payment
```bash
curl -X POST "https://localhost:7244/api/workerpayments" \
  -H "Content-Type: application/json" \
  -d '{
    "workerId": "YOUR_WORKER_ID",
    "processingStageId": "YOUR_STAGE_ID",
    "amount": 2500.00
  }'
```

### Get Unpaid Worker Payments
```bash
curl -X GET "https://localhost:7244/api/workerpayments/unpaid"
```

### Mark Worker Payment as Paid
```bash
curl -X PATCH "https://localhost:7244/api/workerpayments/YOUR_PAYMENT_ID/mark-paid" \
  -H "Content-Type: application/json" \
  -d '"2024-01-25T10:00:00Z"'
```

---

## ?? Reports

### Batch Profit Report
```bash
curl -X GET "https://localhost:7244/api/reports/batch-profit/YOUR_BATCH_ID"
```

### Yield Analysis Report
```bash
curl -X GET "https://localhost:7244/api/reports/yield-analysis/YOUR_PRODUCT_ID"
```

### Outstanding Payments Report
```bash
curl -X GET "https://localhost:7244/api/reports/outstanding-payments"
```

### Inventory Valuation Report
```bash
curl -X GET "https://localhost:7244/api/reports/inventory-valuation"
```

### Worker Payment Summary
```bash
# All workers
curl -X GET "https://localhost:7244/api/reports/worker-payment-summary"

# Specific worker
curl -X GET "https://localhost:7244/api/reports/worker-payment-summary?workerId=YOUR_WORKER_ID"
```

### Processing Cost Analysis
```bash
curl -X GET "https://localhost:7244/api/reports/processing-cost-analysis/YOUR_RUN_ID"
```

---

## ?? Testing Tips

### 1. Pretty Print JSON Response
Add `| jq` at the end (requires jq installed):
```bash
curl -X GET "https://localhost:7244/api/products" | jq
```

### 2. Save Response to File
```bash
curl -X GET "https://localhost:7244/api/products" -o products.json
```

### 3. Show Response Headers
```bash
curl -v -X GET "https://localhost:7244/api/products"
```

### 4. Silent Mode (No Progress Bar)
```bash
curl -s -X GET "https://localhost:7244/api/products"
```

### 5. Ignore SSL Certificate (Development Only)
```bash
curl -k -X GET "https://localhost:7244/api/products"
```

---

## ?? Complete Workflow Script

```bash
#!/bin/bash
BASE_URL="https://localhost:7244"

# 1. Create Product
PRODUCT=$(curl -s -X POST "$BASE_URL/api/products" \
  -H "Content-Type: application/json" \
  -d '{"name":"Turmeric","expectedYieldPercent":85.5,"dryingDaysMin":7,"dryingDaysMax":10,"isActive":true}')
PRODUCT_ID=$(echo $PRODUCT | jq -r '.data.productId')
echo "Created Product: $PRODUCT_ID"

# 2. Create Farmer
FARMER=$(curl -s -X POST "$BASE_URL/api/farmers" \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","phone":"+1234567890"}')
FARMER_ID=$(echo $FARMER | jq -r '.data.farmerId')
echo "Created Farmer: $FARMER_ID"

# 3. Create Purchase
PURCHASE=$(curl -s -X POST "$BASE_URL/api/purchases" \
  -H "Content-Type: application/json" \
  -d "{\"farmerId\":\"$FARMER_ID\",\"productId\":\"$PRODUCT_ID\",\"purchaseDate\":\"2024-01-15T10:30:00Z\",\"rawWeight\":1000.50,\"ratePerKg\":50.00,\"paymentSettlementType\":\"AfterYield\",\"initialPayment\":0}")
BATCH_ID=$(echo $PURCHASE | jq -r '.data.purchaseBatchId')
echo "Created Purchase Batch: $BATCH_ID"

# Continue with processing, sales, etc...
```

---

**?? Pro Tip:** Use Swagger UI for initial testing, then export to cURL for automation!
