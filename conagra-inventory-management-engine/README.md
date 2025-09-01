# Conagra Inventory Management Engine

A .NET Core Web API for managing inventory across warehouses and stores using Supabase as the backend database.

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Supabase account and project

### Environment Setup

1. **Clone the repository**
   ```bash
   git clone <your-repo-url>
   cd conagra-inventory-management-engine
   ```

2. **Set up environment variables**
   
   Create a `env.local` file in the project root with your Supabase credentials:
   ```bash
   SUPABASE_URL=https://your-project-ref.supabase.co
   SUPABASE_KEY=your-supabase-anon-key
   ```

   **Important**: Never commit this file to version control! The application will automatically load these environment variables from the `env.local` file.

3. **Set environment variables (Optional)**
   
   The application automatically loads environment variables from the `env.local` file. However, you can also set them manually:
   
   **On macOS/Linux:**
   ```bash
   export SUPABASE_URL="https://your-project-ref.supabase.co"
   export SUPABASE_KEY="your-supabase-anon-key"
   ```
   
   **On Windows:**
   ```cmd
   set SUPABASE_URL=https://your-project-ref.supabase.co
   set SUPABASE_KEY=your-supabase-anon-key
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the API**
   - Swagger UI: `https://localhost:7001/swagger`
   - API Base URL: `https://localhost:7001/api`

## 📊 API Endpoints

### Stores
- `GET /api/stores` - Get all stores
- `GET /api/stores/{storeId}` - Get store by ID

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{productId}` - Get product by ID

### Warehouse
- `GET /api/warehouse` - Get all warehouse inventory
- `GET /api/warehouse/product/{productId}` - Get warehouse inventory by product

### Store Inventory
- `GET /api/storeinventory` - Get all store inventory
- `GET /api/storeinventory/store/{storeId}` - Get store inventory by store
- `GET /api/storeinventory/product/{productId}` - Get store inventory by product
- `GET /api/storeinventory/store/{storeId}/product/{productId}` - Get specific store/product inventory
- `GET /api/storeinventory/below-threshold` - Get stores below threshold

### Inventory Thresholds
- `GET /api/inventorythresholds` - Get all inventory thresholds
- `GET /api/inventorythresholds/store/{storeId}` - Get thresholds by store
- `GET /api/inventorythresholds/product/{productId}` - Get thresholds by product
- `GET /api/inventorythresholds/store/{storeId}/product/{productId}` - Get specific store/product threshold

## 🗄️ Database Schema

The application expects the following tables in your Supabase database:

- `products` (id, name)
- `stores` (id, name, address)
- `warehouse` (id, product_id, quantity)
- `store_inventory` (id, store_id, product_id, quantity)
- `inventory_thresholds` (id, store_id, product_id, threshold_quantity)

## 🔒 Security

- Environment variables are used for sensitive configuration
- RLS (Row Level Security) policies should be configured in Supabase
- The `env.local` file is excluded from version control

## 🏗️ Architecture

- **Controllers**: Handle HTTP requests and responses
- **Services**: Business logic layer
- **Repositories**: Data access layer
- **Models**: Entity models with Supabase attributes

## 📝 License

This project is licensed under the MIT License.
