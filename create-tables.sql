Create Database KutuphaneDB

GO

Use KutuphaneDB;

GO

Create table Kitaplar(
ID int primary KEY identity(1,1) not null,
Baslik nvarchar(100) not null,
Yazar nvarchar(50) not null,
Kategori nvarchar(50) not null,
Stokadedi int not null,
OlusturmaTarihi datetime default getdate() NOT NULL
);


GO


Create table Kullanicilar(
ID int primary key identity(1,1) not null,
AdSoyad nvarchar(100) not null,
Email nvarchar(100) not null,
Telefon nvarchar(15) not null,
Adres nvarchar(250) not null,
OlusturulmaTarihi datetime default getdate() NOT NULL
);

GO

Create Table Islemler(
ID int primary key identity(1,1) not null,
KullaniciID int not null foreign key references Kullanicilar(ID),
KitapID int not null foreign key references Kitaplar(ID),
AlimTarihi datetime not null,
IadeTarihi datetime null,
OlusturulmaTarihi datetime default getdate() NOT NULL
);

GO