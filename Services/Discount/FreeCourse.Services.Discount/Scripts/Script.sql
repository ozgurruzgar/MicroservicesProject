create table Discount
(
	Id serial primary key,
	UserId varchar(100) not null,
	Rate smallint not null,
	Code varchar(30) not null,
	CreatedDate timestamp not null default CURRENT_TIMESTAMP
)