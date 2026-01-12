using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroProcessing.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgreSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buyers",
                columns: table => new
                {
                    BuyerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CreditLimit = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.BuyerId);
                });

            migrationBuilder.CreateTable(
                name: "Farmers",
                columns: table => new
                {
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmers", x => x.FarmerId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LocationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExpectedYieldPercent = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    DryingDaysMin = table.Column<int>(type: "integer", nullable: false),
                    DryingDaysMax = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    WorkerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SkillType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DefaultRate = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.WorkerId);
                });

            migrationBuilder.CreateTable(
                name: "WorkTypes",
                columns: table => new
                {
                    WorkTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RateType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTypes", x => x.WorkTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Open")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SaleId);
                    table.ForeignKey(
                        name: "FK_Sales_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Buyers",
                        principalColumn: "BuyerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transportations",
                columns: table => new
                {
                    TransportationId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelatedType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RelatedId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transportations", x => x.TransportationId);
                    table.ForeignKey(
                        name: "FK_Transportations_Locations_FromLocationId",
                        column: x => x.FromLocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transportations_Locations_ToLocationId",
                        column: x => x.ToLocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingRuns",
                columns: table => new
                {
                    ProcessingRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Started")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingRuns", x => x.ProcessingRunId);
                    table.ForeignKey(
                        name: "FK_ProcessingRuns_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseBatches",
                columns: table => new
                {
                    PurchaseBatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RawWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    RatePerKg = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PaymentSettlementType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Stored")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseBatches", x => x.PurchaseBatchId);
                    table.ForeignKey(
                        name: "FK_PurchaseBatches_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "FarmerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseBatches_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaleItems",
                columns: table => new
                {
                    SaleItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItems", x => x.SaleItemId);
                    table.ForeignKey(
                        name: "FK_SaleItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleItems_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesPayments",
                columns: table => new
                {
                    SalesPaymentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AmountPaid = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentMode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPayments", x => x.SalesPaymentId);
                    table.ForeignKey(
                        name: "FK_SalesPayments_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "SaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingCosts",
                columns: table => new
                {
                    ProcessingCostId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessingRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    CostType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingCosts", x => x.ProcessingCostId);
                    table.ForeignKey(
                        name: "FK_ProcessingCosts_ProcessingRuns_ProcessingRunId",
                        column: x => x.ProcessingRunId,
                        principalTable: "ProcessingRuns",
                        principalColumn: "ProcessingRunId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingOutputs",
                columns: table => new
                {
                    ProcessingOutputId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessingRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalInputWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalOutputWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalLossWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    YieldPercent = table.Column<decimal>(type: "numeric(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingOutputs", x => x.ProcessingOutputId);
                    table.ForeignKey(
                        name: "FK_ProcessingOutputs_ProcessingRuns_ProcessingRunId",
                        column: x => x.ProcessingRunId,
                        principalTable: "ProcessingRuns",
                        principalColumn: "ProcessingRunId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingStages",
                columns: table => new
                {
                    ProcessingStageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessingRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingStages", x => x.ProcessingStageId);
                    table.ForeignKey(
                        name: "FK_ProcessingStages_ProcessingRuns_ProcessingRunId",
                        column: x => x.ProcessingRunId,
                        principalTable: "ProcessingRuns",
                        principalColumn: "ProcessingRunId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessingStages_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "WorkTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BatchOutputAllocations",
                columns: table => new
                {
                    BatchOutputAllocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessingRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseBatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    InputWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OutputWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AllocatedProcessingCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchOutputAllocations", x => x.BatchOutputAllocationId);
                    table.ForeignKey(
                        name: "FK_BatchOutputAllocations_ProcessingRuns_ProcessingRunId",
                        column: x => x.ProcessingRunId,
                        principalTable: "ProcessingRuns",
                        principalColumn: "ProcessingRunId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BatchOutputAllocations_PurchaseBatches_PurchaseBatchId",
                        column: x => x.PurchaseBatchId,
                        principalTable: "PurchaseBatches",
                        principalColumn: "PurchaseBatchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinishedInventories",
                columns: table => new
                {
                    FinishedInventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseBatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinishedInventories", x => x.FinishedInventoryId);
                    table.ForeignKey(
                        name: "FK_FinishedInventories_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinishedInventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinishedInventories_PurchaseBatches_PurchaseBatchId",
                        column: x => x.PurchaseBatchId,
                        principalTable: "PurchaseBatches",
                        principalColumn: "PurchaseBatchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingRunInputs",
                columns: table => new
                {
                    ProcessingRunInputId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessingRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseBatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    InputWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingRunInputs", x => x.ProcessingRunInputId);
                    table.ForeignKey(
                        name: "FK_ProcessingRunInputs_ProcessingRuns_ProcessingRunId",
                        column: x => x.ProcessingRunId,
                        principalTable: "ProcessingRuns",
                        principalColumn: "ProcessingRunId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessingRunInputs_PurchaseBatches_PurchaseBatchId",
                        column: x => x.PurchaseBatchId,
                        principalTable: "PurchaseBatches",
                        principalColumn: "PurchaseBatchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchasePayments",
                columns: table => new
                {
                    PurchasePaymentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseBatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AmountPaid = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentMode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasePayments", x => x.PurchasePaymentId);
                    table.ForeignKey(
                        name: "FK_PurchasePayments_PurchaseBatches_PurchaseBatchId",
                        column: x => x.PurchaseBatchId,
                        principalTable: "PurchaseBatches",
                        principalColumn: "PurchaseBatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RawInventories",
                columns: table => new
                {
                    RawInventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseBatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawInventories", x => x.RawInventoryId);
                    table.ForeignKey(
                        name: "FK_RawInventories_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RawInventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RawInventories_PurchaseBatches_PurchaseBatchId",
                        column: x => x.PurchaseBatchId,
                        principalTable: "PurchaseBatches",
                        principalColumn: "PurchaseBatchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaleBatchAllocations",
                columns: table => new
                {
                    SaleBatchAllocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseBatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantityAllocated = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleBatchAllocations", x => x.SaleBatchAllocationId);
                    table.ForeignKey(
                        name: "FK_SaleBatchAllocations_PurchaseBatches_PurchaseBatchId",
                        column: x => x.PurchaseBatchId,
                        principalTable: "PurchaseBatches",
                        principalColumn: "PurchaseBatchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleBatchAllocations_SaleItems_SaleItemId",
                        column: x => x.SaleItemId,
                        principalTable: "SaleItems",
                        principalColumn: "SaleItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessingStageWorkers",
                columns: table => new
                {
                    ProcessingStageWorkerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessingStageId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkerId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkedDays = table.Column<int>(type: "integer", nullable: false),
                    CalculatedCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessingStageWorkers", x => x.ProcessingStageWorkerId);
                    table.ForeignKey(
                        name: "FK_ProcessingStageWorkers_ProcessingStages_ProcessingStageId",
                        column: x => x.ProcessingStageId,
                        principalTable: "ProcessingStages",
                        principalColumn: "ProcessingStageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessingStageWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkerPayments",
                columns: table => new
                {
                    WorkerPaymentId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessingStageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PaidStatus = table.Column<bool>(type: "boolean", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerPayments", x => x.WorkerPaymentId);
                    table.ForeignKey(
                        name: "FK_WorkerPayments_ProcessingStages_ProcessingStageId",
                        column: x => x.ProcessingStageId,
                        principalTable: "ProcessingStages",
                        principalColumn: "ProcessingStageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerPayments_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchOutputAllocations_ProcessingRunId",
                table: "BatchOutputAllocations",
                column: "ProcessingRunId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchOutputAllocations_PurchaseBatchId",
                table: "BatchOutputAllocations",
                column: "PurchaseBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_Name",
                table: "Buyers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FinishedInventories_LocationId",
                table: "FinishedInventories",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FinishedInventories_ProductId",
                table: "FinishedInventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FinishedInventories_PurchaseBatchId",
                table: "FinishedInventories",
                column: "PurchaseBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingCosts_ProcessingRunId",
                table: "ProcessingCosts",
                column: "ProcessingRunId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingOutputs_ProcessingRunId",
                table: "ProcessingOutputs",
                column: "ProcessingRunId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingRunInputs_ProcessingRunId",
                table: "ProcessingRunInputs",
                column: "ProcessingRunId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingRunInputs_PurchaseBatchId",
                table: "ProcessingRunInputs",
                column: "PurchaseBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingRuns_ProductId",
                table: "ProcessingRuns",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingStages_ProcessingRunId",
                table: "ProcessingStages",
                column: "ProcessingRunId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingStages_WorkTypeId",
                table: "ProcessingStages",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingStageWorkers_ProcessingStageId",
                table: "ProcessingStageWorkers",
                column: "ProcessingStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessingStageWorkers_WorkerId",
                table: "ProcessingStageWorkers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseBatches_FarmerId",
                table: "PurchaseBatches",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseBatches_ProductId",
                table: "PurchaseBatches",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePayments_PurchaseBatchId",
                table: "PurchasePayments",
                column: "PurchaseBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_RawInventories_LocationId",
                table: "RawInventories",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RawInventories_ProductId",
                table: "RawInventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RawInventories_PurchaseBatchId",
                table: "RawInventories",
                column: "PurchaseBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleBatchAllocations_PurchaseBatchId",
                table: "SaleBatchAllocations",
                column: "PurchaseBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleBatchAllocations_SaleItemId",
                table: "SaleBatchAllocations",
                column: "SaleItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_ProductId",
                table: "SaleItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_SaleId",
                table: "SaleItems",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_BuyerId",
                table: "Sales",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPayments_SaleId",
                table: "SalesPayments",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_FromLocationId",
                table: "Transportations",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_ToLocationId",
                table: "Transportations",
                column: "ToLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerPayments_ProcessingStageId",
                table: "WorkerPayments",
                column: "ProcessingStageId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerPayments_WorkerId",
                table: "WorkerPayments",
                column: "WorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchOutputAllocations");

            migrationBuilder.DropTable(
                name: "FinishedInventories");

            migrationBuilder.DropTable(
                name: "ProcessingCosts");

            migrationBuilder.DropTable(
                name: "ProcessingOutputs");

            migrationBuilder.DropTable(
                name: "ProcessingRunInputs");

            migrationBuilder.DropTable(
                name: "ProcessingStageWorkers");

            migrationBuilder.DropTable(
                name: "PurchasePayments");

            migrationBuilder.DropTable(
                name: "RawInventories");

            migrationBuilder.DropTable(
                name: "SaleBatchAllocations");

            migrationBuilder.DropTable(
                name: "SalesPayments");

            migrationBuilder.DropTable(
                name: "Transportations");

            migrationBuilder.DropTable(
                name: "WorkerPayments");

            migrationBuilder.DropTable(
                name: "PurchaseBatches");

            migrationBuilder.DropTable(
                name: "SaleItems");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "ProcessingStages");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Farmers");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "ProcessingRuns");

            migrationBuilder.DropTable(
                name: "WorkTypes");

            migrationBuilder.DropTable(
                name: "Buyers");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
