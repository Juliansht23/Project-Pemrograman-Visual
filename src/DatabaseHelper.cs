using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

public static class DatabaseHelper
{
    private static readonly string DbFile = "perumahan.db";

    public static void InitializeDatabase()
    {
        if (!File.Exists(DbFile))
        {
            SQLiteConnection.CreateFile(DbFile);
        }

        CreateTables();
    }

    private static void CreateTables()
    {
        using var conn = new SQLiteConnection($"Data Source={DbFile};Version=3;");
        conn.Open();

        ExecuteNonQuery(conn, @"
            CREATE TABLE IF NOT EXISTS rumah (
                rumahId INTEGER PRIMARY KEY AUTOINCREMENT,
                alamat TEXT NOT NULL,
                status TEXT
            );
        ");

        ExecuteNonQuery(conn, @"
            CREATE TABLE IF NOT EXISTS users (
                usersId INTEGER PRIMARY KEY AUTOINCREMENT,
                rumahId INTEGER NOT NULL,
                nama TEXT,
                usia TEXT,
                jenis_kelamin TEXT,
                telepon TEXT,
                FOREIGN KEY (rumahId) REFERENCES rumah(rumahId)
            );
        ");
    }

    private static void ExecuteNonQuery(SQLiteConnection conn, string sql)
    {
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.ExecuteNonQuery();
    }

    // ====== CRUD: Rumah ======

    public static List<Rumah> GetRumah()
    {
        var rumahList = new List<Rumah>();

        using var conn = new SQLiteConnection($"Data Source={DbFile};Version=3;");
        conn.Open();

        string sql = "SELECT * FROM rumah";
        using var cmd = new SQLiteCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            rumahList.Add(new Rumah
            {
                RumahId = Convert.ToInt32(reader["rumahId"]),
                Alamat = reader["alamat"]?.ToString(),
                Status = reader["status"]?.ToString()
            });
        }

        return rumahList;
    }

    public static void InsertRumah(Rumah r)
    {
        using var conn = new SQLiteConnection($"Data Source={DbFile};Version=3;");
        conn.Open();

        string sql = "INSERT INTO rumah (alamat, status) VALUES (@alamat, @status)";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@alamat", r.Alamat);
        cmd.Parameters.AddWithValue("@status", r.Status);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateRumah(Rumah r)
    {
        using var conn = new SQLiteConnection($"Data Source={DbFile};Version=3;");
        conn.Open();

        string sql = "UPDATE rumah SET alamat = @alamat, status = @status WHERE rumahId = @id";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", r.RumahId);
        cmd.Parameters.AddWithValue("@alamat", r.Alamat);
        cmd.Parameters.AddWithValue("@status", r.Status);
        cmd.ExecuteNonQuery();
    }

    public static void DeleteRumah(int rumahId)
    {
        using var conn = new SQLiteConnection($"Data Source={DbFile};Version=3;");
        conn.Open();

        string sql = "DELETE FROM rumah WHERE rumahId = @id";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", rumahId);
        cmd.ExecuteNonQuery();
    }

    // ====== CRUD: Users ======

    public static List<Users> GetUsers()
    {
        var usersList = new List<Users>();

        using var conn = new SQLiteConnection($"Data Source={DbFile};Version=3;");
        conn.Open();

        string sql = "SELECT * FROM users";
        using var cmd = new SQLiteCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            usersList.Add(new Users
            {
                UsersId = Convert.ToInt32(reader["usersId"]),
                RumahId = Convert.ToInt32(reader["rumahId"]),
                Nama = reader["nama"]?.ToString(),
                Usia = reader["usia"]?.ToString(),
                JenisKelamin = reader["jenis_kelamin"]?.ToString(),
                Telepon = reader["telepon"]?.ToString()
            });
        }

        return usersList;
    }

    public static void InsertUser(Users u)
    {
        using var conn = new SQLiteConnection($"Data Source={DbFile};Version=3;");
        conn.Open();

        string sql = @"
            INSERT INTO users (rumahId, nama, usia, jenis_kelamin, telepon)
            VALUES (@rumahId, @nama, @usia, @jenis_kelamin, @telepon)";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@rumahId", u.RumahId);
        cmd.Parameters.AddWithValue("@nama", u.Nama);
        cmd.Parameters.AddWithValue("@usia", u.Usia);
        cmd.Parameters.AddWithValue("@jenis_kelamin", u.JenisKelamin);
        cmd.Parameters.AddWithValue("@telepon", u.Telepon);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateUser(Users u)
    {
        using var conn = new SQLiteConnection($"Data Source={DbFile};Version=3;");
        conn.Open();

        string sql = @"
            UPDATE users
            SET rumahId = @rumahId,
                nama = @nama,
                usia = @usia,
                jenis_kelamin = @jenis_kelamin,
                telepon = @telepon
            WHERE usersId = @usersId";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@usersId", u.UsersId);
        cmd.Parameters.AddWithValue("@rumahId", u.RumahId);
        cmd.Parameters.AddWithValue("@nama", u.Nama);
        cmd.Parameters.AddWithValue("@usia", u.Usia);
        cmd.Parameters.AddWithValue("@jenis_kelamin", u.JenisKelamin);
        cmd.Parameters.AddWithValue("@telepon", u.Telepon);
        cmd.ExecuteNonQuery();
    }

    public static void DeleteUser(int usersId)
    {
        using var conn = new SQLiteConnection($"Data Source={DbFile};Version=3;");
        conn.Open();

        string sql = "DELETE FROM users WHERE usersId = @id";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", usersId);
        cmd.ExecuteNonQuery();
    }


    // ==== Model Classes ====

    public class Rumah
    {
        public int RumahId { get; set; }
        public string? Alamat { get; set; }
        public string? Status { get; set; }
    }

    public class Users
    {
        public int UsersId { get; set; }
        public int RumahId { get; set; }
        public string? Nama { get; set; }
        public string? Usia { get; set; }
        public string? JenisKelamin { get; set; }
        public string? Telepon { get; set; }
    }
}
