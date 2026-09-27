using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[]
                {
                    "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail",
                    "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
                    "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled",
                    "LockoutEnd", "LockoutEnabled", "AccessFailedCount",
                    "FullName", "Address", "IsActive", "SellerStatus", "CreatedAt"
                },
                values: new object[,]
                {
                    {
                        "b1b10000-0000-0000-0000-000000000001", "seller1@example.com", "SELLER1@EXAMPLE.COM",
                        "seller1@example.com", "SELLER1@EXAMPLE.COM", true,
                        "AQAAAAEAAYagAAAAEAc448TUKmDIfCeFREDKBiJMfledj65wRDB1/HemMq6OiT+F95UArbppSU8fl49FPA==",
                        "86c80cac-a436-4b61-841b-4e93252ce7b3", "47a9282e-ff42-465e-beb6-6709ae0bb742",
                        null, false, false, null, true, 0,
                        "TechHub Store", "12 Tahrir St, Cairo, Egypt", true, 2, new DateTime(2026, 8, 15, 9, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "b1b10000-0000-0000-0000-000000000002", "seller2@example.com", "SELLER2@EXAMPLE.COM",
                        "seller2@example.com", "SELLER2@EXAMPLE.COM", true,
                        "AQAAAAEAAYagAAAAEGBauU8ha/9a/5FYeFiwJh5Zthv1A/Jkbaq/Ap6mCdHMTWtD6e4BocU1eIZ2Ew2zAg==",
                        "09827242-9859-47bb-a48a-7177b5fbf4dc", "40cc1fc4-951f-4e21-b713-0a0f61499bf2",
                        null, false, false, null, true, 0,
                        "Fashion Corner", "5 Corniche Rd, Alexandria, Egypt", true, 2, new DateTime(2026, 8, 18, 9, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "b1b10000-0000-0000-0000-000000000003", "customer1@example.com", "CUSTOMER1@EXAMPLE.COM",
                        "customer1@example.com", "CUSTOMER1@EXAMPLE.COM", true,
                        "AQAAAAEAAYagAAAAEMk4JCoqL2ZMER1YD+/HVlENThqmomq42NMNf91iaYHq7zsX7aPalSVwxPyQQ8zspA==",
                        "70adfdcf-efee-42f0-8c78-0fe3745e0a02", "850b7260-e231-40d0-bebe-93b98d23aa3b",
                        null, false, false, null, true, 0,
                        "Ahmed Hassan", "10 Nile St, Giza, Egypt", true, 0, new DateTime(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "b1b10000-0000-0000-0000-000000000004", "customer2@example.com", "CUSTOMER2@EXAMPLE.COM",
                        "customer2@example.com", "CUSTOMER2@EXAMPLE.COM", true,
                        "AQAAAAEAAYagAAAAEEW5Pb8CWRrmAQmzFVRXFCUhjSO3qbJZftNdG3qkvqT/U3rv5cZm984f1h9V/2qUiw==",
                        "ac51dec5-ec7e-45ee-b9fb-3783a7694bbf", "1d39303f-a6d9-4ae4-ab97-433aa80141c3",
                        null, false, false, null, true, 0,
                        "Mona Ali", "22 Corniche Rd, Alexandria, Egypt", true, 0, new DateTime(2026, 9, 3, 10, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "b1b10000-0000-0000-0000-000000000005", "customer3@example.com", "CUSTOMER3@EXAMPLE.COM",
                        "customer3@example.com", "CUSTOMER3@EXAMPLE.COM", true,
                        "AQAAAAEAAYagAAAAECypaf7CANh0hBHZ7IWAiyFB226Vs8D+S/+PBZJesR2ssCFmAjdk6f/yX/iW8QK/3w==",
                        "e3add30f-fd2c-44af-9bd7-69093f992e46", "f001c37e-0280-427a-932e-86dcae9460c1",
                        null, false, false, null, true, 0,
                        "Omar Khaled", "7 Road 9, Maadi, Cairo, Egypt", true, 1, new DateTime(2026, 9, 5, 10, 0, 0, DateTimeKind.Utc)
                    }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { "b1b10000-0000-0000-0000-000000000001", "9b7208c3-cef5-4482-b312-84506c143202" },
                    { "b1b10000-0000-0000-0000-000000000002", "9b7208c3-cef5-4482-b312-84506c143202" },
                    { "b1b10000-0000-0000-0000-000000000003", "8e46c643-964d-4055-b340-578b10af7929" },
                    { "b1b10000-0000-0000-0000-000000000004", "8e46c643-964d-4055-b340-578b10af7929" },
                    { "b1b10000-0000-0000-0000-000000000005", "8e46c643-964d-4055-b340-578b10af7929" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[]
                {
                    "Id", "Name", "Description", "Price", "AvailableQuantity", "ImageUrl",
                    "CategoryId", "SellerId", "IsRemovedByAdmin", "IsDeletedBySeller", "CreatedAt"
                },
                values: new object[,]
                {
                    { 1,  "Wireless Bluetooth Headphones",  "Over-ear headphones with active noise cancellation and 30h battery life.", 45.99m,  48, null, 1, "b1b10000-0000-0000-0000-000000000001", false, false, new DateTime(2026, 9, 1, 12, 0, 0, DateTimeKind.Utc) },
                    { 2,  "27-inch 4K Monitor",              "IPS panel, 4K UHD resolution, USB-C connectivity.",                        289.99m, 14, null, 1, "b1b10000-0000-0000-0000-000000000001", false, false, new DateTime(2026, 9, 2, 12, 0, 0, DateTimeKind.Utc) },
                    { 3,  "Mechanical Gaming Keyboard",      "RGB backlit mechanical keyboard with blue switches.",                       69.50m,  29, null, 1, "b1b10000-0000-0000-0000-000000000001", false, false, new DateTime(2026, 9, 3, 12, 0, 0, DateTimeKind.Utc) },
                    { 4,  "Smart Fitness Watch",              "Heart-rate monitor, GPS tracking and 7-day battery life.",                  89.00m,  39, null, 1, "b1b10000-0000-0000-0000-000000000001", false, false, new DateTime(2026, 9, 4, 12, 0, 0, DateTimeKind.Utc) },
                    { 5,  "Men's Slim Fit Denim Jacket",      "Classic blue denim jacket, slim fit, sizes S-XXL.",                         34.99m,  24, null, 2, "b1b10000-0000-0000-0000-000000000002", false, false, new DateTime(2026, 9, 5, 12, 0, 0, DateTimeKind.Utc) },
                    { 6,  "Women's Summer Floral Dress",      "Lightweight floral dress, perfect for summer.",                             27.50m,  20, null, 2, "b1b10000-0000-0000-0000-000000000002", false, false, new DateTime(2026, 9, 6, 12, 0, 0, DateTimeKind.Utc) },
                    { 7,  "Running Shoes",                    "Breathable mesh running shoes with cushioned sole.",                        55.00m,  34, null, 5, "b1b10000-0000-0000-0000-000000000002", false, false, new DateTime(2026, 9, 7, 12, 0, 0, DateTimeKind.Utc) },
                    { 8,  "Leather Wallet",                   "Genuine leather bifold wallet with card slots.",                            19.99m,  60, null, 2, "b1b10000-0000-0000-0000-000000000002", false, false, new DateTime(2026, 9, 8, 12, 0, 0, DateTimeKind.Utc) },
                    { 9,  "Stainless Steel Cookware Set",     "10-piece stainless steel pots and pans set.",                              120.00m,   9, null, 3, "b1b10000-0000-0000-0000-000000000001", false, false, new DateTime(2026, 9, 9, 12, 0, 0, DateTimeKind.Utc) },
                    { 10, "Clean Code",                       "Book by Robert C. Martin — a handbook of agile software craftsmanship.",    22.00m,  45, null, 4, "b1b10000-0000-0000-0000-000000000002", false, false, new DateTime(2026, 9, 10, 12, 0, 0, DateTimeKind.Utc) },
                    { 11, "Yoga Mat",                          "Non-slip 6mm yoga mat with carrying strap.",                                15.00m,  70, null, 5, "b1b10000-0000-0000-0000-000000000002", false, false, new DateTime(2026, 9, 11, 12, 0, 0, DateTimeKind.Utc) },
                    { 12, "Air Fryer 5L",                      "Digital air fryer, 5-liter capacity, 8 preset programs.",                   75.00m,  12, null, 3, "b1b10000-0000-0000-0000-000000000001", false, false, new DateTime(2026, 9, 12, 12, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerId", "OrderDate", "TotalPrice", "ShippingAddress", "Status" },
                values: new object[,]
                {
                    { 1, "b1b10000-0000-0000-0000-000000000003", new DateTime(2026, 9, 10, 10, 15, 0, DateTimeKind.Utc), 161.48m, "10 Nile St, Giza, Egypt",             3 },
                    { 2, "b1b10000-0000-0000-0000-000000000003", new DateTime(2026, 9, 22, 14, 40, 0, DateTimeKind.Utc), 378.99m, "10 Nile St, Giza, Egypt",             2 },
                    { 3, "b1b10000-0000-0000-0000-000000000004", new DateTime(2026, 9, 25,  9,  5, 0, DateTimeKind.Utc),  89.99m, "22 Corniche Rd, Alexandria, Egypt",   1 },
                    { 4, "b1b10000-0000-0000-0000-000000000005", new DateTime(2026, 9, 27,  8, 30, 0, DateTimeKind.Utc), 120.00m, "7 Road 9, Maadi, Cairo, Egypt",       0 },
                    { 5, "b1b10000-0000-0000-0000-000000000004", new DateTime(2026, 9, 15, 16, 20, 0, DateTimeKind.Utc),  27.50m, "22 Corniche Rd, Alexandria, Egypt",   4 }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "OrderId", "ProductId", "SellerId", "Quantity", "UnitPrice", "Status" },
                values: new object[,]
                {
                    { 1, 1, 1, "b1b10000-0000-0000-0000-000000000001", 2, 45.99m,  3 },
                    { 2, 1, 3, "b1b10000-0000-0000-0000-000000000001", 1, 69.50m,  3 },
                    { 3, 2, 4, "b1b10000-0000-0000-0000-000000000001", 1, 89.00m,  2 },
                    { 4, 2, 2, "b1b10000-0000-0000-0000-000000000001", 1, 289.99m, 2 },
                    { 5, 3, 5, "b1b10000-0000-0000-0000-000000000002", 1, 34.99m,  1 },
                    { 6, 3, 7, "b1b10000-0000-0000-0000-000000000002", 1, 55.00m,  1 },
                    { 7, 4, 9, "b1b10000-0000-0000-0000-000000000001", 1, 120.00m, 0 },
                    { 8, 5, 6, "b1b10000-0000-0000-0000-000000000002", 1, 27.50m,  4 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "ProductId", "CustomerId", "Rating", "Comment", "CreatedAt" },
                values: new object[,]
                {
                    { 1, 1, "b1b10000-0000-0000-0000-000000000003", 5, "Excellent sound quality and very comfortable for long listening sessions.", new DateTime(2026, 9, 12, 11, 0, 0, DateTimeKind.Utc) },
                    { 2, 3, "b1b10000-0000-0000-0000-000000000003", 4, "Great tactile feedback, a bit loud but very responsive for gaming.",         new DateTime(2026, 9, 13,  9, 30, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "Id", "CustomerId", "ProductId", "Quantity", "AddedAt" },
                values: new object[,]
                {
                    { 1, "b1b10000-0000-0000-0000-000000000004", 2,  1, new DateTime(2026, 9, 26, 12, 0, 0, DateTimeKind.Utc) },
                    { 2, "b1b10000-0000-0000-0000-000000000004", 11, 2, new DateTime(2026, 9, 26, 12, 5, 0, DateTimeKind.Utc) },
                    { 3, "b1b10000-0000-0000-0000-000000000005", 12, 1, new DateTime(2026, 9, 27,  7, 50, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "WishlistItems",
                columns: new[] { "Id", "CustomerId", "ProductId", "AddedAt" },
                values: new object[,]
                {
                    { 1, "b1b10000-0000-0000-0000-000000000003", 2, new DateTime(2026, 9, 20, 10, 0, 0, DateTimeKind.Utc) },
                    { 2, "b1b10000-0000-0000-0000-000000000003", 6, new DateTime(2026, 9, 21, 10, 0, 0, DateTimeKind.Utc) },
                    { 3, "b1b10000-0000-0000-0000-000000000004", 9, new DateTime(2026, 9, 23, 10, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse order: children before parents.
            migrationBuilder.DeleteData(table: "WishlistItems", keyColumn: "Id", keyValues: new object[] { 1, 2, 3 });
            migrationBuilder.DeleteData(table: "CartItems", keyColumn: "Id", keyValues: new object[] { 1, 2, 3 });
            migrationBuilder.DeleteData(table: "Reviews", keyColumn: "Id", keyValues: new object[] { 1, 2 });
            migrationBuilder.DeleteData(table: "OrderItems", keyColumn: "Id", keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            migrationBuilder.DeleteData(table: "Orders", keyColumn: "Id", keyValues: new object[] { 1, 2, 3, 4, 5 });
            migrationBuilder.DeleteData(table: "Products", keyColumn: "Id", keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[,]
                {
                    { "b1b10000-0000-0000-0000-000000000001", "9b7208c3-cef5-4482-b312-84506c143202" },
                    { "b1b10000-0000-0000-0000-000000000002", "9b7208c3-cef5-4482-b312-84506c143202" },
                    { "b1b10000-0000-0000-0000-000000000003", "8e46c643-964d-4055-b340-578b10af7929" },
                    { "b1b10000-0000-0000-0000-000000000004", "8e46c643-964d-4055-b340-578b10af7929" },
                    { "b1b10000-0000-0000-0000-000000000005", "8e46c643-964d-4055-b340-578b10af7929" }
                });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValues: new object[]
                {
                    "b1b10000-0000-0000-0000-000000000001",
                    "b1b10000-0000-0000-0000-000000000002",
                    "b1b10000-0000-0000-0000-000000000003",
                    "b1b10000-0000-0000-0000-000000000004",
                    "b1b10000-0000-0000-0000-000000000005"
                });
        }
    }
}
