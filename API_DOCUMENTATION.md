# AgroProcessing REST API Documentation

## Base URL
- Development: `https://localhost:7xxx/api`
- Swagger UI: `https://localhost:7xxx/swagger`

## Response Format
All API responses follow this structure:
```json
{
  "success": true|false,
  "message": "Optional message",
  "data": { /* Response data */ },
  "errors": ["Error messages if any"]
}
```

## API Endpoints

### 1. Products API
**Base Route:** `/api/products`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all active products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create new product |
| PATCH | `/api/products/{id}/toggle-status` | Toggle product active status |

### 2. Farmers API
**Base Route:** `/api/farmers`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/farmers` | Get all farmers |
| GET | `/api/farmers/{id}` | Get farmer by ID |
| POST | `/api/farmers` | Create new farmer |

### 3. Workers API
**Base Route:** `/api/workers`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/workers` | Get all active workers |
| GET | `/api/workers/{id}` | Get worker by ID |
| POST | `/api/workers` | Create new worker |
| PATCH | `/api/workers/{id}/toggle-status` | Toggle worker active status |

### 4. Buyers API
**Base Route:** `/api/buyers`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/buyers` | Get all buyers |
| GET | `/api/buyers/{id}` | Get buyer by ID |
| POST | `/api/buyers` | Create new buyer |

### 5. Locations API
**Base Route:** `/api/locations`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/locations` | Get all locations |
| POST | `/api/locations` | Create new location |

### 6. Purchases API
**Base Route:** `/api/purchases`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/purchases` | Create new purchase batch |
| GET | `/api/purchases/{id}` | Get purchase batch by ID |
| GET | `/api/purchases/farmer/{farmerId}` | Get purchases by farmer |
| GET | `/api/purchases/pending` | Get pending purchase batches |
| GET | `/api/purchases/{id}/outstanding` | Get outstanding amount |
| GET | `/api/purchases/{id}/can-process` | Check if batch can be processed |

**Create Purchase Request:**
```json
{
  "farmerId": "guid",
  "productId": "guid",
  "purchaseDate": "2024-01-15T10:30:00Z",
  "rawWeight": 1000.50,
  "ratePerKg": 50.00,
  "paymentSettlementType": "BeforeProcessing",
  "initialPayment": 25000.00,
  "paymentMode": "Cash"
}
```

### 7. Purchase Payments API
**Base Route:** `/api/purchasepayments`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/purchasepayments` | Record new payment |
| GET | `/api/purchasepayments/batch/{batchId}` | Get payments by batch |
| GET | `/api/purchasepayments/batch/{batchId}/total` | Get total paid |
| GET | `/api/purchasepayments/overdue` | Get overdue payments |
| PATCH | `/api/purchasepayments/{id}/mark-paid` | Mark payment as paid |

### 8. Inventory API
**Base Route:** `/api/inventory`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/inventory/raw/location/{locationId}` | Get raw inventory by location |
| GET | `/api/inventory/raw/product/{productId}` | Get raw inventory by product |
| GET | `/api/inventory/raw/batch/{batchId}/available` | Get available raw quantity |
| GET | `/api/inventory/finished/location/{locationId}` | Get finished inventory by location |
| GET | `/api/inventory/finished/product/{productId}` | Get finished inventory by product |
| GET | `/api/inventory/finished/product/{productId}/available` | Get available finished stock |

### 9. Processing API
**Base Route:** `/api/processing`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/processing/runs` | Create new processing run |
| GET | `/api/processing/runs/{id}` | Get processing run by ID |
| GET | `/api/processing/runs/active` | Get active processing runs |
| POST | `/api/processing/runs/{runId}/stages` | Add processing stage |
| PATCH | `/api/processing/stages/{stageId}/complete` | Complete processing stage |
| POST | `/api/processing/stages/{stageId}/workers` | Assign worker to stage |
| POST | `/api/processing/runs/complete` | Complete processing run |
| GET | `/api/processing/runs/{runId}/cost` | Get total processing cost |

