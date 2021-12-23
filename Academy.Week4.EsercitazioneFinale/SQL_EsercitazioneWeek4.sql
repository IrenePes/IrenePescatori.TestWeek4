create database GestioneSpese;

create table Categorie (
Id int not null constraint PK_Categorie primary key identity(1,1),
Categoria varchar(100) not null
);


create table Spese (
Id int not null constraint PK_Spese primary key identity(1,1),
Data datetime not null,
CategoriaId int not null,
Descrizione varchar(500) not null,
Utente varchar(100) not null,
Importo decimal(5,2),
Approvato bit,
constraint FK_SpeseCategorie foreign key(CategoriaId) references Categorie(Id)
);