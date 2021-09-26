use [Sarvar Bank]

create table Users(
	ID int primary key identity not null,
	PhoneNumber		nvarchar(50) not null,
	UserType		char(1)		 not null, -- C - client, A - admin
	FirstName		nvarchar(50) not null,
	MiddleName		nvarchar(50) not null,
	LastName		nvarchar(50) not null,
	BirthDate		datetime	 not null,
	Gender			char(1)		 not null, -- M - male, F - female
	MaritalStatus	char(1)		 not null, -- M - maried, D - divorsed, U - unmarried, W - widow
	CitizenShip		char(1)		 not null, -- T - tajik, F - foreigner 
)