**Create Processing Run Request:**
```json
{
  "productId": "guid",
  "startDate": "2024-01-15T08:00:00Z",
  "inputs": [
    {
      "purchaseBatchId": "guid",
      "inputWeight": 500.00
    }
  ]
}
```

**Complete Processing Run Request:**
```json
{
  "processingRunId": "guid",
  "endDate": "2024-01-20T17:00:00Z",
  "totalOutputWeight": 450.00,
  "additionalCosts": [
    {
      "costType": "Electricity",
      "amount": 5000.00
    }
  ]
}
```

### 10. Sales API
**Base Route:** `/api/sales`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/sales` | Create new sale |
| GET | `/api/sales/{id}` | Get sale by ID |
| GET | `/api/sales/buyer/{buyerId}` | Get sales by buyer |
| GET | `/api/sales/open` | Get open sales |
| PATCH | `/api/sales/{id}/close` | Close sale |
| GET | `/api/sales/buyer/{buyerId}/outstanding` | Get buyer outstanding |
| POST | `/api/sales/buyer/{buyerId}/validate-credit` | Validate buyer credit limit |

**Create Sale Request:**
```json
{
  "buyerId": "guid",
  "saleDate": "2024-01-15T10:00:00Z",
  "items": [
    {
      "productId": "guid",
      "quantity": 100.00,
      "rate": 120.00
    }
  ]
}
```

### 11. Sales Payments API
**Base Route:** `/api/salespayments`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/salespayments` | Record sales payment |
| GET | `/api/salespayments/sale/{saleId}` | Get payments by sale |
| GET | `/api/salespayments/sale/{saleId}/total` | Get total paid for sale |
| GET | `/api/salespayments/overdue` | Get overdue sales payments |
| PATCH | `/api/salespayments/{id}/mark-paid` | Mark sales payment as paid |

### 12. Transportation API
**Base Route:** `/api/transportation`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/transportation` | Record transportation |
| GET | `/api/transportation/{relatedType}/{relatedId}` | Get transportation by type |
| GET | `/api/transportation/{relatedType}/{relatedId}/total-cost` | Get total cost |
| GET | `/api/transportation/unpaid` | Get unpaid transportations |
| PATCH | `/api/transportation/{id}/mark-paid` | Mark transportation as paid |

### 13. Worker Payments API
**Base Route:** `/api/workerpayments`

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/workerpayments` | Create worker payment |
| GET | `/api/workerpayments/unpaid` | Get unpaid worker payments |
| GET | `/api/workerpayments/worker/{workerId}` | Get payments by worker |
| GET | `/api/workerpayments/worker/{workerId}/unpaid-total` | Get total unpaid |
| GET | `/api/workerpayments/stage/{stageId}` | Get payments by stage |
| PATCH | `/api/workerpayments/{id}/mark-paid` | Mark worker payment as paid |

### 14. Reports API
**Base Route:** `/api/reports`

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/reports/batch-profit/{batchId}` | Get batch profit report |
| GET | `/api/reports/yield-analysis/{productId}` | Get yield analysis report |
| GET | `/api/reports/outstanding-payments` | Get outstanding payments report |
| GET | `/api/reports/inventory-valuation` | Get inventory valuation report |
| GET | `/api/reports/worker-payment-summary?workerId={id}` | Get worker payment summary |
| GET | `/api/reports/processing-cost-analysis/{runId}` | Get processing cost analysis |

## HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success |
| 201 | Created |
| 400 | Bad Request (validation error) |
| 404 | Not Found |
| 500 | Internal Server Error |

## Error Handling

All errors return consistent format:
```json
{
  "success": false,
  "message": "Error description",
  "errors": ["Detailed error 1", "Detailed error 2"]
}
```

## Testing with Swagger

Navigate to `/swagger` in development mode to access interactive API documentation and test endpoints.
