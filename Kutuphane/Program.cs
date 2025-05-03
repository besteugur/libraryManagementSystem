using System;
using System.Data.SqlClient;

namespace Kutuphane
{
    internal class Program
    {
        public class Kitaplar
        {
            public int ID { get; set; }
            public string Baslik { get; set; }
            public string Yazar { get; set; }
            public string Kategori { get; set; }
            public int StokAdedi { get; set; }
        }
        public class Kullanicilar
        {
            public int ID { get; set; }
            public string AdSoyad { get; set; }
            public string Email { get; set; }
            public string Telefon { get; set; }
            public string Adres { get; set; }
        }
        public class Islemler
        {
            public int ID { get; set; }
            public int KullaniciID { get; set; }
            public int KitapID { get; set; }
            public DateTime AlımTarihi { get; set; }
            public DateTime? IadeTarihi { get; set; }
        }


        static SqlConnection connection = new SqlConnection("Data Source=DESKTOP-KK864H4;Initial Catalog=KutuphaneDB;Integrated Security=True;Encrypt=False");

        public static class BookTransactions
        {
            static SqlConnection connection = new SqlConnection("Data Source=DESKTOP-KK864H4;Initial Catalog=KutuphaneDB;Integrated Security=True;Encrypt=False");

            

            public static void ListBooks()
            {
                string query = "Select * from Kitaplar";

                using (var connection = new SqlConnection("Data Source=DESKTOP-KK864H4;Initial Catalog=KutuphaneDB;Integrated Security=True;Encrypt=False"))
                using (var command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Kitap Listesi");
                            while (reader.Read())
                            {
                                Console.WriteLine($"ID: {reader["ID"]}, Başlık: {reader["Baslik"]}, " +
                                                $"Yazar: {reader["Yazar"]}, Kategori: {reader["Kategori"]}, " +
                                                $"Stok Adedi: {reader["StokAdedi"]}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Bir Hata Oluştu: " + ex.Message);
                    }
                }
            }

            
            public static void AddBook()
            {
                try
                {
                    
                    Console.WriteLine("Kitabın Adını Giriniz: ");
                    string baslik = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(baslik))
                    {
                        Console.WriteLine("Kitap adı boş olamaz!");
                        return;
                    }

                    Console.WriteLine("Kitabın Stok Adedini Giriniz: ");
                    if (!int.TryParse(Console.ReadLine(), out int envanter) || envanter < 0)
                    {
                        Console.WriteLine("Geçersiz stok adedi!");
                        return;
                    }

                    Console.WriteLine("Kitabın Yazarını Giriniz: ");
                    string yazar = Console.ReadLine();

                    Console.WriteLine("Kitabın Kategorisini Giriniz: ");
                    string kategori = Console.ReadLine();

                    string query = "Insert into Kitaplar (Baslik, Yazar, Kategori, StokAdedi) Values (@Baslik, @Yazar, @Kategori, @StokAdedi)";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Baslik", baslik);
                    command.Parameters.AddWithValue("@Yazar", yazar);
                    command.Parameters.AddWithValue("@Kategori", kategori);
                    command.Parameters.AddWithValue("@StokAdedi", envanter);

                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Yeni Kitap Eklendi.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Veritabanı hatası: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Beklenmeyen hata: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

            
            public static void DeleteBook()
            {
                Console.WriteLine("Silmek İstediğiniz Kitap ID'sini Giriniz: ");
                int bookId = int.Parse(Console.ReadLine());

                string query = "Delete from Kitaplar Where ID = @ID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", bookId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Kitap Silindi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            
            public static void UpdateBook()
            {
                Console.WriteLine("Güncellemek İstediğiniz Kitap ID'sini Giriniz: ");
                int ID = int.Parse(Console.ReadLine());

                Console.WriteLine("Kitabın Adını Giriniz: ");
                string baslik = Console.ReadLine();

                Console.WriteLine("Kitabın Yazarını Giriniz: ");
                string yazar = Console.ReadLine();

                Console.WriteLine("Kitabın Kategorisini Giriniz: ");
                string kategori = Console.ReadLine();

                Console.WriteLine("Kitabın Stok Adedini Giriniz: ");
                string envanter = Console.ReadLine();

                string query = "Update Kitaplar SET Baslik = @Baslik, Yazar = @Yazar, Kategori = @Kategori, StokAdedi = @StokAdedi WHERE ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@ID", ID);
                command.Parameters.AddWithValue("@Baslik", baslik);
                command.Parameters.AddWithValue("@Yazar", yazar);
                command.Parameters.AddWithValue("@Kategori", kategori);
                command.Parameters.AddWithValue("@StokAdedi", envanter);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Kitap Güncellendi");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public static class UserTransactions
        {


            
            public static void ListUsers()
            {

                string query = "Select * from Kullanicilar";

                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    Console.WriteLine("Kullanıcı Listesi");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ID"]}, AdSoyad: {reader["AdSoyad"]}, Email: {reader["Email"]}, Telefon: {reader["Telefon"]}, Adres: {reader["Adres"]}");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            
            public static void AddUser()
            {
                Console.WriteLine("Kullanıcı AdSoyad Giriniz: ");
                string adSoyad = Console.ReadLine();

                Console.WriteLine("Kullanıcı Email Giriniz: ");
                string email = Console.ReadLine();

                Console.WriteLine("Kullanıcı Telefon Giriniz: ");
                string telefon = Console.ReadLine();

                Console.WriteLine("Kullanıcı Adres Giriniz: ");
                string adres = Console.ReadLine();

                string query = "Insert into Kullanicilar (AdSoyad, Email, Telefon, Adres) values (@AdSoyad, @Email, @Telefon, @Adres)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AdSoyad", adSoyad);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Telefon", telefon);
                command.Parameters.AddWithValue("@Adres", adres);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Yeni Kullanıcı Eklendi");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            
            public static void DeleteUser()
            {
                Console.WriteLine("Silmek İstediğiniz Kullanıcı ID Giriniz: ");
                int userId = int.Parse(Console.ReadLine());

                string query = "Delete from Kullanicilar where ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", userId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Kullanıcı Başarıyla Silindi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            
            public static void UpdateUser()
            {
                Console.WriteLine("Güncellemek İstediğiniz Kullanıcı ID giriniz: ");
                int userId = int.Parse(Console.ReadLine());

                Console.WriteLine("Kullanıcı AdSoyad Giriniz: ");
                string adSoyad = Console.ReadLine();

                Console.WriteLine("Kullanıcı Email Giriniz: ");
                string email = Console.ReadLine();

                Console.WriteLine("Kullanıcı Telefon Giriniz: ");
                string telefon = Console.ReadLine();

                Console.WriteLine("Kullanıcı Adres Giriniz: ");
                string adres = Console.ReadLine();

                string query = "Update Kullanicilar set AdSoyad = @AdSoyad, Email = @Email, Telefon = @Telefon, Adres = @Adres where ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", userId);
                command.Parameters.AddWithValue("@AdSoyad", adSoyad);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Telefon", telefon);
                command.Parameters.AddWithValue("@Adres", adres);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Kullanıcı Güncellendi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static class TransactionTransactions
        {


            
            public static void ListTransactions()
            {
                string query = "Select * from Islemler";

                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    Console.WriteLine("İşlem Listesi");
                    while (reader.Read())
                    {
                        string iadeTarihi;
                        if (reader["IadeTarihi"] == DBNull.Value)
                        {
                            iadeTarihi = "Henüz İade Edilmedi";
                        }
                        else
                        {
                            iadeTarihi = reader["IadeTarihi"].ToString();
                        }

                        Console.WriteLine($"ID: {reader["ID"]}, Kitap ID: {reader["KitapID"]}, Kullanıcı ID: {reader["KullaniciID"]}, AlimTarihi: {reader["AlimTarihi"]}, IadeTarihi: {iadeTarihi}");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            
            public static void AddTransaction()
            {
                Console.WriteLine("Kitap ID'sini Giriniz: ");
                int kitapId = int.Parse(Console.ReadLine());

                Console.WriteLine("Kullanıcı ID'sini Giriniz: ");
                int kullaniciId = int.Parse(Console.ReadLine());

                Console.WriteLine("Alım Tarihini Giriniz: ");
                string alimTarihiInput = Console.ReadLine();
                DateTime alimTarihi = DateTime.Parse(alimTarihiInput);

                Console.WriteLine("İade Tarihini Giriniz (Boş Bırakılabilir): ");
                string iadeTarihiInput = Console.ReadLine();


                string query = "Insert into Islemler (KitapID, KullaniciID, AlimTarihi, IadeTarihi) values (@KitapID, @KullaniciID, @AlimTarihi, @IadeTarihi)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@KitapID", kitapId);
                command.Parameters.AddWithValue("@KullaniciID", kullaniciId);
                command.Parameters.AddWithValue("AlimTarihi", alimTarihi);

                
                if (string.IsNullOrWhiteSpace(iadeTarihiInput))
                {
                    command.Parameters.AddWithValue("@IadeTarihi", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@IadeTarihi", DateTime.Parse(iadeTarihiInput));
                }


                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Yeni İşlem Eklendi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            
            public static void DeleteTransaction()
            {
                Console.WriteLine("Silmek İstediğiniz İşlem ID'sini Giriniz: ");
                int islemId = int.Parse(Console.ReadLine());

                string query = "Delete from Islemler where ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", islemId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("İşlem Silindi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            
            public static void UpdateTransaction()
            {
                Console.WriteLine("Güncellemek İstediğiniz İşlem ID'sini Giriniz: ");
                int islemId = int.Parse(Console.ReadLine());

                Console.WriteLine("Yeni Kitap ID'sini Giriniz: ");
                int kitapId = int.Parse(Console.ReadLine());

                Console.WriteLine("Yeni Kullanıcı ID'sini Giriniz: ");
                int kullaniciId = int.Parse(Console.ReadLine());

                Console.WriteLine("Alım Tarihini Giriniz: ");
                string alimTarihiInput = Console.ReadLine();
                DateTime alimTarihi = DateTime.Parse(alimTarihiInput);

                Console.WriteLine("İade Tarihini Giriniz (Opsiyonel, boş bırakın): ");
                string iadeTarihiInput = Console.ReadLine();


                string query = "Update Islemler Set KitapID = @KitapID, KullaniciID = @KullaniciID, AlimTarihi= @AlimTarihi, IadeTarihi = @IadeTarihi where ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@KitapID", kitapId);
                command.Parameters.AddWithValue("@KullaniciID", kullaniciId);
                command.Parameters.AddWithValue("AlimTarihi", alimTarihi);

                
                if (string.IsNullOrWhiteSpace(iadeTarihiInput))
                {
                    command.Parameters.AddWithValue("@IadeTarihi", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@IadeTarihi", DateTime.Parse(iadeTarihiInput));
                }

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("İşlem Güncellendi.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir Hata Oluştu: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        static void Main(string[] args)
        {
            
            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("Hoşgeldiniz!");
                Console.WriteLine("İşlem Yapılacak Tabloyu Seçiniz (1 - Kitaplar, 2 - Kullanicilar, 3 - Islemler, 0 - Çıkış): ");

                int tableChoice;
                if (int.TryParse(Console.ReadLine(), out tableChoice))
                {

                    switch (tableChoice)
                    {
                        
                        case 1:
                            Console.WriteLine("Kitaplar Tablosunu Seçtiniz");
                            Console.WriteLine("Yapmak İstediğiniz İşlem? (1 - Listeleme, 2 - Ekleme, 3 - Silme, 4 - Güncelleme): ");
                            int kitapTaskChoice = int.Parse(Console.ReadLine());

                            switch (kitapTaskChoice)
                            {
                                case 1:
                                    BookTransactions.ListBooks();
                                    break;
                                case 2:
                                    BookTransactions.AddBook();
                                    break;
                                case 3:
                                    BookTransactions.DeleteBook();
                                    break;
                                case 4:
                                    BookTransactions.UpdateBook();
                                    break;
                                default:
                                    Console.WriteLine("Bir Hata Oluştu");
                                    break;
                            }
                            break;

                        
                        case 2:
                            Console.WriteLine("Kullanıcılar Tablosunu Seçtiniz");
                            Console.WriteLine("Yapmak İstediğiniz İşlem? (1 - Listeleme, 2 - Ekleme, 3 - Silme, 4 - Güncelleme): ");
                            int kullaniciTaskChoice = int.Parse(Console.ReadLine());

                            switch (kullaniciTaskChoice)
                            {
                                case 1:
                                    UserTransactions.ListUsers();
                                    break;
                                case 2:
                                    UserTransactions.AddUser();
                                    break;
                                case 3:
                                    UserTransactions.DeleteUser();
                                    break;
                                case 4:
                                    UserTransactions.UpdateUser();
                                    break;
                                default:
                                    Console.WriteLine("Bir Hata Oluştu");
                                    break;
                            }
                            break;

                        
                        case 3:
                            Console.WriteLine("İşlemler Tablosunu Seçtiniz");
                            Console.WriteLine("Yapmak İstediğiniz İşlem? (1 - Listeleme, 2 - Ekleme, 3 - Silme, 4 - Güncelleme): ");
                            int islemlerTaskChoice = int.Parse(Console.ReadLine());

                            switch (islemlerTaskChoice)
                            {
                                case 1:
                                    TransactionTransactions.ListTransactions();
                                    break;
                                case 2:
                                    TransactionTransactions.AddTransaction();
                                    break;
                                case 3:
                                    TransactionTransactions.DeleteTransaction();
                                    break;
                                case 4:
                                    TransactionTransactions.UpdateTransaction();
                                    break;
                                default:
                                    Console.WriteLine("Bir Hata Oluştu");
                                    break;
                            }
                            break;

                        
                        case 0:
                            isRunning = false;
                            Console.WriteLine("iyi Günler");
                            break;

                        default:
                            Console.WriteLine("Bir Hata Oluştu");
                            break;
                    }
                }
            }
        }
    }
}


