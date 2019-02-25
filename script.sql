create tablespace sync
  DATAFILE 'C:\oradata\sync.dbf' 
    SIZE 10M
    AUTOEXTEND ON NEXT 10M MAXSIZE 200M;
	
create user sync
identified by sync
default tablespace sync;

create table zpatch_hdim (       
zpatch_id NUMBER(20),
parent_id NUMBER(20),
cpatch_id NUMBER(20),
zpatch_name VARCHAR2(50),
validfrom DATE ,
validto DATE,
dwsact VARCHAR2(1),
zpatchstatus VARCHAR2(50))
TABLESPACE sync;

create table cpatch_hdim  (       
cpatch_id NUMBER(20),
parent_id NUMBER(20),
release_id NUMBER(20),
cpatch_name VARCHAR2(50),
validfrom DATE ,
validto DATE,
dwsact VARCHAR2(1),
cpatchstatus VARCHAR2(50),
kod_sredy VARCHAR2(50))
TABLESPACE sync;

create table release_hdim  (       
release_id NUMBER(20),
release_name VARCHAR2(50),
validfrom DATE ,
validto DATE,
dwsact VARCHAR2(1))
TABLESPACE sync;

create table zpatchorder_hdim  (       
zpatch_id NUMBER(20),
order NUMBER(20),
validfrom DATE ,
validto DATE,
dwsact VARCHAR2(1))
TABLESPACE sync;