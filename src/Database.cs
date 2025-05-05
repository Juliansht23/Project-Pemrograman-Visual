using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

public class Database
{
    private static string dbFile = "stockmaster.db";

    public static void InitializeDatabase()
    {
        if (!File.Exists(dbFile))
        {
            SQLiteConnection.CreateFile(dbFile);
        }

        CreateTables();
    }

    private static void CreateTables()
    {
        using (var conn = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
        {
            conn.Open();

            // product
            string sqlRumah = @"
            CREATE TABLE IF NOT EXISTS rumah (
                rumahId INTEGER PRIMARY KEY AUTOINCREMENT,
                alamat TEXT NOT NULL,
                status TEXT
            );";

            // inventory
            string sqlUsers = @"
            CREATE TABLE IF NOT EXISTS users (
                usersId INTEGER PRIMARY KEY AUTOINCREMENT,
                rumahId INTEGER NOT NULL,
                nama TEXT,
                usia TEXT,
                jenis_kelamin TEXT,
                telepon TEXT,
                FOREIGN KEY (rumahId) REFERENCES rumah(rumahId)
            );";


            using (var cmd = new SQLiteCommand(sqlRumah, conn))
            {
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new SQLiteCommand(sqlUsers, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }

    // ====== PRODUCTS ======

    public static List<Rumah> GetRumah()
    {
        List<Rumah> rumah = new();

        using (var conn = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
        {
            conn.Open();
            string sql = "SELECT * FROM rumah";
            using (var cmd = new SQLiteCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    rumah.Add(new Rumah
                    {
                        RumahId = Convert.ToInt32(reader["rumahId"]),
                        Alamat = reader["alamat"].ToString(),
                        Status = reader["status"].ToString()
                    });
                }
            }
        }

        return rumah;
    }

    public static void InsertRumah(Rumah r)
    {
        using (var conn = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
        {
            conn.Open();
            string sql = "INSERT INTO rumah (alamat, status) VALUES (@alamat, @status)";
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@alamat", r.Alamat);
                cmd.Parameters.AddWithValue("@status", r.Status);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void UpdateRumah(Rumah r)
    {
        using (var conn = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
        {
            conn.Open();
            string sql = "UPDATE rumah SET alamat=@alamat, status=@status WHERE rumahId=@id";
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", r.RumahId);
                cmd.Parameters.AddWithValue("@alamat", r.Alamat);
                cmd.Parameters.AddWithValue("@status", r.Status);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteRumah(int rumahId)
    {
        using (var conn = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
        {
            conn.Open();
            string sql = "DELETE FROM rumah WHERE rumahId=@id";
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", rumahId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    // ====== INVENTORY ======

    // public static List<Inventory> GetInventory()
    // {
    //     List<Inventory> inventories = new();

    //     using (var conn = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
    //     {
    //         conn.Open();
    //         string sql = "SELECT * FROM inventory";
    //         using (var cmd = new SQLiteCommand(sql, conn))
    //         using (var reader = cmd.ExecuteReader())
    //         {
    //             while (reader.Read())
    //             {
    //                 inventories.Add(new Inventory
    //                 {
    //                     InventoryId = Convert.ToInt32(reader["inventoryId"]),
    //                     ProductId = Convert.ToInt32(reader["productId"]),
    //                     Quantity = Convert.ToInt32(reader["quantity"]),
    //                     Date = reader["date"].ToString(),
    //                     Type = reader["type"].ToString()
    //                 });
    //             }
    //         }
    //     }

    //     return inventories;
    // }

    // public static void InsertInventory(Inventory i)
    // {
    //     using (var conn = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
    //     {
    //         conn.Open();
    //         string sql = "INSERT INTO inventory (productId, quantity, date, type) VALUES (@pid, @qty, @date, @type)";
    //         using (var cmd = new SQLiteCommand(sql, conn))
    //         {
    //             cmd.Parameters.AddWithValue("@pid", i.ProductId);
    //             cmd.Parameters.AddWithValue("@qty", i.Quantity);
    //             cmd.Parameters.AddWithValue("@date", i.Date);
    //             cmd.Parameters.AddWithValue("@type", i.Type);
    //             cmd.ExecuteNonQuery();
    //         }
    //     }
    // }

    // public static void UpdateInventory(Inventory i)
    // {
    //     using (var conn = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
    //     {
    //         conn.Open();
    //         string sql = "UPDATE inventory SET productId=@pid, quantity=@qty, date=@date, type=@type WHERE inventoryId=@id";
    //         using (var cmd = new SQLiteCommand(sql, conn))
    //         {
    //             cmd.Parameters.AddWithValue("@pid", i.ProductId);
    //             cmd.Parameters.AddWithValue("@qty", i.Quantity);
    //             cmd.Parameters.AddWithValue("@date", i.Date);
    //             cmd.Parameters.AddWithValue("@type", i.Type);
    //             cmd.Parameters.AddWithValue("@id", i.InventoryId);
    //             cmd.ExecuteNonQuery();
    //         }
    //     }
    // }

    // public static void DeleteInventory(int inventoryId)
    // {
    //     using (var conn = new SQLiteConnection($"Data Source={dbFile};Version=3;"))
    //     {
    //         conn.Open();
    //         string sql = "DELETE FROM inventory WHERE inventoryId=@id";
    //         using (var cmd = new SQLiteCommand(sql, conn))
    //         {
    //             cmd.Parameters.AddWithValue("@id", inventoryId);
    //             cmd.ExecuteNonQuery();
    //         }
    //     }
    // }

    // ==== Model Classes ====

    public class Rumah
    {
        public int RumahId { get; set; }
        public string? Alamat { get; set; }
        public string? Status { get; set; }
    }

    // public class Inventory
    // {
    //     public int InventoryId { get; set; }
    //     public int ProductId { get; set; }
    //     public int Quantity { get; set; }
    //     public string? Date { get; set; }
    //     public string? Type { get; set; }
    // }
}