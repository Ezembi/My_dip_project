CREATE DATABASE `polise` ;

USE `polise`;

Create table Ypakovka (
	pk_ypakovka Int NOT NULL AUTO_INCREMENT,
	sposob Varchar(200),
 Primary Key (pk_ypakovka)) ENGINE = MyISAM
COMMENT = 'Способы упаковки вещественных доказательств';

Create table Vesh_dok (
	pk_vesh_dok Int NOT NULL AUTO_INCREMENT,
	priznaki Varchar(500),
	naiminovanie Varchar(200),
	pk_protokol Int NOT NULL,
	pk_material Int NOT NULL,
	pk_ypakovka Int NOT NULL,
 Primary Key (pk_vesh_dok,pk_protokol)) ENGINE = MyISAM
COMMENT = 'Вещественное доказательство';

Create table Polise (
	pk_polise Int NOT NULL AUTO_INCREMENT,
	id_number Varchar(200),
	surname Varchar(200),
	Pname Varchar(200),
	second_name Varchar(200),
	pk_zvanie Int NOT NULL,
	pk_dolgnost Int NOT NULL,
	pk_chin Int NOT NULL,
 Primary Key (pk_polise)) ENGINE = MyISAM
COMMENT = 'Уполномоченный';

Create table Protokol (
	pk_protokol Int NOT NULL AUTO_INCREMENT,
	data_sostav Date,
	Vremya_nachala Date,
	vremya_okonch Date,
	mesto_peibitiya Varchar(300),
	soobshil Varchar(200),
	coobshenie Varchar(500),
	predmet_osmotra Varchar(300),
	id_prot Int,
	Organ Varchar(5000),
	Zayavleniya Varchar(500),
	I_look Varchar(200),
	Sposob_izyatiya Varchar(200),
	temperature Varchar(200),
	dead_go Varchar(5000),
	pk_gorod Int NOT NULL,
	pk_pogoda Int NOT NULL,
	pk_osveshennost Int NOT NULL,
	pk_tex_sredstvo Int NOT NULL,
	pk_spec Int NOT NULL,
	pk_polise Int NOT NULL,
	pk_postanov Int,
	PK_Dela Int NOT NULL,
 Primary Key (pk_protokol)) ENGINE = MyISAM
COMMENT = 'Протокол';

Create table ponatoi (
	pk_ponatoi Int NOT NULL AUTO_INCREMENT,
	surname Varchar(200),
	Pname Varchar(200),
	second_name Varchar(200),
	street Varchar(1000),
	house Varchar(200),
	room Varchar(200),
	pk_protokol Int NOT NULL,
 Primary Key (pk_ponatoi)) ENGINE = MyISAM
COMMENT = 'Понятой';

Create table specialist (
	pk_spec Int NOT NULL AUTO_INCREMENT,
	surname Varchar(200),
	Pname Varchar(200),
	second_name Varchar(200),
	pk_special Int NOT NULL,
 Primary Key (pk_spec)) ENGINE = MyISAM
COMMENT = 'Специалист';

Create table Spravochnik_dolgnostei (
	pk_dolgnost Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
	id_number Varchar(200),
 Primary Key (pk_dolgnost)) ENGINE = MyISAM
COMMENT = 'Справочник_должностей';

Create table Spravochnik_materialov (
	pk_material Int NOT NULL AUTO_INCREMENT,
	material Varchar(200),
	in_number Varchar(200),
 Primary Key (pk_material)) ENGINE = MyISAM
COMMENT = 'Справочник материалов, в которые упаковываю вещественные доказательства (полиэтилен, бумага и т.д.
)';

Create table Spravochnik_tex_sredstv (
	pk_tex_sredstvo Int NOT NULL AUTO_INCREMENT,
	tex_sredstvo Varchar(200),
	id_number Varchar(200),
 Primary Key (pk_tex_sredstvo)) ENGINE = MyISAM
COMMENT = 'Справочник_тех_средств';

