use [Sarvar Bank]

create table Credits (
	ID							int primary key identity	not null,
	Amount						int							not null,
	Duration					int							not null,
	Purpose						char(1)						not null, -- A - ������� �������, R - ������, P - �������, O - ������
	UserPhoneNumber				nvarchar(50)				not null,
	UserIncome					int							not null,
	UserCreditHistory			int							not null,
	UserDelayedCreditHistory	int							not null,
	ProcessingDate				datetime					not null
)