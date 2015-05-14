CREATE DATABASE `polise` ;

USE `polise`;

Create table Ypakovka (
	pk_ypakovka Int NOT NULL AUTO_INCREMENT,
	sposob Varchar(200),
 Primary Key (pk_ypakovka)) ENGINE = MyISAM;

Create table Vesh_dok (
	pk_vesh_dok Int NOT NULL AUTO_INCREMENT,
	priznaki Varchar(500),
	naiminovanie Varchar(200),
	pk_protokol Int NOT NULL,
	pk_material Int NOT NULL,
	pk_ypakovka Int NOT NULL,
 Primary Key (pk_vesh_dok,pk_protokol)) ENGINE = MyISAM;

Create table Polise (
	pk_polise Int NOT NULL AUTO_INCREMENT,
	fio Varchar(200),
	Number Char(200),
	pk_zvanie Int NOT NULL,
	pk_dolgnost Int NOT NULL,
	pk_chin Int NOT NULL,
 Primary Key (pk_polise)) ENGINE = MyISAM;

Create table Protokol (
	pk_protokol Int NOT NULL AUTO_INCREMENT,
	data_sostav Date,
	Vremya_nachala Date,
	vremya_okonch Date,
	mesto_peibitiya Varchar(300),
	soobshil Varchar(200),
	coobshenie Varchar(500),
	predmet_osmotra Varchar(300),
	Number Int,
	Organ Varchar(5000),
	Zayavleniya Varchar(500),
	I_look Varchar(200),
	Sposob_izyatiya Varchar(200),
	temperature Int,
	dead_go Varchar(5000),
	pk_gorod Int NOT NULL,
	pk_pogoda Int NOT NULL,
	pk_osveshennost Int NOT NULL,
	pk_tex_sredstvo Int NOT NULL,
	pk_spec Int NOT NULL,
	pk_prot Int NOT NULL,
	pk_polise Int NOT NULL,
	pk_postanov Int NOT NULL,
	PK_Dela Int NOT NULL,
 Primary Key (pk_protokol)) ENGINE = MyISAM;

Create table ponatoi (
	pk_ponatoi Int NOT NULL AUTO_INCREMENT,
	fio Varchar(200),
	street Varchar(1000),
	house Varchar(200),
	room Varchar(200),
	pk_protokol Int NOT NULL,
 Primary Key (pk_ponatoi)) ENGINE = MyISAM;

Create table sp_prot (
	pk_prot Int NOT NULL AUTO_INCREMENT,
	Name_pro Varchar(1000),
 Primary Key (pk_prot)) ENGINE = MyISAM;

Create table specialist (
	pk_spec Int NOT NULL AUTO_INCREMENT,
	fio Varchar(200),
	pk_special Int NOT NULL,
 Primary Key (pk_spec)) ENGINE = MyISAM;

Create table Spravochnik_dolgnostei (
	pk_dolgnost Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
	id_number Int,
 Primary Key (pk_dolgnost)) ENGINE = MyISAM;

Create table Spravochnik_materialov (
	pk_material Int NOT NULL AUTO_INCREMENT,
	material Varchar(200),
	in_number Int,
 Primary Key (pk_material)) ENGINE = MyISAM;

Create table Spravochnik_tex_sredstv (
	pk_tex_sredstvo Int NOT NULL AUTO_INCREMENT,
	tex_sredstvo Varchar(200),
	id_number Int,
 Primary Key (pk_tex_sredstvo)) ENGINE = MyISAM;

Create table Spravochnik_zvanii (
	pk_zvanie Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
	id_number Int,
 Primary Key (pk_zvanie)) ENGINE = MyISAM;

Create table Spravochnik_gorodov (
	pk_gorod Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
	id_number Int,
 Primary Key (pk_gorod)) ENGINE = MyISAM;

Create table Spravochnik_pogodi (
	pk_pogoda Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
 Primary Key (pk_pogoda)) ENGINE = MyISAM;

Create table Spravochnik_osveshennosti (
	pk_osveshennost Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
 Primary Key (pk_osveshennost)) ENGINE = MyISAM;

Create table Spravochnik_oblastei_spec (
	pk_special Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
	id_number Int,
 Primary Key (pk_special)) ENGINE = MyISAM;

Create table Peoples (
	PK_people Int NOT NULL AUTO_INCREMENT,
	FIO Varchar(200),
	primichanie Varchar(1000),
	pk_protokol Int NOT NULL,
	pk_postanov Int,
 Primary Key (PK_people)) ENGINE = MyISAM;

Create table SpravochnikPod (
	PK_Raiona Double NOT NULL AUTO_INCREMENT,
	Nazv Varchar(200),
	Raion Varchar(200),
	Gorod Varchar(200),
	id_number Int,
 Primary Key (PK_Raiona)) ENGINE = MyISAM;

Create table Delo (
	PK_Dela Int NOT NULL AUTO_INCREMENT,
	Nomer_materiala Varchar(200),
	DateofM Date,
	Nomer_dela Varchar(200),
	DateofV Date,
	DateofPeredachi Date,
	DateofClose Date,
	Comment Varchar(500),
	pk_polise Int NOT NULL,
	PK_Raiona Double NOT NULL,
 Primary Key (PK_Dela)) ENGINE = MyISAM;

Create table Postanovlenie (
	pk_postanov Int NOT NULL AUTO_INCREMENT,
	Obosnovanie Varchar(5000),
	Date Date,
	plase Varchar(200),
	Thing Varchar(5000),
	street Varchar(1000),
	house Varchar(20),
	room Varchar(200),
	pk_post Int NOT NULL,
	pk_polise Int NOT NULL,
	pk_gorod Int NOT NULL,
 Primary Key (pk_postanov)) ENGINE = MyISAM;

Create table Chin (
	pk_chin Int NOT NULL AUTO_INCREMENT,
	nazvanie Varchar(200),
	id_number Int,
 Primary Key (pk_chin)) ENGINE = MyISAM;

Create table sp_pots (
	pk_post Int NOT NULL AUTO_INCREMENT,
	Name_pro Varchar(1000),
 Primary Key (pk_post)) ENGINE = MyISAM;