Create table Spravochnik_zvanii (
	pk_zvanie Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
	id_number Varchar(200),
 Primary Key (pk_zvanie)) ENGINE = MyISAM
COMMENT = 'Справочник_званий';

Create table Spravochnik_gorodov (
	pk_gorod Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
	id_number Varchar(200),
 Primary Key (pk_gorod)) ENGINE = MyISAM
COMMENT = 'Справочник_городов';

Create table Spravochnik_pogodi (
	pk_pogoda Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
 Primary Key (pk_pogoda)) ENGINE = MyISAM
COMMENT = 'Справочник_погодных_условий';

Create table Spravochnik_osveshennosti (
	pk_osveshennost Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
 Primary Key (pk_osveshennost)) ENGINE = MyISAM
COMMENT = 'Справочник_освещённости';

Create table Spravochnik_oblastei_spec (
	pk_special Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
	id_number Varchar(200),
 Primary Key (pk_special)) ENGINE = MyISAM
COMMENT = 'Справочник_областей_специализации';

Create table Peoples (
	PK_people Int NOT NULL AUTO_INCREMENT,
	surname Varchar(200),
	Pname Varchar(200),
	second_name Varchar(200),
	primichanie Varchar(1000),
	mystate Varchar(20),
	pk_postanov Int,
	pk_protokol Int NOT NULL,
	pk_pol Int NOT NULL,
 Primary Key (PK_people)) ENGINE = MyISAM
COMMENT = 'Лицо в постановлении (другие лица)';

Create table Spravochnik_Pod (
	PK_Raiona Int NOT NULL AUTO_INCREMENT,
	Nazv Varchar(200),
	Raion Varchar(200),
	id_number Varchar(200),
	pk_gorod Int NOT NULL,
 Primary Key (PK_Raiona)) ENGINE = MyISAM
COMMENT = 'Справочник подразделений следственного комитета';

Create table Delo (
	PK_Dela Int NOT NULL AUTO_INCREMENT,
	Nomer_materiala Varchar(200),
	DateofM Date,
	Nomer_dela Varchar(200),
	DateofV Date,
	DateofPeredachi Date,
	DateofClose Date,
	Comment Varchar(1000),
	pk_polise Int NOT NULL,
	PK_Raiona Int NOT NULL,
 Primary Key (PK_Dela)) ENGINE = MyISAM
COMMENT = 'Уголовное дело (материал проверки)';

Create table Postanovlenie (
	pk_postanov Int NOT NULL AUTO_INCREMENT,
	Obosnovanie Varchar(5000),
	DateOfCreate Date,
	plase Varchar(5000),
	street Varchar(1000),
	house Varchar(20),
	room Varchar(200),
	id_post Int,
	pk_polise Int NOT NULL,
	pk_gorod Int NOT NULL,
	pk_prosecutor1 Int NOT NULL,
	pk_court1 Int NOT NULL,
	pk_prosecutor2 Int NOT NULL,
	pk_court2 Int NOT NULL,
	pk_dolgnost Int NOT NULL,
 Primary Key (pk_postanov)) ENGINE = MyISAM
COMMENT = 'Постановление';

Create table Chin (
	pk_chin Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
	id_number Varchar(200),
 Primary Key (pk_chin)) ENGINE = MyISAM
COMMENT = 'Справочник классных чинов';

Create table sp_pro_pol (
	pk_pol Int NOT NULL AUTO_INCREMENT,
	Name_pro Varchar(1000),
	id_number Varchar(200),
 Primary Key (pk_pol)) ENGINE = MyISAM
COMMENT = 'Справочник процессуальных положений';

Create table prosecutor (
	pk_prosecutor Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
	id_number Varchar(200),
 Primary Key (pk_prosecutor)) ENGINE = MyISAM
COMMENT = 'Наименование органа прокуратуры';

Create table court (
	pk_court Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
	id_number Varchar(200),
 Primary Key (pk_court)) ENGINE = MyISAM
COMMENT = 'Наименование суда';